using MassTransit.ServiceBus;
using MassTransit.ServiceBus.Subscriptions;

namespace Machine.MassTransitExtensions
{
  public interface IServiceBusHub : IHostedService
  {
    IServiceBus Bus
    {
      get;
    }

    ISubscriptionCache SubscriptionCache
    {
      get;
    }

    ISubscriptionService SubscriptionService
    {
      get;
    }
  }
}