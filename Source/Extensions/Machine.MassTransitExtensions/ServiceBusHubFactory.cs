using System;

using MassTransit.ServiceBus;
using MassTransit.ServiceBus.Internal;
using MassTransit.ServiceBus.Subscriptions;

namespace Machine.MassTransitExtensions
{
  public class ServiceBusHubFactory : IServiceBusHubFactory
  {
    private readonly IMassTransitConfigurationProvider _configurationProvider;
    private readonly IMassTransitUriFactory _uriFactory;
    private readonly IObjectBuilder _objectBuilder;
    private readonly IEndpointResolver _endpointResolver;
    private readonly ISubscriptionRepository _subscriptionRepository;

    public ServiceBusHubFactory(IMassTransitConfigurationProvider configurationProvider, IEndpointResolver endpointResolver, IObjectBuilder objectBuilder, IMassTransitUriFactory uriFactory, ISubscriptionRepository subscriptionRepository)
    {
      _configurationProvider = configurationProvider;
      _endpointResolver = endpointResolver;
      _subscriptionRepository = subscriptionRepository;
      _objectBuilder = objectBuilder;
      _uriFactory = uriFactory;
    }

    #region IServiceBusHubFactory Members
    public IServiceBusHub CreateServerHub(EndpointName endpointName)
    {
      ISubscriptionCache subscriptionCache = new LocalSubscriptionCache();
      IEndpoint endpoint = _endpointResolver.Resolve(_uriFactory.CreateUri(endpointName));
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
      get { return _endpointResolver.Resolve(_uriFactory.CreateUri(_configurationProvider.Configuration.SubscriptionManagerEndpointUri)); }
    }
  }
}