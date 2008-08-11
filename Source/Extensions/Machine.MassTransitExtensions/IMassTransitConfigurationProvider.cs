using System;
using System.Collections.Generic;

namespace Machine.MassTransitExtensions
{
  public interface IMassTransitConfigurationProvider
  {
    MassTransitConfiguration Configuration
    {
      get;
    }
  }
}
