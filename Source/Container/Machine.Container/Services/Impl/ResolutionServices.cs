using System;
using System.Collections.Generic;

using Machine.Container.Plugins;

namespace Machine.Container.Services.Impl
{
  [CoverageExclude]
  public class ContainerServices : IContainerServices
  {
    private readonly IActivatorStrategy _activatorStrategy;
    private readonly IActivatorStore _activatorStore;
    private readonly ILifestyleFactory _lifestyleFactory;
    private readonly IServiceEntryResolver _serviceEntryResolver;
    private readonly IListenerInvoker _listenerInvoker;
    private readonly IObjectInstances _objectInstances;

    public ContainerServices(IActivatorStore activatorStore, IActivatorStrategy activatorStrategy, ILifestyleFactory lifestyleFactory, IListenerInvoker listenerInvoker, IObjectInstances objectInstances, IServiceEntryResolver serviceEntryResolver)
    {
      _activatorStore = activatorStore;
      _activatorStrategy = activatorStrategy;
      _lifestyleFactory = lifestyleFactory;
      _listenerInvoker = listenerInvoker;
      _objectInstances = objectInstances;
      _serviceEntryResolver = serviceEntryResolver;
    }

    public IActivatorStrategy ActivatorStrategy
    {
      get { return _activatorStrategy; }
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

    public IResolutionServices CreateResolutionServices(object[] serviceOverrides)
    {
      return new ResolutionServices(this, new StaticOverrideLookup(serviceOverrides));
    }
  }
  [CoverageExclude]
  public class ResolutionServices : IResolutionServices
  {
    private readonly DependencyGraphTracker _dependencyGraphTracker = new DependencyGraphTracker();
    private readonly IOverrideLookup _overrideLookup;
    private readonly IContainerServices _containerServices;

    public ResolutionServices(IContainerServices containerServices, IOverrideLookup overrideLookup)
    {
      _containerServices = containerServices;
      _overrideLookup = overrideLookup;
    }

    public IContainerServices ContainerServices
    {
      get { return _containerServices; }
    }

    public IOverrideLookup Overrides
    {
      get { return _overrideLookup; }
    }

    public DependencyGraphTracker DependencyGraphTracker
    {
      get { return _dependencyGraphTracker; }
    }

    public IActivatorStore ActivatorStore
    {
      get { return _containerServices.ActivatorStore; }
    }

    public IActivatorStrategy ActivatorStrategy
    {
      get { return _containerServices.ActivatorStrategy; }
    }

    public ILifestyleFactory LifestyleFactory
    {
      get { return _containerServices.LifestyleFactory; }
    }

    public IServiceEntryResolver ServiceEntryResolver
    {
      get { return _containerServices.ServiceEntryResolver; }
    }

    public IListenerInvoker ListenerInvoker
    {
      get { return _containerServices.ListenerInvoker; }
    }

    public IObjectInstances ObjectInstances
    {
      get { return _containerServices.ObjectInstances; }
    }
  }
}