using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services
{
  public interface IServiceEntryFactory
  {
    ServiceEntry CreateServiceEntry(Type implementationType, LifestyleType lifestyleType);
    ServiceEntry CreateServiceEntry(ServiceDependency dependency);
  }
}