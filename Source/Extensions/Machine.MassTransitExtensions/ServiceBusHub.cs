using MassTransit.ServiceBus;
using MassTransit.ServiceBus.Subscriptions;

namespace Machine.MassTransitExtensions
{
  public class ServiceBusHub : IServiceBusHub
  {
    private readonly IServiceBus _bus;
    private readonly ISubscriptionCache _subscriptionCache;
    private readonly ISubscriptionService _subscriptionService;

    public ServiceBusHub(IServiceBus bus, ISubscriptionCache subscriptionCache, ISubscriptionService subscriptionService)
    {
      _bus = bus;
      _subscriptionCache = subscriptionCache;
      _subscriptionService = subscriptionService;
    }

    #region IServiceBusHub Members
    public IServiceBus Bus
    {
      get { return _bus; }
    }

    public ISubscriptionCache SubscriptionCache
    {
      get { return _subscriptionCache; }
    }

    public ISubscriptionService SubscriptionService
    {
      get { return _subscriptionService; }
    }
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
      _subscriptionCache.Dispose();
      _subscriptionService.Dispose();
      _bus.Dispose();
    }
    #endregion

    #region IHostedService Members
    public void Start()
    {
      _subscriptionService.Start();
    }

    public void Stop()
    {
      _subscriptionService.Stop();
    }
    #endregion
  }
}