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
      IList<object> handlers = _container.Resolve.All(IsAcceptableHandler);
      foreach (object handler in handlers)
      {
        foreach (Type handlerType in EnumerateHandlerImplementationsOf(handler.GetType()))
        {
          IInvoker invoker = Invokers.CreateFor(handlerType.GetGenericArguments()[0]);
          invoker.Dispatch(message, handler);
        }
      }
    }

    private static Type MakeHandlerType(Type messageType)
    {
      return typeof(Consumes<>.All).MakeGenericType(messageType);
    }

    private static IEnumerable<Type> EnumerateHandlerImplementationsOf(Type type)
    {
      foreach (Type interfaceType in type.GetInterfaces())
      {
        if (interfaceType.GetGenericArguments().Length != 1)
        {
          continue;
        }
        Type wouldBeMessageType = interfaceType.GetGenericArguments()[0];
        if (!typeof(IMessage).IsAssignableFrom(wouldBeMessageType))
        {
          continue;
        }
        if (MakeHandlerType(wouldBeMessageType).IsAssignableFrom(interfaceType))
        {
          yield return interfaceType;
        }
      }
    }

    private static bool IsAcceptableHandler(Type type)
    {
      foreach (Type handlerType in EnumerateHandlerImplementationsOf(type))
      {
        return true;
      }
      return false;
    }
  }
}