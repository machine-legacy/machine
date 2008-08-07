using System;

using MassTransit.ServiceBus;

namespace Machine.MassTransitExtensions
{
  public interface ISubscriptionService : IDisposable
  {
    void Start();
    void Stop();
  }
  public abstract class AbstractSubscriptionService : ISubscriptionService
  {
    private readonly IHostedService _hostedService;

    protected AbstractSubscriptionService(IHostedService hostedService)
    {
      _hostedService = hostedService;
    }

    #region IHostedService Members
    public void Start()
    {
      _hostedService.Start();
    }

    public void Stop()
    {
      _hostedService.Stop();
    }
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
      _hostedService.Dispose();
    }
    #endregion
  }
}