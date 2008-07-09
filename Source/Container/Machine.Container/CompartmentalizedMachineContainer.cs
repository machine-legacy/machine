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
    private IActivatorFactory _activatorFactory;
    private IActivatorStore _activatorStore;
    private ILifestyleFactory _lifestyleFactory;
    private IServiceGraph _serviceGraph;
    private IContainerServices _containerServices;
    private ContainerRegisterer _containerRegisterer;
    private ContainerResolver _containerResolver;
    private IRootActivatorResolver _rootActivatorResolver;
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
      IResolutionServices services = _containerServices.CreateResolutionServices(new object[0]);
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
      ServiceEntry entry = _resolver.LookupEntry(type);
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
      _rootActivatorResolver = _containerInfrastructureFactory.CreateDependencyResolver();
      _pluginServices = new PluginServices(_state, this, _rootActivatorResolver);
      IServiceEntryFactory serviceEntryFactory = new ServiceEntryFactory();
      _serviceGraph = new ServiceGraph(_listenerInvoker);
      _resolver = new ServiceEntryResolver(_serviceGraph, serviceEntryFactory, _rootActivatorResolver);
      _activatorStore = new ActivatorStore();
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
      _pluginManager.Initialize(_pluginServices);
      _listenerInvoker.InitializeListener(this);
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
      return new ContainerServices(_activatorStore, _activatorFactory, _lifestyleFactory, _listenerInvoker, _objectInstances, _resolver, _serviceGraph, _state);
    }

    protected virtual void RegisterContainerInContainer()
    {
      _containerRegisterer.Type<IMachineContainer>().Is(this);
    }
  }
  public class DefaultContainerInfrastructureFactory : IContainerInfrastructureFactory
  {
    #region IContainerInfrastructureFactory Members
    public virtual IRootActivatorResolver CreateDependencyResolver()
    {
      RootActivatorResolver resolver = new RootActivatorResolver();
      resolver.AddLast(new StaticLookupActivatorResolver());
      resolver.AddLast(new ActivatorStoreActivatorResolver());
      resolver.AddLast(new ThrowsPendingActivatorResolver());
      return resolver;
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