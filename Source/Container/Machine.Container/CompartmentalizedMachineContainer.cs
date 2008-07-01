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
    protected readonly IDependencyResolverFactory _dependencyResolverFactory;
    protected IObjectInstances _objectInstances;
    protected IServiceEntryResolver _resolver;
    protected IActivatorStrategy _activatorStrategy;
    protected IActivatorStore _activatorStore;
    protected ILifestyleFactory _lifestyleFactory;
    protected IServiceGraph _serviceGraph;
    protected IContainerServices _containerServices;
    protected ContainerRegisterer _containerRegisterer;
    protected ContainerResolver _containerResolver;
    #endregion

    public CompartmentalizedMachineContainer(IDependencyResolverFactory dependencyResolverFactory)
    {
      _dependencyResolverFactory = dependencyResolverFactory;
      _pluginManager = new PluginManager(this);
      _listenerInvoker = new ListenerInvoker(_pluginManager);
    }

    public CompartmentalizedMachineContainer()
     : this(new DefaultDependencyResolverFactory())
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

    // Releasing
    public void Release(object instance)
    {
      _state.AssertCanRelease();
      IResolutionServices services = _containerServices.CreateResolutionServices(new object[0]);
      _objectInstances.Release(services, instance);
    }

    // Miscellaneous
    public bool HasService<T>()
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
      IActivatorResolver activatorResolver = _dependencyResolverFactory.CreateDependencyResolver();
      IServiceEntryFactory serviceEntryFactory = new ServiceEntryFactory();
      IServiceDependencyInspector serviceDependencyInspector = new ServiceDependencyInspector();
      _serviceGraph = new ServiceGraph(_listenerInvoker);
      _resolver = new ServiceEntryResolver(_serviceGraph, serviceEntryFactory, activatorResolver);
      _activatorStrategy = new DefaultActivatorStrategy(new DotNetObjectFactory(), _resolver, serviceDependencyInspector);
      _activatorStore = new ActivatorStore();
      _lifestyleFactory = new LifestyleFactory(_activatorStrategy);
      _objectInstances = new ObjectInstances(_listenerInvoker);
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
        _state.AssertCanResolve();
        return _containerResolver;
      }
    }

    protected virtual IContainerServices CreateContainerServices()
    {
      return new ContainerServices(_activatorStore, _activatorStrategy, _lifestyleFactory, _listenerInvoker, _objectInstances, _resolver, _serviceGraph);
    }

    protected virtual void RegisterContainerInContainer()
    {
      _containerRegisterer.Type<IMachineContainer>().Is(this);
    }
  }
  public class DefaultDependencyResolverFactory : IDependencyResolverFactory
  {
    #region IDependencyResolverFactory Members
    public IActivatorResolver CreateDependencyResolver()
    {
      return new RootActivatorResolver(new StaticLookupActivatorResolver(), new ActivatorStoreActivatorResolver(), new ThrowsPendingActivatorResolver());
    }
    #endregion
  }
}