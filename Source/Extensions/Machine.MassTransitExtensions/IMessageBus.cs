using System;
using System.Collections.Generic;

namespace Machine.MassTransitExtensions
{
  public interface IMessageBus : IDisposable
  {
    EndpointName Address { get; }
    void Start();
    void Send<T>(params T[] messages) where T : class, IMessage;
    void Send<T>(EndpointName destination, params T[] messages) where T : class, IMessage;
    void Stop();
    IRequestReplyBuilder Request<T>(params T[] messages) where T : class, IMessage;
    void Reply<T>(params T[] messages) where T : class, IMessage;
  }
  public interface IRequestReplyBuilder
  {
    void OnReply(AsyncCallback callback, object state);
    void OnReply(AsyncCallback callback);
  }
}
