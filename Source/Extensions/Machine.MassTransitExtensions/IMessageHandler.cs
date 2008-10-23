using System;
using MassTransit.ServiceBus;

namespace Machine.MassTransitExtensions
{
  public interface IMessageHandler<T> : Consumes<T>.All where T : class, IMessage
  {
  }
}