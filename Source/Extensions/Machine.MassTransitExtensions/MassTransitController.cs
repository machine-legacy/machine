using Machine.Container.Services;

using MassTransit.ServiceBus;
using MassTransit.ServiceBus.Internal;
using MassTransit.ServiceBus.Subscriptions;

namespace Machine.MassTransitExtensions
{
  public class MassTransitController : IMassTransit
  {
    private readonly IMachineContainer _container;
    private readonly MassTransitConfiguration _configuration;
    private readonly HostedServicesController _hostedServicesController;
    private readonly ISubscriptionCache _subscriptionCache;
    private readonly IEndpointResolver _endpointResolver;
    private readonly IObjectBuilder _objectBuilder;
    private ISubscriptionRepository _subscriptionRepository;
    private IServiceBus _bus;

    public MassTransitController(IMachineContainer container, HostedServicesController hostedServicesController, ISubscriptionCache subscriptionCache, IEndpointResolver endpointResolver, IObjectBuilder objectBuilder, MassTransitConfiguration configuration)
    {
      _container = container;
      _objectBuilder = objectBuilder;
      _endpointResolver = endpointResolver;
      _subscriptionCache = subscriptionCache;
      _hostedServicesController = hostedServicesController;
      _configuration = configuration;
    }

    #region IMassTransit Members
    public virtual void Start()
    {
      EndpointResolver.AddTransport(_configuration.TransportType);
      if (_configuration.IsSubscriptionManager)
      {
        _subscriptionRepository = _container.Resolve.Object<ISubscriptionRepository>();
      }
      _bus = CreateServiceBus();
      _container.Register.Type<IServiceBus>().Is(_bus);
      _container.Resolve.Object<ISubscriptionService>().Start();
      _hostedServicesController.Start();
    }

    public virtual void Stop()
    {
      _hostedServicesController.Stop();
      _container.Resolve.Object<ISubscriptionService>().Stop();
    }
    #endregion

    #region IDisposable Members
    public virtual void Dispose()
    {
      _hostedServicesController.Dispose();
      _bus.Dispose();
      _subscriptionCache.Dispose();
      _container.Resolve.Object<ISubscriptionService>().Dispose();
      if (_subscriptionRepository != null)
      {
        _subscriptionRepository.Dispose();
      }
    }
    #endregion

    public virtual IServiceBus CreateServiceBus()
    {
      IEndpoint endpoint = _endpointResolver.Resolve(_configuration.LocalEndpointUri);
      return new ServiceBus(endpoint, _objectBuilder, _subscriptionCache, _endpointResolver);
    }

    public virtual IHostedService CreateSubscriptionService()
    {
      if (_configuration.IsSubscriptionManager)
      {
        return new SubscriptionService(_bus, _subscriptionCache, _subscriptionRepository);
      }
      IEndpoint endpoint = _endpointResolver.Resolve(_configuration.SubscriptionEndpointUri);
      return new SubscriptionClient(_bus, _subscriptionCache, endpoint);
    }
  }
}