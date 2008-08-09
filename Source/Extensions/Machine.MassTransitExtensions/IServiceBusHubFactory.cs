using System;

namespace Machine.MassTransitExtensions
{
  public interface IServiceBusHubFactory
  {
    IServiceBusHub CreateServerHub(Uri uri);
    IServiceBusHub CreateSubscriptionManagerHub();
  }
}