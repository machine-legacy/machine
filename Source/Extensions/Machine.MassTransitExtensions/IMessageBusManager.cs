using System;
using System.Collections.Generic;

namespace Machine.MassTransitExtensions
{
  public interface IMessageBusManager
  {
    void UseSingleBus(EndpointName local);
  }
}
