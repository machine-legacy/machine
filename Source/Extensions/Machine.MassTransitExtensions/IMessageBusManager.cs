using System;
using System.Collections.Generic;

namespace Machine.MassTransitExtensions
{
  public interface IMessageBusManager
  {
    IMessageBus UseSingleBus(EndpointName local);
  }
}
