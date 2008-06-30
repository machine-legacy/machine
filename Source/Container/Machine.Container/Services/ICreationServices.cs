using System;

using Machine.Container.Activators;
using Machine.Container.Plugins;
using Machine.Container.Services.Impl;

namespace Machine.Container.Services
{
  public interface ICreationServices
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
