using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services.Impl
{
  [CoverageExclude]
  public class CreationServices : ICreationServices
  {
    #region Member Data
    readonly Stack<ServiceEntry> _progress = new Stack<ServiceEntry>();
    readonly IActivatorStrategy _activatorStrategy;
    readonly IActivatorStore _activatorStore;
    readonly ILifestyleFactory _lifestyleFactory;
    readonly IOverrideLookup _overrideLookup;
    #endregion

    #region CreationServices()
    public CreationServices(IActivatorStrategy activatorStrategy, IActivatorStore activatorStore,
      ILifestyleFactory lifestyleFactory, IOverrideLookup overrideLookup)
    {
      _activatorStore = activatorStore;
      _lifestyleFactory = lifestyleFactory;
      _activatorStrategy = activatorStrategy;
      _overrideLookup = overrideLookup;
    }
    #endregion

    #region ICreationServices Members
    public Stack<ServiceEntry> Progress
    {
      get { return _progress; }
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
    #endregion
  }
}