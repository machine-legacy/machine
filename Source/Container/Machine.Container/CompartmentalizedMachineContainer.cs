using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Plugins;
using Machine.Container.Services;
using Machine.Container.Services.Impl;

namespace Machine.Container
{
  public class CompartmentalizedMachineContainer : IMachineContainer
  {
    #region Member Data
    private readonly ContainerStatePolicy _state = new ContainerStatePolicy();
    private readonly IPluginManager _pluginManager;
    private readonly IListenerInvoker _listenerInvoker;
    private readonly IContainerInfrastructureFactory _containerInfrastructureFactory;
    private IObjectInstances _objectInstances;
    private IServiceEntryResolver _resolver;
    private IRootActivatorFactory _rootActivatorFactory;
    private IRootActivatorResolver _rootActivatorResolver;
    private IActivatorStore _activatorStore;
    private ILifestyleFactory _lifestyleFactory;
    private IServiceGraph _serviceGraph;
    private IContainerServices _containerServices;
    private ContainerRegisterer _containerRegisterer;
    private ContainerResolver _containerResolver;
    private PluginServices _pluginServices;
    #endregion

    public CompartmentalizedMachineContainer(IContainerInfrastructureFactory containerInfrastructureFactory)
    {
      _containerInfrastructureFactory = containerInfrastructureFactory;
      _pluginManager = new PluginManager();
      _listenerInvoker = new ListenerInvoker(_pluginManager);
    }

    public CompartmentalizedMachineContainer()
     : this(new DefaultContainerInfrastructureFactory())
    {
    }

    #region IHighLevelContainer Members
    // Plugins / Listeners
    public void AddPlugin(IServiceContainerPlugin plugin)
    {
      _state.AssertCanAddPlugins();
      _pluginManager.AddPlugin(plugin);
    }

    public void AddListener(IServiceContainerListener listener)
    {
      _state.AssertCanAddListeners();
      _pluginManager.AddListener(listener);
    }

    // Deactivation
    public void Deactivate(object instance)
    {
      _state.AssertCanDeactivate();
      IResolutionServices services = _containerServices.CreateResolutionServices(new StaticOverrideLookup(new object[0]), LookupFlags.Default);
      _objectInstances.Deactivate(services, instance);
    }

    // Miscellaneous
    public bool CanResolve<T>()
    {
      return CanResolve(typeof(T));
    }

    public bool CanResolve(Type type)
    {
      _state.AssertIsInitialized();
      IResolutionServices services = _containerServices.CreateResolutionServices(new StaticOverrideLookup(new object[0]), LookupFlags.None);
      ResolvedServiceEntry dependencyEntry = _containerServices.ServiceEntryResolver.ResolveEntry(services, type);
      return dependencyEntry != null;
    }

    public IEnumerable<ServiceRegistration> RegisteredServices
    {
      get
      {
        _state.AssertIsInitialized();
        return _serviceGraph.RegisteredServices;
      }
    }
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
      _listenerInvoker.Dispose();
      _pluginManager.Dispose();
    }
    #endregion

    public virtual void Initialize()
    {
      _rootActivatorResolver = _containerInfrastructureFactory.CreateDependencyResolver();
      IServiceEntryFactory serviceEntryFactory = new ServiceEntryFactory();
      _serviceGraph = new ServiceGraph(_listenerInvoker);
      _resolver = new ServiceEntryResolver(_serviceGraph, serviceEntryFactory, _rootActivatorResolver);
      _activatorStore = new ActivatorStore();
      _rootActivatorFactory = _containerInfrastructureFactory.CreateActivatorFactory(_resolver);
      _pluginServices = new PluginServices(_state, this, _rootActivatorResolver, _rootActivatorFactory);
      _objectInstances = new ObjectInstances(_listenerInvoker, _containerInfrastructureFactory.CreateInstanceTrackingPolicy());
      _lifestyleFactory = new LifestyleFactory(_rootActivatorFactory);
      _containerServices = CreateContainerServices();
      _containerRegisterer = new ContainerRegisterer(_containerServices);
      _containerResolver = new ContainerResolver(_containerServices, _containerRegisterer);
      _state.Initialize();
    }

    public virtual void PrepareForServices()
    {
      _pluginManager.Initialize(_pluginServices);
      _listenerInvoker.InitializeListener(this);
      _state.PrepareForServices();
      RegisterContainerInContainer();
      _listenerInvoker.PreparedForServices();
      _pluginManager.ReadyForServices(_pluginServices);
    }

    public virtual void Start()
    {
      _state.Start();
      _listenerInvoker.Started();
    }

    public ContainerRegisterer Register
    {
      get
      {
        _state.AssertCanAddServices();
        return _containerRegisterer;
      }
    }

    public ContainerResolver Resolve
    {
      get
      {
        _state.AssertCanActivate();
        return _containerResolver;
      }
    }

    protected virtual IContainerServices CreateContainerServices()
    {
      return new ContainerServices(_activatorStore, _rootActivatorFactory, _lifestyleFactory, _listenerInvoker, _objectInstances, _resolver, _serviceGraph, _state);
    }

    protected virtual void RegisterContainerInContainer()
    {
      _containerRegisterer.Type<IMachineContainer>().Is(this);
    }
  }
}