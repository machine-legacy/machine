using System;

namespace Machine.MassTransitExtensions
{
  public class MassTransitConfiguration
  {
    private bool _isSubscriptionManager;
    private Type _transportType;
    private Uri _defaultLocalEndpointUri;
    private Uri _subscriptionManagerEndpointUri;

    public bool IsSubscriptionManager
    {
      get { return _isSubscriptionManager; }
      set { _isSubscriptionManager = value; }
    }

    public Type TransportType
    {
      get { return _transportType; }
      set { _transportType = value; }
    }

    public Uri DefaultLocalEndpointUri
    {
      get { return _defaultLocalEndpointUri; }
      set { _defaultLocalEndpointUri = value; }
    }

    public Uri SubscriptionManagerEndpointUri
    {
      get { return _subscriptionManagerEndpointUri; }
      set { _subscriptionManagerEndpointUri = value; }
    }

    public MassTransitConfiguration(string subscriptionEndpointUri, string localEndpointUri)
     : this(new Uri(subscriptionEndpointUri), new Uri(localEndpointUri), false)
    {
    }

    public MassTransitConfiguration(string subscriptionEndpointUri, string localEndpointUri, bool isSubscriptionManager)
     : this(new Uri(subscriptionEndpointUri), new Uri(localEndpointUri), isSubscriptionManager)
    {
    }

    public MassTransitConfiguration(Uri subscriptionEndpointUri, Uri localEndpointUri, bool isSubscriptionManager)
    {
      _isSubscriptionManager = isSubscriptionManager;
      _defaultLocalEndpointUri = localEndpointUri;
      _subscriptionManagerEndpointUri = subscriptionEndpointUri;
      _transportType = typeof(MassTransit.ServiceBus.MSMQ.MsmqEndpoint);
    }
  }
}