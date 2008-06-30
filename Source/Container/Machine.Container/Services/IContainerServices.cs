using System;

using Machine.Container.Activators;
using Machine.Container.Plugins;

namespace Machine.Container.Services
{
  public interface IContainerServices
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

    IOverrideLookup Overrides
    {
      get;
    }

    IServiceEntryResolver ServiceEntryResolver
    {
      get;
    }

    DependencyGraphTracker DependencyGraphTracker
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
  }
}
