using System;
using System.Collections.Generic;

namespace Machine.Container.Plugins
{
  public interface IServiceContainerPlugin : IDisposable
  {
    void Initialize(PluginServices services);
  }
}
