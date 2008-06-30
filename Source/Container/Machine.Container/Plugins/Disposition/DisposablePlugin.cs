using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Plugins.Disposition
{
  public class DisposablePlugin : AbstractServiceContainerListener, IServiceContainerPlugin
  {
    #region IServiceContainerListener Members
    public override void InstanceCreated(ResolvedServiceEntry entry, object instance)
    {
      base.InstanceCreated(entry, instance);
    }

    public override void InstanceReleased(ResolvedServiceEntry entry, object instance)
    {
      base.InstanceReleased(entry, instance);
    }
    #endregion

    #region IDisposable Members
    public override void Dispose()
    {
      base.Dispose();
    }
    #endregion
  }
}
