using System;

using MassTransit.ServiceBus;
using MassTransit.ServiceBus.Internal;
using MassTransit.ServiceBus.Subscriptions;

namespace Machine.MassTransitExtensions
{
  public class ServiceBusHubFactory : IServiceBusHubFactory
  {
    private readonly MassTransitConfiguration _configuration;
    private readonly IMassTransitUriFactory _uriFactory;
    private readonly IObjectBuilder _objectBuilder;
    private readonly IEndpointResolver _endpointResolver;
    private readonly ISubscriptionRepository _subscriptionRepository;

    public ServiceBusHubFactory(MassTransitConfiguration configuration, IEndpointResolver endpointResolver, IObjectBuilder objectBuilder, IMassTransitUriFactory uriFactory, ISubscriptionRepository subscriptionRepository)
    {
      _configuration = configuration;
      _endpointResolver = endpointResolver;
      _subscriptionRepository = subscriptionRepository;
      _objectBuilder = objectBuilder;
      _uriFactory = uriFactory;
    }

    #region IServiceBusHubFactory Members
    public IServiceBusHub CreateServerHub(Uri uri)
    {
      ISubscriptionCache subscriptionCache = new LocalSubscriptionCache();
      IEndpoint endpoint = _endpointResolver.Resolve(_uriFactory.CreateUri(uri));
      IServiceBus bus = new ServiceBus(endpoint, _objectBuilder, subscriptionCache, _endpointResolver);
      ISubscriptionService subscriptionService = new ServerSubscriptionService(bus, subscriptionCache, this.SubscriptionManagerEndpoint);
      return new ServiceBusHub(bus, subscriptionCache, subscriptionService);
    }

    public IServiceBusHub CreateSubscriptionManagerHub()
    {
      ISubscriptionCache subscriptionCache = new LocalSubscriptionCache();
      IEndpoint endpoint = this.SubscriptionManagerEndpoint;
      IServiceBus bus = new ServiceBus(endpoint, _objectBuilder, subscriptionCache, _endpointResolver);
      ISubscriptionService subscriptionService = new SubscriptionManagerService(bus, subscriptionCache, _subscriptionRepository);
      return new ServiceBusHub(bus, subscriptionCache, subscriptionService);
    }
    #endregion

    private IEndpoint SubscriptionManagerEndpoint
    {
      get
      {
        return _endpointResolver.Resolve(_uriFactory.CreateUri(_configuration.SubscriptionManagerEndpointUri));
      }
    }
  }
}