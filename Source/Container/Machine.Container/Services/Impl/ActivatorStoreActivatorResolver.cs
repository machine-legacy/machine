using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services.Impl
{
  public class ActivatorStoreActivatorResolver : IActivatorResolver
  {
    public virtual IActivator ResolveActivator(IResolutionServices services, ServiceEntry entry)
    {
      if (!services.ActivatorStore.HasActivator(entry))
      {
        return null;
      }
      IActivator activator = services.ActivatorStore.ResolveActivator(entry);
      if (activator.CanActivate(services))
      {
        return activator;
      }
      if ((services.Flags & LookupFlags.ThrowIfUnable) == LookupFlags.ThrowIfUnable)
      {
        throw new ServiceResolutionException("Unable to activate: " + entry);
      }
      return null;
    }
  }
}