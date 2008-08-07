using System;

namespace Machine.MassTransitExtensions
{
  public interface IMassTransit : IDisposable
  {
    void Start();
    void Stop();
  }
}