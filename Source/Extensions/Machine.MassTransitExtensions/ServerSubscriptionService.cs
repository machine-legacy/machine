using MassTransit.ServiceBus;
using MassTransit.ServiceBus.Internal;
using MassTransit.ServiceBus.Subscriptions;

namespace Machine.MassTransitExtensions
{
  public class ServerSubscriptionService : AbstractSubscriptionService
  {
    public ServerSubscriptionService(IServiceBus bus, ISubscriptionCache subscriptionCache, IEndpointResolver endpointResolver, MassTransitConfiguration configuration)
      : base(new SubscriptionClient(bus, subscriptionCache, endpointResolver.Resolve(configuration.SubscriptionEndpointUri)))
    {
    }
  }
}