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
      IList<object> handlers = _container.Resolve.All(delegate(Type handlerType) {
        return IsAcceptableHandler(handlerType, message.GetType());
      });
      foreach (object handler in handlers)
      {
        foreach (Type interfaceType in EnumerateHandlerImplementationsOf(handler.GetType(), message.GetType()))
        {
          IInvoker invoker = Invokers.CreateFor(interfaceType.GetGenericArguments()[0]);
          invoker.Dispatch(message, handler);
        }
      }
    }

    private static Type MakeHandlerType(Type messageType)
    {
      return typeof(Consumes<>.All).MakeGenericType(messageType);
    }

    private static IEnumerable<Type> EnumerateHandlerImplementationsOf(Type handlerType, Type messageType)
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
          yield return interfaceType;
        }
      }
    }

    private static bool IsAcceptableHandler(Type handlerType, Type messageType)
    {
      foreach (Type interfaceType in EnumerateHandlerImplementationsOf(handlerType, messageType))
      {
        return true;
      }
      return false;
    }
  }
}