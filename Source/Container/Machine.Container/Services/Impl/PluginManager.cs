using System;
using System.Collections.Generic;
using Machine.Container.Plugins;

namespace Machine.Container.Services.Impl
{
  public class PluginManager : IPluginManager
  {
    private readonly IHighLevelContainer _container;
    private readonly List<IServiceContainerPlugin> _plugins = new List<IServiceContainerPlugin>();
    private bool _initialized;

    public PluginManager(IHighLevelContainer container)
    {
      _container = container;
    }

    #region IPluginManager Members
    public void AddPlugin(IServiceContainerPlugin plugin)
    {
      if (_initialized)
      {
        throw new InvalidOperationException("Plugins may only be added before the container is initialized!");
      }
      _plugins.Add(plugin);
    }

    public void Initialize()
    {
      foreach (IServiceContainerPlugin plugin in _plugins)
      {
        plugin.Initialize(_container);
      }
      _initialized = true;
    }

    public void Dispose()
    {
      if (!_initialized)
      {
        return;
      }
      foreach (IServiceContainerPlugin plugin in _plugins)
      {
        plugin.Dispose();
      }
    }
    #endregion
  }
}
