using System;

using Machine.Container.Plugins;

namespace Machine.Container.Services
{
  public interface IInternalServices
  {
    IActivatorStore ActivatorStore
    {
      get;
    }

    IActivatorStrategy ActivatorStrategy
    {
      get;
    }

    ILifestyleFactory LifestyleFactory
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
  }
  public interface IContainerServices : IInternalServices
  {
    IResolutionServices CreateResolutionServices(object[] serviceOverrides);
  }
  public interface IResolutionServices : IInternalServices
  {
    IOverrideLookup Overrides
    {
      get;
    }

    DependencyGraphTracker DependencyGraphTracker
    {
      get;
    }
  }
}
