using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Plugins.Disposition
{
  public class DisposablePlugin : IServiceContainerPlugin, IServiceContainerListener
  {
    #region IServiceContainerPlugin Members
    public void Initialize(IHighLevelContainer container)
    {
    }
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
    }
    #endregion

    #region IServiceContainerListener Members
    public void ServiceRegistered(ServiceEntry entry)
    {
    }

    public void InstanceCreated(ServiceEntry entry, object instance)
    {
    }

    public void InstanceReleased(ServiceEntry entry, object instance)
    {
    }
    #endregion
  }
}
