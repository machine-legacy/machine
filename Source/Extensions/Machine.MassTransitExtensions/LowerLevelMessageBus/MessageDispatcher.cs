using System;
using System.Collections.Generic;

using Machine.Container.Services;

using MassTransit.ServiceBus;

namespace Machine.MassTransitExtensions.LowerLevelMessageBus
{
  public class MessageDispatcher
  {
    private readonly IMachineContainer _container;

    public MessageDispatcher(IMachineContainer container)
    {
      _container = container;
    }

    public void Dispatch(IMessage message)
    {
      Type messageType = message.GetType();
      IList<object> handlers = _container.Resolve.All(delegate(Type handlerType) {
        return IsAcceptableHandler(handlerType, messageType);
      });
      foreach (object handler in handlers)
      {
        foreach (HandlerConsumption consumption in EnumerateHandlerImplementationsOf(handler.GetType(), messageType))
        {
          IInvoker invoker = Invokers.CreateFor(consumption.MessageType);
          invoker.Dispatch(message, handler);
        }
      }
    }

    private static Type MakeHandlerType(Type messageType)
    {
      return typeof(Consumes<>.All).MakeGenericType(messageType);
    }

    private static IEnumerable<HandlerConsumption> EnumerateHandlerImplementationsOf(Type handlerType, Type messageType)
    {
      foreach (Type interfaceType in handlerType.GetInterfaces())
      {
        if (interfaceType.GetGenericArguments().Length != 1)
        {
          continue;
        }
        Type wouldBeMessageType = interfaceType.GetGenericArguments()[0];
        if (!wouldBeMessageType.IsAssignableFrom(messageType))
        {
          continue;
        }
        if (MakeHandlerType(wouldBeMessageType).IsAssignableFrom(interfaceType))
        {
          yield return new HandlerConsumption(handlerType, wouldBeMessageType, interfaceType);
        }
      }
    }

    private static bool IsAcceptableHandler(Type handlerType, Type messageType)
    {
      foreach (HandlerConsumption consumption in EnumerateHandlerImplementationsOf(handlerType, messageType))
      {
        return true;
      }
      return false;
    }
  }

  public class HandlerConsumption
  {
    readonly Type _handlerType;
    readonly Type _messageType;
    readonly Type _interfaceType;

    public Type HandlerType
    {
      get { return _handlerType; }
    }

    public Type MessageType
    {
      get { return _messageType; }
    }

    public Type InterfaceType
    {
      get { return _interfaceType; }
    }

    public HandlerConsumption(Type handlerType, Type messageType, Type interfaceType)
    {
      _handlerType = handlerType;
      _messageType = messageType;
      _interfaceType = interfaceType;
    }
  }
}