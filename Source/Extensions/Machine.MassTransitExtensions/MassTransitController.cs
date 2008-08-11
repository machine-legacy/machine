using MassTransit.ServiceBus.Internal;

namespace Machine.MassTransitExtensions
{
  public class MassTransitController : IMassTransit
  {
    private readonly IMassTransitConfigurationProvider _configurationProvider;
    private readonly HostedServicesController _hostedServicesController;

    public MassTransitController(HostedServicesController hostedServicesController, IMassTransitConfigurationProvider configurationProvider)
    {
      _hostedServicesController = hostedServicesController;
      _configurationProvider = configurationProvider;
    }

    #region IMassTransit Members
    public virtual void Start()
    {
      EndpointResolver.AddTransport(_configurationProvider.Configuration.TransportType);
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