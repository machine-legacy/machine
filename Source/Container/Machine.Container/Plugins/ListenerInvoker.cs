using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Plugins
{
  public class ListenerInvoker : IListenerInvoker
  {
    private readonly IPluginManager _pluginManager;

    public ListenerInvoker(IPluginManager pluginManager)
    {
      _pluginManager = pluginManager;
    }

    #region IServiceContainerListener Members
    public void Initialize(IMachineContainer container)
    {
      foreach (IServiceContainerListener listener in _pluginManager.AllListeners)
      {
        listener.Initialize(container);
      }
    }

    public void PreparedForServices()
    {
      foreach (IServiceContainerListener listener in _pluginManager.AllListeners)
      {
        listener.PreparedForServices();
      }
    }

    public void OnRegistration(ServiceEntry entry)
    {
      foreach (IServiceContainerListener listener in _pluginManager.AllListeners)
      {
        listener.OnRegistration(entry);
      }
    }

    public void Started()
    {
      foreach (IServiceContainerListener listener in _pluginManager.AllListeners)
      {
        listener.Started();
      }
    }

    public void OnActivation(ResolvedServiceEntry entry, Activation activation)
    {
      foreach (IServiceContainerListener listener in _pluginManager.AllListeners)
      {
        listener.OnActivation(entry, activation);
      }
    }

    public void OnDeactivation(ResolvedServiceEntry entry, Deactivation deactivation)
    {
      foreach (IServiceContainerListener listener in _pluginManager.AllListeners)
      {
        listener.OnDeactivation(entry, deactivation);
      }
    }
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
      foreach (IServiceContainerListener listener in _pluginManager.AllListeners)
      {
        listener.Dispose();
      }
    }
    #endregion
  }
}
