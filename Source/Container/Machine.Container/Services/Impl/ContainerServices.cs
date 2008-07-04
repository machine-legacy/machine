using System;
using System.Collections.Generic;

using Machine.Container.Plugins;

namespace Machine.Container.Services.Impl
{
  [CoverageExclude]
  public class ContainerServices : IContainerServices
  {
    private readonly IActivatorFactory _activatorFactory;
    private readonly IActivatorStore _activatorStore;
    private readonly ILifestyleFactory _lifestyleFactory;
    private readonly IServiceEntryResolver _serviceEntryResolver;
    private readonly IListenerInvoker _listenerInvoker;
    private readonly IObjectInstances _objectInstances;
    private readonly IServiceGraph _serviceGraph;

    public ContainerServices(IActivatorStore activatorStore, IActivatorFactory activatorFactory, ILifestyleFactory lifestyleFactory, IListenerInvoker listenerInvoker, IObjectInstances objectInstances, IServiceEntryResolver serviceEntryResolver, IServiceGraph serviceGraph)
    {
      _activatorStore = activatorStore;
      _serviceGraph = serviceGraph;
      _activatorFactory = activatorFactory;
      _lifestyleFactory = lifestyleFactory;
      _listenerInvoker = listenerInvoker;
      _objectInstances = objectInstances;
      _serviceEntryResolver = serviceEntryResolver;
    }

    public IActivatorFactory ActivatorFactory
    {
      get { return _activatorFactory; }
    }

    public IActivatorStore ActivatorStore
    {
      get { return _activatorStore; }
    }

    public ILifestyleFactory LifestyleFactory
    {
      get { return _lifestyleFactory; }
    }

    public IServiceEntryResolver ServiceEntryResolver
    {
      get { return _serviceEntryResolver; }
    }

    public IListenerInvoker ListenerInvoker
    {
      get { return _listenerInvoker; }
    }

    public IObjectInstances ObjectInstances
    {
      get { return _objectInstances; }
    }

    public IServiceGraph ServiceGraph
    {
      get { return _serviceGraph; }
    }

    public IResolutionServices CreateResolutionServices(object[] overrides)
    {
      return new ResolutionServices(this, new StaticOverrideLookup(overrides));
    }
  }
}