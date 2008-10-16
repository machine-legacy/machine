using System;
using System.Collections.Generic;

using Machine.Container.Configuration;
using Machine.Container.Model;
using Machine.Container.Plugins;
using Machine.Container.Services;

namespace Machine.Container
{
  public class MachineContainer : IHighLevelContainer
  {
    private readonly CompartmentalizedMachineContainer _container;

    public MachineContainer(CompartmentalizedMachineContainer container)
    {
      _container = container;
    }

    public MachineContainer()
     : this(new CompartmentalizedMachineContainer())
    {
    }

    // Adding Services / Registration
    public void Add<TService>()
    {
      Add<TService>(LifestyleType.Singleton);
    }

    public void Add<TService>(LifestyleType lifestyleType)
    {
      Add(typeof(TService), lifestyleType);
    }

    public void Add(Type serviceType, LifestyleType lifestyleType)
    {
      Register.Type(serviceType).WithLifestyle(lifestyleType);
    }

    public void Add<TService>(Type implementationType)
    {
      Add(typeof(TService), implementationType, LifestyleType.Singleton);
    }

    public void Add<TService, TImpl>()
    {
      Add<TService, TImpl>(LifestyleType.Singleton);
    }

    public void Add<TService, TImpl>(LifestyleType lifestyleType)
    {
      Add(typeof(TService), typeof(TImpl), lifestyleType);
    }

    public void Add(Type serviceType, Type implementationType, LifestyleType lifestyleType)
    {
      Register.Type(serviceType).ImplementedBy(implementationType).WithLifestyle(lifestyleType);
    }

    public void Add<TService>(object instance)
    {
      Add(typeof(TService), instance);
    }

    public void Add(Type serviceType, object instance)
    {
      Register.Type(serviceType).Is(instance);
    }

    // Resolving
    public T ResolveObject<T>()
    {
      return (T)ResolveObject(typeof(T));
    }

    public object ResolveObject(Type type)
    {
      return ResolveWithOverrides(type);
    }

    public T New<T>(params object[] overrides)
    {
      return Resolve.New<T>(overrides);
    }

    public T ResolveWithOverrides<T>(params object[] overrides)
    {
      return (T)ResolveWithOverrides(typeof(T), overrides);
    }

    public T ResolveWithParameters<T>(System.Collections.IDictionary parameters)
    {
      return Resolve.ObjectWithParameters<T>(parameters);
    }
    
    public object ResolveWithOverrides(Type type, params object[] overrides)
    {
      return Resolve.Object(type, overrides);
    }

    #region IMachineContainer Members
    public void AddPlugin(IServiceContainerPlugin plugin)
    {
      _container.AddPlugin(plugin);
    }

    public void AddListener(IServiceContainerListener listener)
    {
      _container.AddListener(listener);
    }

    public void Initialize()
    {
      _container.Initialize();
      ReadyForPlugins();
    }

    public virtual void ReadyForPlugins()
    {
    }

    public void PrepareForServices()
    {
      _container.PrepareForServices();
      ReadyForServices();
    }

    public virtual void ReadyForServices()
    {
    }

    public void Start()
    {
      _container.Start();
    }

    public ContainerRegisterer Register
    {
      get { return _container.Register; }
    }

    public ContainerResolver Resolve
    {
      get { return _container.Resolve; }
    }

    public IEnumerable<ServiceRegistration> RegisteredServices
    {
      get { return _container.RegisteredServices; }
    }

    public void Deactivate(object instance)
    {
      _container.Deactivate(instance);
    }

    public bool CanResolve<T>()
    {
      return CanResolve(typeof(T));
    }

    public bool CanResolve(Type type)
    {
      return _container.CanResolve(type);
    }
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
      _container.Dispose();
    }
    #endregion
  }
}
