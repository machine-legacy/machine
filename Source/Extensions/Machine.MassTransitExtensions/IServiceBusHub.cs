using MassTransit.ServiceBus;

namespace Machine.MassTransitExtensions
{
  public interface IServiceBusHub : IHostedService
  {
    IServiceBus Bus
    {
      get;
    }
  }
}