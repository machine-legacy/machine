using System;
using System.Collections.Generic;

using MassTransit.ServiceBus;

namespace Machine.MassTransitExtensions
{
  public interface IStandardServiceBuses
  {
    IServiceBus DefaultServiceBus
    {
      get;
    }
  }
}
