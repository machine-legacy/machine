using MassTransit.ServiceBus;
using MassTransit.ServiceBus.Subscriptions;

namespace Machine.MassTransitExtensions
{
  public class SubscriptionManagerService : AbstractSubscriptionService
  {
    public SubscriptionManagerService(IStandardServiceBuses buses, ISubscriptionCache subscriptionCache, ISubscriptionRepository subscriptionRepository)
      : base(new SubscriptionService(buses.DefaultServiceBus, subscriptionCache, subscriptionRepository))
    {
    }
  }
}