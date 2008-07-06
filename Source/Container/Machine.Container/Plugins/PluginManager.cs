using System;
using System.Collections.Generic;

using Machine.Container.Plugins;
using Machine.Container.Services;

namespace Machine.Container.Plugins
{
  public class PluginManager : IPluginManager
  {
    private readonly List<IServiceContainerPlugin> _plugins = new List<IServiceContainerPlugin>();
    private readonly List<IServiceContainerListener> _listeners = new List<IServiceContainerListener>();
    private bool _initialized;

    public PluginManager()
    {
    }

    #region IPluginManager Members
    public void AddListener(IServiceContainerListener listener)
    {
      AssertNotInitialized();
      _listeners.Add(listener);
    }

    public void AddPlugin(IServiceContainerPlugin plugin)
    {
      AssertNotInitialized();
      _plugins.Add(plugin);
    }

    public void Initialize(PluginServices services)
    {
      AssertNotInitialized();
      foreach (IServiceContainerPlugin plugin in _plugins)
      {
        plugin.Initialize(services);
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

    public IEnumerable<IServiceContainerListener> AllListeners
    {
      get { return _listeners; }
    }
    #endregion

    private void AssertNotInitialized()
    {
      if (_initialized)
      {
        throw new InvalidOperationException("Plugins may only be added before the container is initialized!");
      }
    }
  }
}