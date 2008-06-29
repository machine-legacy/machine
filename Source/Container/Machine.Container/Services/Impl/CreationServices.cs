using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services.Impl
{
  [CoverageExclude]
  public class CreationServices : ICreationServices
  {
    #region Member Data
    readonly DependencyGraphTracker _dependencyGraphTracker = new DependencyGraphTracker();
    readonly IActivatorStrategy _activatorStrategy;
    readonly IActivatorStore _activatorStore;
    readonly ILifestyleFactory _lifestyleFactory;
    readonly IOverrideLookup _overrideLookup;
    readonly IServiceEntryResolver _serviceEntryResolver;
    #endregion

    #region CreationServices()
    public CreationServices(IActivatorStrategy activatorStrategy, IActivatorStore activatorStore, ILifestyleFactory lifestyleFactory, IOverrideLookup overrideLookup, IServiceEntryResolver serviceEntryResolver)
    {
      _activatorStore = activatorStore;
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
    #endregion
  }
}