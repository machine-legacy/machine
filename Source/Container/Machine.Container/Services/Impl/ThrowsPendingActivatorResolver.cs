using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services.Impl
{
  public class ThrowsPendingActivatorResolver : IActivatorResolver
  {
    #region IActivatorResolver Members
    public IActivator ResolveActivator(IResolutionServices services, ServiceEntry entry)
    {
      if ((services.Flags & LookupFlags.ThrowIfUnable) == LookupFlags.ThrowIfUnable)
      {
        throw new PendingDependencyException(services.DependencyGraphTracker.BuildProgressMessage(entry));
      }
      return null;
    }
    #endregion
  }
}