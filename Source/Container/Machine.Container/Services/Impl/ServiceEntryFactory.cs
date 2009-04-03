using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services.Impl
{
  public class ServiceEntryFactory : IServiceEntryFactory
  {
    public ServiceEntry CreateServiceEntry(Type implementationType, LifestyleType lifestyleType)
    {
      return new ServiceEntry(implementationType, lifestyleType);
    }

    public ServiceEntry CreateServiceEntry(ServiceDependency dependency)
    {
      return new ServiceEntry(dependency.DependencyType, LifestyleType.Override, dependency.Key);
    }
  }
}