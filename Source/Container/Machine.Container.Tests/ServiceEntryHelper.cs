using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container
{
  public static class ServiceEntryHelper
  {
    public static ServiceEntry NewEntry()
    {
      return NewEntry(typeof(SimpleService1));
    }

    public static ServiceEntry NewEntry(Type implementationTyoe)
    {
      return new ServiceEntry(implementationTyoe, LifestyleType.Singleton);
    }

    public static ServiceEntry NewEntry(LifestyleType lifestyleType)
    {
      return new ServiceEntry(typeof(Service1DependsOn2), lifestyleType);
    }
  }
}
