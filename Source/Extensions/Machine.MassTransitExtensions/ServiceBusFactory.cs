using System;
using System.Collections.Generic;

using MassTransit.ServiceBus;
using MassTransit.ServiceBus.Internal;
using MassTransit.ServiceBus.Subscriptions;

namespace Machine.MassTransitExtensions
{
  public class ServiceBusFactory : IServiceBusFactory
  {
    private readonly IMassTransitUriFactory _uriFactory;
    private readonly IObjectBuilder _objectBuilder;
    private readonly ISubscriptionCache _subscriptionCache;
    private readonly IEndpointResolver _endpointResolver;

    public ServiceBusFactory(IEndpointResolver endpointResolver, IObjectBuilder objectBuilder, ISubscriptionCache subscriptionCache, IMassTransitUriFactory uriFactory)
    {
      _endpointResolver = endpointResolver;
      _objectBuilder = objectBuilder;
      _subscriptionCache = subscriptionCache;
      _uriFactory = uriFactory;
    }

    #region IServiceBusFactory Members
    public IServiceBus CreateServiceBus(EndpointName endpointName)
    {
      Uri transformedUri = _uriFactory.CreateUri(endpointName);
      return new ServiceBus(_endpointResolver.Resolve(transformedUri), _objectBuilder, _subscriptionCache, _endpointResolver);
    }
    #endregion
  }
}
