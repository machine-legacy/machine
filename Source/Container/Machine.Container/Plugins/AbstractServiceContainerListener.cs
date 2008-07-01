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

    public virtual void ServiceRegistered(ServiceEntry entry)
    {
    }

    public virtual void Started()
    {
    }

    public virtual void InstanceCreated(ResolvedServiceEntry entry, object instance)
    {
    }

    public virtual void InstanceReleased(ResolvedServiceEntry entry, object instance)
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
