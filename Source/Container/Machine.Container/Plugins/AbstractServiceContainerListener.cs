using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Plugins
{
  public abstract class AbstractServiceContainerListener : IServiceContainerListener
  {
    private IMachineContainer _container;

    public IMachineContainer Container
    {
      get { return _container; }
    }

    #region IServiceContainerListener Members
    public virtual void Initialize(IMachineContainer container)
    {
      _container = container;
    }

    public virtual void PreparedForServices()
    {
    }

    public virtual void OnRegistration(ServiceEntry entry)
    {
    }

    public virtual void Started()
    {
    }

    public virtual void OnActivation(ResolvedServiceEntry entry, Activation activation)
    {
    }

    public virtual void OnDeactivation(ResolvedServiceEntry entry, Deactivation deactivation)
    {
    }
    #endregion

    #region IDisposable Members
    public virtual void Dispose()
    {
    }
    #endregion
  }
}
