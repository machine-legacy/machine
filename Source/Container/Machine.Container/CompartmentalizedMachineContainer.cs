using System;
using System.Collections.Generic;

using Machine.Container.Configuration;
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
    protected IObjectInstances _objectInstances;
    protected IServiceEntryResolver _resolver;
    protected IActivatorStrategy _activatorStrategy;
    protected IActivatorStore _activatorStore;
    protected ILifestyleFactory _lifestyleFactory;
    protected IServiceGraph _serviceGraph;
    protected IContainerServices _containerServices;
    #endregion

    public CompartmentalizedMachineContainer()
    {
      _pluginManager = new PluginManager(this);
      _listenerInvoker = new ListenerInvoker(_pluginManager);
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

    protected virtual IContainerServices CreateContainerServices()
    {
      return new ContainerServices(_activatorStore, _activatorStrategy, _lifestyleFactory, _listenerInvoker, _objectInstances, _resolver, _serviceGraph);
    }

    protected virtual IActivatorResolver CreateDependencyResolver()
    {
      return new RootActivatorResolver(new StaticLookupActivatorResolver(), new ActivatorStoreActivatorResolver(), new ThrowsPendingActivatorResolver());
    }

    protected virtual void RegisterContainerInContainer()
    {
      ServiceEntry entry = _containerServices.ServiceEntryResolver.CreateEntryIfMissing(typeof(IHighLevelContainer));
      IActivator activator = _activatorStrategy.CreateStaticActivator(entry, this);
      _activatorStore.AddActivator(entry, activator);
    }

    public virtual void Initialize()
    {
      IActivatorResolver activatorResolver = CreateDependencyResolver();
      IServiceEntryFactory serviceEntryFactory = new ServiceEntryFactory();
      IServiceDependencyInspector serviceDependencyInspector = new ServiceDependencyInspector();
      _serviceGraph = new ServiceGraph(_listenerInvoker);
      _resolver = new ServiceEntryResolver(_serviceGraph, serviceEntryFactory, activatorResolver);
      _activatorStrategy = new DefaultActivatorStrategy(new DotNetObjectFactory(), _resolver, serviceDependencyInspector);
      _activatorStore = new ActivatorStore();
      _lifestyleFactory = new LifestyleFactory(_activatorStrategy);
      _objectInstances = new ObjectInstances(_listenerInvoker);
      _containerServices = CreateContainerServices();
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
      get { return new ContainerRegisterer(_containerServices); }
    }

    public ContainerResolver Resolver
    {
      get { return new ContainerResolver(_containerServices); }
    }
  }
  public class ContainerRegisterer
  {
    readonly IContainerServices _containerServices;

    public ContainerRegisterer(IContainerServices containerServices)
    {
      _containerServices = containerServices;
    }

    public RegistrationConfigurer Implementation(Type implementationType)
    {
      ServiceEntry entry = _containerServices.ServiceEntryResolver.CreateEntryIfMissing(implementationType);
      return new RegistrationConfigurer(_containerServices.ActivatorStrategy, _containerServices.ActivatorStore, entry);
    }

    public RegistrationConfigurer Implementation<TImplementation>()
    {
      return Implementation(typeof(TImplementation));
    }
  }
  public class ContainerResolver
  {
    readonly IContainerServices _containerServices;

    public ContainerResolver(IContainerServices containerServices)
    {
      _containerServices = containerServices;
    }

    public object Resolve(Type serviceType)
    {
      return ResolveWithOverrides(serviceType);
    }

    public T Resolve<T>()
    {
      return (T)Resolve(typeof(T));
    }

    public T New<T>(params object[] serviceOverrides)
    {
      ServiceEntry entry = _containerServices.ServiceEntryResolver.CreateEntryIfMissing(typeof(T));
      entry.LifestyleType = LifestyleType.Transient;
      return ResolveWithOverrides<T>(serviceOverrides);
    }

    public T ResolveWithOverrides<T>(params object[] serviceOverrides)
    {
      return (T)ResolveWithOverrides(typeof(T), serviceOverrides);
    }

    public object ResolveWithOverrides(Type serviceType, params object[] serviceOverrides)
    {
      IResolutionServices services = _containerServices.CreateResolutionServices(serviceOverrides);
      ResolvedServiceEntry entry = _containerServices.ServiceEntryResolver.ResolveEntry(services, serviceType, true);
      return entry.Activate(services);
    }

    public IList<T> ResolveAll<T>()
    {
      List<T> found = new List<T>();
      foreach (ServiceRegistration registration in _containerServices.ServiceGraph.RegisteredServices)
      {
        if (typeof(T).IsAssignableFrom(registration.ImplementationType))
        {
          found.Add((T)Resolve(registration.ImplementationType));
        }
      }
      return found;
    }
  }
}