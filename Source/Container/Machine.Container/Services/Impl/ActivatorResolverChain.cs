using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services.Impl
{
  public class RootActivatorResolverChain : Chain<IActivatorResolver>, IRootActivatorResolver
  {
    #region IActivatorResolver Members
    public IActivator ResolveActivator(IResolutionServices services, ServiceEntry entry)
    {
      foreach (IActivatorResolver resolver in this.ChainItems)
      {
        IActivator activator = resolver.ResolveActivator(services, entry);
        if (activator != null)
        {
          return activator;
        }
      }
      if ((services.Flags & LookupFlags.ThrowIfUnable) == LookupFlags.ThrowIfUnable)
      {
        throw new ServiceContainerException("Unable to resolve Activator for: " + entry);
      }
      return null;
    }
    #endregion
  }
}