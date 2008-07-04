using System;
using System.Collections.Generic;

using Machine.Container.Plugins;

namespace Machine.Container.Services
{
  public interface IInternalServices
  {
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
    IResolutionServices CreateResolutionServices(object[] overrides);
  }
}