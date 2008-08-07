using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.ObjectFactories
{
  public class FactoriesActivatorResolver : IActivatorResolver
  {
    private readonly IMachineContainer _container;

    public FactoriesActivatorResolver(IMachineContainer container)
    {
      _container = container;
    }

    #region IActivatorResolver Members
    public IActivator ResolveActivator(IResolutionServices services, ServiceEntry entry)
    {
      Type factoryType = FactoryHelper.CreateFactoryType(entry.ServiceType);
      if (!_container.CanResolve(factoryType))
      {
        return null;
      }
      return new FactoriesActivator(_container, entry);
    }
    #endregion
  }
}