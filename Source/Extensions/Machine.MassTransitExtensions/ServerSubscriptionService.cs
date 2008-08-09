using MassTransit.ServiceBus;
using MassTransit.ServiceBus.Subscriptions;

namespace Machine.MassTransitExtensions
{
  public class ServerSubscriptionService : AbstractSubscriptionService
  {
    public ServerSubscriptionService(IServiceBus bus, ISubscriptionCache subscriptionCache, IStandardEndpoints standardEndpoints)
      : base(new SubscriptionClient(bus, subscriptionCache, standardEndpoints.SubscriptionManagerEndpoint))
    {
    }
  }
}