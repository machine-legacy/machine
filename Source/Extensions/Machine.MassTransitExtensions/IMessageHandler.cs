using System;

namespace Machine.MassTransitExtensions
{
  public interface IMessageHandler<T> where T : IMessage
  {
    void Handle(T message);
  }
}