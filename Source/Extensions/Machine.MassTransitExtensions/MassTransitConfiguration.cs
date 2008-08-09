using System;

namespace Machine.MassTransitExtensions
{
  public class MassTransitConfiguration
  {
    private Type _transportType;
    private Uri _subscriptionManagerEndpointUri;

    public Type TransportType
    {
      get { return _transportType; }
      set { _transportType = value; }
    }

    public Uri SubscriptionManagerEndpointUri
    {
      get { return _subscriptionManagerEndpointUri; }
      set { _subscriptionManagerEndpointUri = value; }
    }

    public MassTransitConfiguration(string subscriptionEndpointUri)
     : this(new Uri(subscriptionEndpointUri))
    {
    }

    public MassTransitConfiguration(Uri subscriptionEndpointUri)
    {
      _subscriptionManagerEndpointUri = subscriptionEndpointUri;
      _transportType = typeof(MassTransit.ServiceBus.MSMQ.MsmqEndpoint);
    }
  }
}