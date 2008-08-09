using MassTransit.ServiceBus;
using MassTransit.ServiceBus.Subscriptions;

namespace Machine.MassTransitExtensions
{
  public class ServerSubscriptionService : AbstractSubscriptionService
  {
    public ServerSubscriptionService(IStandardServiceBuses buses, ISubscriptionCache subscriptionCache, IStandardEndpoints standardEndpoints)
      : base(new SubscriptionClient(buses.DefaultServiceBus, subscriptionCache, standardEndpoints.SubscriptionManagerEndpoint))
    {
    }
  }
}