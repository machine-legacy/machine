using System;
using System.Collections.Generic;

namespace Machine.MassTransitExtensions.InterfacesAsMessages
{
  public interface IMessageFactory
  {
    IMessage Create(Type type);
    T Create<T>() where T : class, IMessage;
  }
}
