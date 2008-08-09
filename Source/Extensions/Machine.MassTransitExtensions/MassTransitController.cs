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

    public MassTransitController(IMachineContainer container, HostedServicesController hostedServicesController, ISubscriptionCache subscriptionCache, MassTransitConfiguration configuration)
    {
      _container = container;
      _subscriptionCache = subscriptionCache;
      _hostedServicesController = hostedServicesController;
      _configuration = configuration;
    }

    #region IMassTransit Members
    public virtual void Start()
    {
      EndpointResolver.AddTransport(_configuration.TransportType);
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
      _subscriptionCache.Dispose();
      _container.Resolve.Object<ISubscriptionService>().Dispose();
    }
    #endregion
  }
}