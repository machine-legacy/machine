using System;
using System.Collections.Generic;

using MassTransit.ServiceBus;

namespace Machine.MassTransitExtensions
{
  public class StandardServiceBuses : IStandardServiceBuses, IDisposable
  {
    private readonly MassTransitConfiguration _configuration;
    private readonly IMassTransitUriFactory _uriFactory;
    private readonly IServiceBusFactory _serviceBusFactory;
    private IServiceBus _defaultServiceBus;

    public StandardServiceBuses(MassTransitConfiguration configuration, IMassTransitUriFactory uriFactory, IServiceBusFactory serviceBusFactory)
    {
      _configuration = configuration;
      _serviceBusFactory = serviceBusFactory;
      _uriFactory = uriFactory;
    }

    #region IStandardServiceBuses Members
    public IServiceBus DefaultServiceBus
    {
      get
      {
        if (_defaultServiceBus == null)
        {
          Uri uri = _uriFactory.CreateUri(_configuration.DefaultLocalEndpointUri);
          _defaultServiceBus = _serviceBusFactory.CreateServiceBus(uri);
        }
        return _defaultServiceBus;
      }
    }
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
      if (_defaultServiceBus != null)
      {
        _defaultServiceBus.Dispose();
        _defaultServiceBus = null;
      }
    }
    #endregion
  }
}
