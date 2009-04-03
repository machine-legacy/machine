using System;
using System.Collections.Generic;

using Microsoft.Practices.ServiceLocation;

using Machine.Container.Services;

namespace Machine.Container
{
  public class CommonServiceLocatorAdapter : ServiceLocatorImplBase
  {
    readonly IMachineContainer _container;

    public CommonServiceLocatorAdapter(IMachineContainer container)
    {
      _container = container;
    }

    protected override object DoGetInstance(Type serviceType, string key)
    {
      return _container.Resolve.Named(key);
    }

    protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
    {
      return _container.Resolve.All(serviceType);
    }
  }
}
