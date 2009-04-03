using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services.Impl
{
  public class ServiceEntryFactory : IServiceEntryFactory
  {
    static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ServiceEntryFactory));

    public ServiceEntry CreateServiceEntry(Type implementationType, LifestyleType lifestyleType)
    {
      ServiceEntry entry = new ServiceEntry(implementationType, lifestyleType);
      _log.Info("Creating: " + entry);
      return entry;
    }

    public ServiceEntry CreateServiceEntry(ServiceDependency dependency)
    {
      ServiceEntry entry = new ServiceEntry(dependency.DependencyType, LifestyleType.Override, dependency.Key);
      _log.Info("Creating: " + entry);
      return entry;
    }
  }
}