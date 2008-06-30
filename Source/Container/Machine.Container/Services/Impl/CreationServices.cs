using System;
using System.Collections.Generic;

using Machine.Container.Plugins;

namespace Machine.Container.Services.Impl
{
  [CoverageExclude]
  public class CreationServices : ICreationServices
  {
    #region Member Data
    private readonly DependencyGraphTracker _dependencyGraphTracker = new DependencyGraphTracker();
    private readonly IActivatorStrategy _activatorStrategy;
    private readonly IActivatorStore _activatorStore;
    private readonly ILifestyleFactory _lifestyleFactory;
    private readonly IOverrideLookup _overrideLookup;
    private readonly IServiceEntryResolver _serviceEntryResolver;
    private readonly IListenerInvoker _listenerInvoker;
    private readonly IObjectInstances _objectInstances;
    #endregion

    #region CreationServices()
    public CreationServices(IActivatorStrategy activatorStrategy, IActivatorStore activatorStore, ILifestyleFactory lifestyleFactory, IOverrideLookup overrideLookup, IServiceEntryResolver serviceEntryResolver, IListenerInvoker listenerInvoker, IObjectInstances objectInstances)
    {
      _activatorStore = activatorStore;
      _objectInstances = objectInstances;
      _listenerInvoker = listenerInvoker;
      _serviceEntryResolver = serviceEntryResolver;
      _lifestyleFactory = lifestyleFactory;
      _activatorStrategy = activatorStrategy;
      _overrideLookup = overrideLookup;
    }
    #endregion

    #region ICreationServices Members
    public DependencyGraphTracker DependencyGraphTracker
    {
      get { return _dependencyGraphTracker; }
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

    public IOverrideLookup Overrides
    {
      get { return _overrideLookup; }
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
    #endregion
  }
}