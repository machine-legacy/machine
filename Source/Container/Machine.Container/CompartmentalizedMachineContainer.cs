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
    protected readonly ContainerStatePolicy _state = new ContainerStatePolicy();
    protected readonly IPluginManager _pluginManager;
    protected readonly IListenerInvoker _listenerInvoker;
    protected readonly IContainerInfrastructureFactory _containerInfrastructureFactory;
    protected IObjectInstances _objectInstances;
    protected IServiceEntryResolver _resolver;
    protected IActivatorFactory _activatorFactory;
    protected IActivatorStore _activatorStore;
    protected ILifestyleFactory _lifestyleFactory;
    protected IServiceGraph _serviceGraph;
    protected IContainerServices _containerServices;
    protected ContainerRegisterer _containerRegisterer;
    protected ContainerResolver _containerResolver;
    #endregion

    public CompartmentalizedMachineContainer(IContainerInfrastructureFactory containerInfrastructureFactory)
    {
      _containerInfrastructureFactory = containerInfrastructureFactory;
      _pluginManager = new PluginManager(this);
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
      IResolutionServices services = _containerServices.CreateResolutionServices(new object[0]);
      _objectInstances.Deactivate(services, instance);
    }

    // Miscellaneous
    public bool CanResolve<T>()
    {
      _state.AssertIsInitialized();
      ServiceEntry entry = _resolver.LookupEntry(typeof(T));
      return entry != null;
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
      IActivatorResolver activatorResolver = _containerInfrastructureFactory.CreateDependencyResolver();
      IServiceEntryFactory serviceEntryFactory = new ServiceEntryFactory();
      _serviceGraph = new ServiceGraph(_listenerInvoker);
      _resolver = new ServiceEntryResolver(_serviceGraph, serviceEntryFactory, activatorResolver);
      _activatorStore = new ActivatorStore();
      // _activatorStrategy = new DefaultActivatorFactory(new DotNetObjectFactory(), _resolver, new ServiceDependencyInspector());
      _activatorFactory = _containerInfrastructureFactory.CreateActivatorFactory(_resolver);
      _objectInstances = new ObjectInstances(_listenerInvoker, _containerInfrastructureFactory.CreateInstanceTrackingPolicy());
      _lifestyleFactory = new LifestyleFactory(_activatorFactory);
      _containerServices = CreateContainerServices();
      _containerRegisterer = new ContainerRegisterer(_containerServices);
      _containerResolver = new ContainerResolver(_containerServices, _containerRegisterer);
      _state.Initialize();
    }

    public virtual void PrepareForServices()
    {
      _pluginManager.Initialize();
      _listenerInvoker.Initialize(this);
      _state.PrepareForServices();
      RegisterContainerInContainer();
      _listenerInvoker.PreparedForServices();
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
      return new ContainerServices(_activatorStore, _activatorFactory, _lifestyleFactory, _listenerInvoker, _objectInstances, _resolver, _serviceGraph);
    }

    protected virtual void RegisterContainerInContainer()
    {
      _containerRegisterer.Type<IMachineContainer>().Is(this);
    }
  }
  public class DefaultContainerInfrastructureFactory : IContainerInfrastructureFactory
  {
    #region IContainerInfrastructureFactory Members
    public virtual IActivatorResolver CreateDependencyResolver()
    {
      return new RootActivatorResolver(new StaticLookupActivatorResolver(), new ActivatorStoreActivatorResolver(), new ThrowsPendingActivatorResolver());
    }

    public virtual IInstanceTrackingPolicy CreateInstanceTrackingPolicy()
    {
      return new GlobalActivationScope();
    }

    public virtual IActivatorFactory CreateActivatorFactory(IServiceEntryResolver entryResolver)
    {
      return new DefaultActivatorFactory(new DotNetObjectFactory(), new ServiceDependencyInspector(), entryResolver);
    }
    #endregion
  }
}