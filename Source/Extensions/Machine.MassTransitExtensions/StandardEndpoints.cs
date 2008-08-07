using System;

using MassTransit.ServiceBus;
using MassTransit.ServiceBus.Internal;

namespace Machine.MassTransitExtensions
{
  public class StandardEndpoints : IStandardEndpoints
  {
    private readonly IEndpointResolver _endpointResolver;
    private readonly IMassTransitUriFactory _uriFactory;
    private readonly MassTransitConfiguration _configuration;

    public StandardEndpoints(IEndpointResolver endpointResolver, IMassTransitUriFactory uriFactory, MassTransitConfiguration configuration)
    {
      _endpointResolver = endpointResolver;
      _uriFactory = uriFactory;
      _configuration = configuration;
    }

    #region IStandardEndpoints Members
    public IEndpoint DefaultLocalEndpoint
    {
      get { return _endpointResolver.Resolve(_uriFactory.CreateUri(_configuration.DefaultLocalEndpointUri)); }
    }

    public IEndpoint SubscriptionManagerEndpoint
    {
      get { return _endpointResolver.Resolve(_uriFactory.CreateUri(_configuration.SubscriptionManagerEndpointUri)); }
    }
    #endregion
  }
}