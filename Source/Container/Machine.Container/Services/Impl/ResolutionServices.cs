using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Plugins;

namespace Machine.Container.Services.Impl
{
  [CoverageExclude]
  public class ResolutionServices : IResolutionServices
  {
    private readonly DependencyGraphTracker _dependencyGraphTracker = new DependencyGraphTracker();
    private readonly IOverrideLookup _overrideLookup;
    private readonly IContainerServices _containerServices;
    private readonly LookupFlags _flags;

    public ResolutionServices(IContainerServices containerServices, IOverrideLookup overrideLookup, LookupFlags flags)
    {
      _containerServices = containerServices;
      _flags = flags;
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

    public ContainerStatePolicy StatePolicy
    {
      get { return _containerServices.StatePolicy; }
    }

    public IActivatorStore ActivatorStore
    {
      get { return _containerServices.ActivatorStore; }
    }

    public IActivatorFactory ActivatorFactory
    {
      get { return _containerServices.ActivatorFactory; }
    }

    public ILifestyleFactory LifestyleFactory
    {
      get { return _containerServices.LifestyleFactory; }
    }

    public IServiceEntryFactory ServiceEntryFactory
    {
      get { return _containerServices.ServiceEntryFactory; }
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

    public IServiceGraph ServiceGraph
    {
      get { return _containerServices.ServiceGraph; }
    }

    public LookupFlags Flags
    {
      get { return _flags; }
    }
  }
}