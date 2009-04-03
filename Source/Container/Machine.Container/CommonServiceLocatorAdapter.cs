using System;
using System.Collections.Generic;

using Machine.Container.Services;
using Microsoft.Practices.ServiceLocation;

namespace Machine.Container
{
  public class CommonServiceLocatorAdapter : ServiceLocatorImplBase
  {
    readonly IHighLevelContainer _container;

    public CommonServiceLocatorAdapter(IHighLevelContainer container)
    {
      _container = container;
    }

    protected override object DoGetInstance(Type serviceType, string key)
    {
      if (!string.IsNullOrEmpty(key))
      {
        throw new NotImplementedException();
      }

      return _container.Resolve.Object(serviceType);
    }

    protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
    {
      return _container.Resolve.All(serviceType);
    }
  }
}
