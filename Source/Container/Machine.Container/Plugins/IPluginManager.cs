using System;
using System.Collections.Generic;

using Machine.Container.Plugins;

namespace Machine.Container.Plugins
{
  public interface IPluginManager
  {
    void Initialize(PluginServices services);
    void ReadyForServices(PluginServices services);
    void AddListener(IServiceContainerListener listener);
    void AddPlugin(IServiceContainerPlugin plugin);
    void Dispose();
    IEnumerable<IServiceContainerListener> AllListeners
    {
      get;
    }
  }
}