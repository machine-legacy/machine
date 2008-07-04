using System;
using System.Collections.Generic;

using Machine.Container.Activators;
using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Services.Impl
{
  public class DefaultActivatorFactory : IActivatorFactory
  {
    #region Member Data
    private readonly IObjectFactory _objectFactory;
    private readonly IServiceEntryResolver _entryResolver;
    private readonly IServiceDependencyInspector _serviceDependencyInspector;
    #endregion

    #region DefaultActivatorStrategy()
    public DefaultActivatorFactory(IObjectFactory objectFactory, IServiceDependencyInspector serviceDependencyInspector, IServiceEntryResolver serviceEntryResolver)
    {
      _objectFactory = objectFactory;
      _serviceDependencyInspector = serviceDependencyInspector;
      _entryResolver = serviceEntryResolver;
    }
    #endregion

    #region IActivatorStrategy Members
    public IActivator CreateStaticActivator(ServiceEntry entry, object instance)
    {
      return new StaticActivator(entry, instance);
    }

    public IActivator CreateDefaultActivator(ServiceEntry entry)
    {
      return new DefaultActivator(_objectFactory, _serviceDependencyInspector, _entryResolver, entry);
    }
    #endregion
  }
}