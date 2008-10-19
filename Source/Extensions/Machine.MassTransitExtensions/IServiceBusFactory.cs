using System;
using System.Collections.Generic;

using MassTransit.ServiceBus;

namespace Machine.MassTransitExtensions
{
  public interface IServiceBusFactory
  {
    IServiceBus CreateServiceBus(EndpointName endpointName);
  }
}
