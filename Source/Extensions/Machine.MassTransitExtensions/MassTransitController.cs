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

    public MassTransitController(HostedServicesController hostedServicesController, MassTransitConfiguration configuration)
    {
      _hostedServicesController = hostedServicesController;
      _configuration = configuration;
    }

    #region IMassTransit Members
    public virtual void Start()
    {
      EndpointResolver.AddTransport(_configuration.TransportType);
      _hostedServicesController.Start();
    }

    public virtual void Stop()
    {
      _hostedServicesController.Stop();
    }
    #endregion

    #region IDisposable Members
    public virtual void Dispose()
    {
    }
    #endregion
  }
}