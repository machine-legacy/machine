using System;
using System.Collections.Generic;

using MassTransit.ServiceBus;
using MassTransit.ServiceBus.Internal;
using MassTransit.ServiceBus.Threading;

namespace Machine.MassTransitExtensions.LowerLevelMessageBus
{
  public class MessageBus : IMessageBus
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(IMessageBus));
    private readonly IMassTransitUriFactory _uriFactory;
    private readonly IMessageEndpointLookup _messageEndpointLookup;
    private readonly IEndpointResolver _endpointResolver;
    private readonly IEndpoint _endpoint;
    private readonly EndpointName _endpointName;
    private readonly ResourceThreadPool<IEndpoint, object> _threads;
    private readonly TransportMessageSerializer _transportMessageSerializer;
    private readonly MessageDispatcher _dispatcher;

    public MessageBus(IEndpointResolver endpointResolver, IMassTransitUriFactory uriFactory, IMessageEndpointLookup messageEndpointLookup, TransportMessageSerializer transportMessageSerializer, MessageDispatcher dispatcher, EndpointName endpointName)
    {
      _endpointResolver = endpointResolver;
      _dispatcher = dispatcher;
      _transportMessageSerializer = transportMessageSerializer;
      _messageEndpointLookup = messageEndpointLookup;
      _uriFactory = uriFactory;
      _endpoint = _endpointResolver.Resolve(_uriFactory.CreateUri(endpointName));
      _messageEndpointLookup = messageEndpointLookup;
      _endpointName = endpointName;
      _threads = new ResourceThreadPool<IEndpoint, object>(_endpoint, EndpointReader, EndpointDispatcher, 10, 1, 1);
    }

    public EndpointName Address
    {
      get { return _endpointName; }
    }

    public void Start()
    {
    }

    public void Send<T>(params T[] messages) where T : class, IMessage
    {
      foreach (EndpointName destination in _messageEndpointLookup.LookupEndpointFor(typeof(T)))
      {
        Send(destination, messages);
      }
    }

    public void Send<T>(EndpointName destination, params T[] messages) where T : class, IMessage
    {
      Uri uri = _uriFactory.CreateUri(destination);
      IEndpoint endpoint = _endpointResolver.Resolve(uri);
      endpoint.Send(_transportMessageSerializer.Serialize(this, messages));
    }

    public void Stop()
    {
      _threads.Dispose();
    }

    public void Dispose()
    {
      Stop();
    }

    private static object EndpointReader(IEndpoint resource)
    {
      try
      {
        return resource.Receive(TimeSpan.FromSeconds(3), Accept);
      }
      catch (Exception error)
      {
        _log.Error(error);
        return null;
      }
    }

    private static bool Accept(object obj)
    {
      return obj is TransportMessage;
    }

    private void EndpointDispatcher(object obj)
    {
			if (obj == null)
			{
			  return;
			}
      try
      {
        TransportMessage transportMessage = (TransportMessage)obj;
        using (CurrentMessageContext currentMessageContext = CurrentMessageContext.Open(transportMessage))
        {
          ICollection<IMessage> messages = _transportMessageSerializer.Deserialize(transportMessage);
          foreach (IMessage message in messages)
          {
            currentMessageContext.CurrentApplicationMessage = message;
            _dispatcher.Dispatch(message);
          }
        }
      }
      catch (Exception error)
      {
        _log.Error(error);
        throw;
      }
    }

    public void Reply<T>(params T[] messages) where T : class, IMessage
    {
      EndpointName returnAddress = CurrentMessageContext.Current.TransportMessage.ReturnAddress;
      Send(returnAddress, messages);
    }
  }
}
