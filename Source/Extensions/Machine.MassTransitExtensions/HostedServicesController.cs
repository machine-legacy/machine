using System;

using MassTransit.ServiceBus;

using Machine.Container.Services;

namespace Machine.MassTransitExtensions
{
  public class HostedServicesController : IDisposable
  {
    private readonly IMachineContainer _container;

    public HostedServicesController(IMachineContainer container)
    {
      _container = container;
    }

    public void Start()
    {
      foreach (IHostedService service in _container.Resolve.All<IHostedService>())
      {
        service.Start();
      }
    }

    public void Stop()
    {
      foreach (IHostedService service in _container.Resolve.All<IHostedService>())
      {
        service.Stop();
      }
    }

    public void Dispose()
    {
      foreach (IHostedService service in _container.Resolve.All<IHostedService>())
      {
        service.Dispose();
      }
    }
  }
}