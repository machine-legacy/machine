using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Plugins.ParentContainers
{
  public class ParentContainerActivatorResolver : IActivatorResolver
  {
    private readonly IMachineContainer _container;

    public ParentContainerActivatorResolver(IMachineContainer container)
    {
      _container = container;
    }

    #region IActivatorResolver Members
    public IActivator ResolveActivator(IResolutionServices services, ServiceEntry entry)
    {
      if (!_container.CanResolve(entry.ServiceType))
      {
        return null;
      }
      return new ParentContainerActivator(_container, entry);
    }
    #endregion
  }
}