using System;
using System.Collections.Generic;

using Machine.Container.Plugins;
using Machine.Container.Services.Impl;

namespace Machine.Container.ObjectFactories
{
  public class FactoriesPlugin : IServiceContainerPlugin
  {
    #region IServiceContainerPlugin Members
    public void Initialize(PluginServices services)
    {
      services.Resolver.AddAfter(typeof(ActivatorStoreActivatorResolver), new FactoriesActivatorResolver(services.Container));
    }

    public void ReadyForServices(PluginServices services)
    {
    }
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
    }
    #endregion
  }
}
