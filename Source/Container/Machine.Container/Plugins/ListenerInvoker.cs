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

    public void ServiceRegistered(ServiceEntry entry)
    {
      foreach (IServiceContainerListener listener in _pluginManager.AllListeners)
      {
        listener.ServiceRegistered(entry);
      }
    }

    public void Started()
    {
      foreach (IServiceContainerListener listener in _pluginManager.AllListeners)
      {
        listener.Started();
      }
    }

    public void InstanceCreated(ResolvedServiceEntry entry, object instance)
    {
      foreach (IServiceContainerListener listener in _pluginManager.AllListeners)
      {
        listener.InstanceCreated(entry, instance);
      }
    }

    public void InstanceReleased(ResolvedServiceEntry entry, object instance)
    {
      foreach (IServiceContainerListener listener in _pluginManager.AllListeners)
      {
        listener.InstanceReleased(entry, instance);
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
