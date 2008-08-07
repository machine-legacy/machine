using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.ObjectFactories
{
  public class FactoriesActivator : IActivator
  {
    private readonly ServiceEntry _entry;
    private readonly IMachineContainer _container;

    public FactoriesActivator(IMachineContainer container, ServiceEntry entry)
    {
      _container = container;
      _entry = entry;
    }

    #region IActivator Members
    public bool CanActivate(IResolutionServices services)
    {
      Type factoryType = FactoryHelper.CreateFactoryType(_entry.ServiceType);
      return _container.CanResolve(factoryType);
    }

    public Activation Activate(IResolutionServices services)
    {
      FactoryProxy factory = ResolveFactory();
      return new Activation(_entry, factory.Create());
    }

    public void Deactivate(IResolutionServices services, object instance)
    {
      FactoryProxy factory = ResolveFactory();
      factory.Deactivate(instance);
    }
    #endregion

    private FactoryProxy ResolveFactory()
    {
      Type factoryType = FactoryHelper.CreateFactoryType(_entry.ServiceType);
      return new FactoryProxy(factoryType, _container.Resolve.Object(factoryType));
    }
  }
}