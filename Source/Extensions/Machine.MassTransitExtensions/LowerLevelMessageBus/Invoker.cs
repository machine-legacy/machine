using System;
using System.Collections.Generic;

namespace Machine.MassTransitExtensions.LowerLevelMessageBus
{
  public class Invoker<T> : IInvoker where T : IMessage
  {
    #region IInvoker Members
    public void Dispatch(IMessage message, object handler)
    {
      IMessageHandler<T> genericHandler = (IMessageHandler<T>)handler;
      genericHandler.Handle((T)message);
    }
    #endregion
  }
  
  public interface IInvoker
  {
    void Dispatch(IMessage message, object handler);
  }
  
  public static class Invokers
  {
    public static IInvoker CreateFor(Type messageType)
    {
      return (IInvoker)Activator.CreateInstance(typeof(Invoker<>).MakeGenericType(messageType));
    }
  }
}