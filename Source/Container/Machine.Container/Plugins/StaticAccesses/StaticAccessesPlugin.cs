using System;
using System.Collections.Generic;
using System.Reflection;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Plugins.StaticAccesses
{
  public class StaticAccessesPlugin : IServiceContainerPlugin, IServiceContainerListener
  {
    #region IServiceContainerPlugin Members
    public void Initialize(PluginServices services)
    {
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

    #region IServiceContainerListener Members
    public void InitializeListener(IMachineContainer container)
    {
    }

    public void PreparedForServices()
    {
    }

    public void OnRegistration(ServiceEntry entry)
    {
      Type concreteType = entry.ConcreteType;
      if (concreteType.IsClass)
      {
      }
    }

    public void Started()
    {
    }

    public void OnActivation(ResolvedServiceEntry entry, Activation activation)
    {
    }

    public void OnDeactivation(ResolvedServiceEntry entry, Deactivation deactivation)
    {
    }
    #endregion
  }
}
