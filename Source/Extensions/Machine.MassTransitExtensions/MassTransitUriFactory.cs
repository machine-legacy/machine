using System;
using System.Collections.Generic;

namespace Machine.MassTransitExtensions
{
  public class MassTransitUriFactory : IMassTransitUriFactory
  {
    private readonly IMassTransitConfigurationProvider _configurationProvider;
    private static readonly Dictionary<Type, IMassTransitUriFactory> _factories = new Dictionary<Type, IMassTransitUriFactory>();

    static MassTransitUriFactory()
    {
      _factories[typeof(MassTransit.ServiceBus.MSMQ.MsmqEndpoint)] = new MsMqFactory();
      _factories[typeof(MassTransit.ServiceBus.NMS.NmsEndpoint)] = new NmsFactory();
    }

    public MassTransitUriFactory(IMassTransitConfigurationProvider configurationProvider)
    {
      _configurationProvider = configurationProvider;
    }

    public Uri CreateUri(EndpointName name)
    {
      return _factories[_configurationProvider.Configuration.TransportType].CreateUri(name);
    }

    public Uri CreateUri(string name)
    {
      return _factories[_configurationProvider.Configuration.TransportType].CreateUri(name);
    }

    public Uri CreateUri(string address, string name)
    {
      return _factories[_configurationProvider.Configuration.TransportType].CreateUri(address, name);
    }

    public Uri CreateUri(Uri uri)
    {
      return _factories[_configurationProvider.Configuration.TransportType].CreateUri(uri);
    }
  }
  public class NmsFactory : IMassTransitUriFactory
  {
    public Uri CreateUri(EndpointName name)
    {
      return CreateUri(name.Address, name.Name);
    }

    public Uri CreateUri(string name)
    {
      return CreateUri("127.0.0.1", name);
    }

    public Uri CreateUri(Uri uri)
    {
      return new Uri("activemq://" + uri.Host + ":61616" + uri.AbsolutePath);
    }

    public Uri CreateUri(string address, string name)
    {
      return new Uri("activemq://" + address + ":61616/" + name);
    }
  }
  public class MsMqFactory : IMassTransitUriFactory
  {
    public Uri CreateUri(EndpointName name)
    {
      return CreateUri(name.Address, name.Name);
    }

    public Uri CreateUri(string name)
    {
      return CreateUri("localhost", name);
    }

    public Uri CreateUri(Uri uri)
    {
      return new Uri("msmq://" + uri.Host + uri.AbsolutePath);
    }

    public Uri CreateUri(string address, string name)
    {
      return new Uri("msmq://" + address + "/" + name);
    }
  }
}