using System;
using System.Collections.Generic;

namespace Machine.MassTransitExtensions.InterfacesAsMessages
{
  public class MessageFactory : IMessageFactory
  {
    private readonly MessageInterfaceImplementations _messageInterfaceImplementor;

    public MessageFactory(MessageInterfaceImplementations messageInterfaceImplementor)
    {
      _messageInterfaceImplementor = messageInterfaceImplementor;
    }

    #region IMessageFactory Members
    public IMessage Create(Type type)
    {
      Type implementation = _messageInterfaceImplementor.GetClassFor(type);
      if (implementation == null || !type.IsAssignableFrom(implementation))
      {
        throw new InvalidOperationException();
      }
      return (IMessage)Activator.CreateInstance(implementation);
    }

    public T Create<T>() where T : class, IMessage
    {
      return (T)Create(typeof(T));
    }
    #endregion
  }
}
