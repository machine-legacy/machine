using System;
using System.Collections.Generic;
using System.Threading;

using Machine.Container.Model;
using Machine.Container.Plugins;
using Machine.Container.Services;
using Machine.Container.Services.Impl;
using Machine.Core.Utility;

namespace Machine.Container
{
  public interface IServiceRegisterer
  {
  }
  public class ServiceRegisterer : IServiceRegisterer
  {
  }
  public class MachineContainer : IHighLevelContainer
  {
    #region Member Data
    private readonly IPluginManager _pluginManager;
    private IServiceEntryResolver _resolver;
    private IActivatorStrategy _activatorStrategy;
    private IActivatorStore _activatorStore;
    private ILifestyleFactory _lifestyleFactory;
    private IServiceGraph _serviceGraph;
    #endregion

    public MachineContainer()
    {
      _pluginManager = new PluginManager(this);
    }

    #region Methods
    public virtual void Initialize()
    {
      IActivatorResolver activatorResolver = CreateDependencyResolver();
      IServiceEntryFactory serviceEntryFactory = new ServiceEntryFactory();
      IServiceDependencyInspector serviceDependencyInspector = new ServiceDependencyInspector();
      _serviceGraph = new ServiceGraph();
      _resolver = new ServiceEntryResolver(_serviceGraph, serviceEntryFactory, activatorResolver);
      _activatorStrategy = new DefaultActivatorStrategy(new DotNetObjectFactory(), _resolver, serviceDependencyInspector);
      _activatorStore = new ActivatorStore();
      _lifestyleFactory = new LifestyleFactory(_activatorStrategy);
      AddService<IHighLevelContainer>(this);
      _pluginManager.Initialize();
    }

    public virtual IActivatorResolver CreateDependencyResolver()
    {
      return new RootActivatorResolver(new StaticLookupActivatorResolver(), new DefaultLifestyleAwareActivatorResolver(), new ThrowsPendingActivatorResolver());
    }
    #endregion

    #region IHighLevelContainer Members
    // Plugins / Listeners
    public void AddPlugin(IServiceContainerPlugin plugin)
    {
      _pluginManager.AddPlugin(plugin);
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
      ServiceEntry entry = _resolver.CreateEntryIfMissing(serviceType, implementationType);
      entry.LifestyleType = lifestyleType;
    }

    public void AddService<TService>(object instance)
    {
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
      ICreationServices services = CreateCreationServices(serviceOverrides);
      ResolvedServiceEntry entry = _resolver.ResolveEntry(services, serviceType, true);
      return entry.Activator.Activate(services);
    }

    // Releasing
    public void Release(object instance)
    {
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
      _pluginManager.Dispose();
    }
    #endregion

    #region Methods
    protected virtual ICreationServices CreateCreationServices(params object[] serviceOverrides)
    {
      IOverrideLookup overrides = new StaticOverrideLookup(serviceOverrides);
      return new CreationServices(_activatorStrategy, _activatorStore, _lifestyleFactory, overrides, _resolver);
    }
    #endregion
  }
}
