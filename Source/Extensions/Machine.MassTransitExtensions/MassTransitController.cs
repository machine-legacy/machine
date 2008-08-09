using Machine.Container.Services;

using MassTransit.ServiceBus;
using MassTransit.ServiceBus.Internal;
using MassTransit.ServiceBus.Subscriptions;

namespace Machine.MassTransitExtensions
{
  public class MassTransitController : IMassTransit
  {
    private readonly MassTransitConfiguration _configuration;
    private readonly HostedServicesController _hostedServicesController;
    private readonly IMachineContainer _container;
    private readonly ISubscriptionCache _subscriptionCache;
    private readonly IServiceBusFactory _serviceBusFactory;
    private IServiceBus _bus;

    public MassTransitController(IMachineContainer container, HostedServicesController hostedServicesController, ISubscriptionCache subscriptionCache, MassTransitConfiguration configuration, IServiceBusFactory serviceBusFactory)
    {
      _container = container;
      _serviceBusFactory = serviceBusFactory;
      _subscriptionCache = subscriptionCache;
      _hostedServicesController = hostedServicesController;
      _configuration = configuration;
    }

    #region IMassTransit Members
    public virtual void Start()
    {
      EndpointResolver.AddTransport(_configuration.TransportType);
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
    }
    #endregion

    public virtual IServiceBus CreateServiceBus()
    {
      return _serviceBusFactory.CreateServiceBus(_configuration.DefaultLocalEndpointUri);
    }
  }
}