using System;

namespace Machine.MassTransitExtensions
{
  public interface IServiceBusHubFactory
  {
    IServiceBusHub CreateServerHub(EndpointName endpointName);
    IServiceBusHub CreateSubscriptionManagerHub();
  }
}