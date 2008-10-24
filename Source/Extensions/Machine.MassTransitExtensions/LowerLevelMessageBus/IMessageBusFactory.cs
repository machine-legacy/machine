using System;
using System.Collections.Generic;

using MassTransit.ServiceBus.Internal;

namespace Machine.MassTransitExtensions.LowerLevelMessageBus
{
  public interface IMessageBusFactory
  {
    IMessageBus CreateMessageBus(EndpointName endpointName);
  }
  public class MessageBusFactory : IMessageBusFactory
  {
    private readonly IEndpointResolver _endpointResolver;
    private readonly IMassTransitUriFactory _uriFactory;
    private readonly IMessageEndpointLookup _messageEndpointLookup;
    private readonly TransportMessageBodySerializer _transportMessageBodySerializer;
    private readonly MessageDispatcher _messageDispatcher;

    public MessageBusFactory(IEndpointResolver endpointResolver, IMassTransitUriFactory uriFactory, IMessageEndpointLookup messageEndpointLookup, TransportMessageBodySerializer transportMessageBodySerializer, MessageDispatcher messageDispatcher)
    {
      _endpointResolver = endpointResolver;
      _uriFactory = uriFactory;
      _messageEndpointLookup = messageEndpointLookup;
      _transportMessageBodySerializer = transportMessageBodySerializer;
      _messageDispatcher = messageDispatcher;
    }

    #region IMessageBusFactory Members
    public IMessageBus CreateMessageBus(EndpointName endpointName)
    {
      return new MessageBus(_endpointResolver, _uriFactory, _messageEndpointLookup, _transportMessageBodySerializer, _messageDispatcher, endpointName);
    }
    #endregion
  }
}
