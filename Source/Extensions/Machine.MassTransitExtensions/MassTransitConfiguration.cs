using System;

namespace Machine.MassTransitExtensions
{
  public class MassTransitConfiguration
  {
    private bool _isSubscriptionManager;
    private Type _transportType;
    private Uri _localEndpointUri;
    private Uri _subscriptionEndpointUri;

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

    public Uri LocalEndpointUri
    {
      get { return _localEndpointUri; }
      set { _localEndpointUri = value; }
    }

    public Uri SubscriptionEndpointUri
    {
      get { return _subscriptionEndpointUri; }
      set { _subscriptionEndpointUri = value; }
    }

    public MassTransitConfiguration(bool isSubscriptionManager, Uri subscriptionEndpointUri, Uri localEndpointUri)
    {
      _isSubscriptionManager = isSubscriptionManager;
      _localEndpointUri = localEndpointUri;
      _subscriptionEndpointUri = subscriptionEndpointUri;
      _transportType = typeof(MassTransit.ServiceBus.MSMQ.MsmqEndpoint);
    }
  }
}