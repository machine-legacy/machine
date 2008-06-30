using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Plugins;
using Machine.Container.Services;
using Machine.Container.Services.Impl;

namespace Machine.Container
{
  public class MachineContainer : IHighLevelContainer
  {
    #region Member Data
    private readonly ContainerStatePolicy _state = new ContainerStatePolicy();
    private readonly IPluginManager _pluginManager;
    private readonly IListenerInvoker _listenerInvoker;
    private IObjectInstances _objectInstances;
    private IServiceEntryResolver _resolver;
    private IActivatorStrategy _activatorStrategy;
    private IActivatorStore _activatorStore;
    private ILifestyleFactory _lifestyleFactory;
    private IServiceGraph _serviceGraph;
    #endregion

    public MachineContainer()
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

    // Adding Services / Registration
    public void AddService<TService>()
    {
      AddService<TService>(LifestyleType.Singleton);
    }

    public void AddService<TService>(LifestyleType lifestyleType)
    {
      AddService(typeof(TService), lifestyleType);
    }

    public void AddService(Type serviceType, LifestyleType lifestyleType)
    {
      _state.AssertCanAddServices();
      ServiceEntry entry = _resolver.CreateEntryIfMissing(serviceType);
      entry.LifestyleType = lifestyleType;
    }

    public void AddService<TService>(Type implementationType)
    {
      AddService(typeof(TService), implementationType, LifestyleType.Singleton);
    }

    public void AddService<TService, TImpl>()
    {
      AddService<TService, TImpl>(LifestyleType.Singleton);
    }

    public void AddService<TService, TImpl>(LifestyleType lifestyleType)
    {
      AddService(typeof(TService), typeof(TImpl), lifestyleType);
    }

    public void AddService(Type serviceType, Type implementationType, LifestyleType lifestyleType)
    {
      _state.AssertCanAddServices();
      ServiceEntry entry = _resolver.CreateEntryIfMissing(serviceType, implementationType);
      entry.LifestyleType = lifestyleType;
    }

    public void AddService<TService>(object instance)
    {
      _state.AssertCanAddServices();
      ServiceEntry entry = _resolver.CreateEntryIfMissing(typeof(TService));
      IActivator activator = _activatorStrategy.CreateStaticActivator(entry, instance);
      _activatorStore.AddActivator(entry, activator);
    }

    // Resolving
    public T Resolve<T>()
    {
      return (T)Resolve(typeof(T));
    }

    public object Resolve(Type serviceType)
    {
      return ResolveWithOverrides(serviceType);
    }

    public T New<T>(params object[] serviceOverrides)
    {
      AddService<T>(LifestyleType.Transient);
      return ResolveWithOverrides<T>(serviceOverrides);
    }

    public T ResolveWithOverrides<T>(params object[] serviceOverrides)
    {
      return (T)ResolveWithOverrides(typeof(T), serviceOverrides);
    }

    public object ResolveWithOverrides(Type serviceType, params object[] serviceOverrides)
    {
      _state.AssertCanResolve();
      IContainerServices services = CreateCreationServices(serviceOverrides);
      ResolvedServiceEntry entry = _resolver.ResolveEntry(services, serviceType, true);
      return entry.Activate(services);
    }

    // Releasing
    public void Release(object instance)
    {
      _state.AssertCanRelease();
      IContainerServices services = CreateCreationServices();
      _objectInstances.Release(services, instance);
    }

    // Miscellaneous
    public bool HasService<T>()
    {
      ServiceEntry entry = _resolver.LookupEntry(typeof(T));
      return entry != null;
    }

    public IEnumerable<ServiceRegistration> RegisteredServices
    {
      get { return _serviceGraph.RegisteredServices; }
    }
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
      _listenerInvoker.Dispose();
      _pluginManager.Dispose();
    }
    #endregion

    protected virtual IContainerServices CreateCreationServices(params object[] serviceOverrides)
    {
      IOverrideLookup overrides = new StaticOverrideLookup(serviceOverrides);
      return new ContainerServices(_activatorStrategy, _activatorStore, _lifestyleFactory, overrides, _resolver, _listenerInvoker, _objectInstances);
    }

    protected virtual IActivatorResolver CreateDependencyResolver()
    {
      return new RootActivatorResolver(new StaticLookupActivatorResolver(), new DefaultLifestyleAwareActivatorResolver(), new ThrowsPendingActivatorResolver());
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
      _state.Initialize();
    }

    public virtual void PrepareForServices()
    {
      _state.PrepareForServices();
      AddService<IHighLevelContainer>(this);
      _pluginManager.Initialize();
      _listenerInvoker.Initialize(this);
      _listenerInvoker.PreparedForServices();
    }

    public virtual void Start()
    {
      _state.Start();
      _listenerInvoker.Started();
    }
  }
  /*
  public class CompartmentalizedMachineContainer : IMachineContainer
  {
    #region IDisposable Members
    public void Dispose()
    {
    }
    #endregion
  }
  */
}
