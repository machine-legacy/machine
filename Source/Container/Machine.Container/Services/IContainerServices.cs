using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Plugins;

namespace Machine.Container.Services
{
  public interface IInternalServices
  {
    ContainerStatePolicy StatePolicy
    {
      get;
    }

    IActivatorStore ActivatorStore
    {
      get;
    }

    IActivatorFactory ActivatorFactory
    {
      get;
    }

    ILifestyleFactory LifestyleFactory
    {
      get;
    }

    IServiceEntryFactory ServiceEntryFactory
    {
      get;
    }

    IServiceEntryResolver ServiceEntryResolver
    {
      get;
    }

    IListenerInvoker ListenerInvoker
    {
      get;
    }

    IObjectInstances ObjectInstances
    {
      get;
    }

    IServiceGraph ServiceGraph
    {
      get;
    }

    IResolvableTypeMap ResolvableTypeMap
    {
      get;
    }
  }

  public interface IResolvableTypeMap
  {
    IResolvableType FindResolvableType(string name);
    IResolvableType FindResolvableType(Type type);
    IResolvableType FindResolvableType(ServiceDependency dependency);
  }
  
  public interface IContainerServices : IInternalServices
  {
    IResolutionServices CreateResolutionServices(IOverrideLookup overrides, LookupFlags flags);
  }
}