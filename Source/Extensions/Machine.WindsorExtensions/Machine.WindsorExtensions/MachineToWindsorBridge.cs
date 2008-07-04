using System;
using System.Collections.Generic;

using Castle.Windsor;

using Machine.Container;
using Machine.Container.Model;
using Machine.Container.Plugins;
using Machine.Container.Services;

using LifestyleType = Castle.Core.LifestyleType;

namespace Machine.WindsorExtensions
{
  public class MachineToWindsorBridge : IHighLevelContainer
  {
    readonly IWindsorContainer _windsor;
    readonly WindsorWrapper _wrapper;

    public MachineToWindsorBridge(IWindsorContainer windsor)
    {
      _windsor = windsor;
      _wrapper = new WindsorWrapper(_windsor);
    }

    #region IHighLevelContainer Members
    public void Add(Type serviceType, Machine.Container.Model.LifestyleType lifestyleType)
    {
      _wrapper.AddService(serviceType, Convert(lifestyleType));
    }

    public void Add<TService>()
    {
      _wrapper.AddService<TService>();
    }

    public void Add<TService>(Type implementation)
    {
      _wrapper.AddService<TService>(implementation);
    }

    public void Add<TService, TImpl>(Machine.Container.Model.LifestyleType lifestyleType)
    {
      _wrapper.AddService<TService, TImpl>(Convert(lifestyleType));
    }

    public void Add<TService, TImpl>()
    {
      _wrapper.AddService<TService, TImpl>();
    }

    public void Add<TService>(Machine.Container.Model.LifestyleType lifestyleType)
    {
      _wrapper.AddService(typeof(TService), Convert(lifestyleType));
    }

    public void Add<TService>(object instance)
    {
      _wrapper.AddService<TService>(instance);
    }

    public void Add(Type serviceType, object instance)
    {
      _wrapper.AddService(instance);
    }

    public T ResolveObject<T>()
    {
      return _windsor.Resolve<T>();
    }

    public object ResolveObject(Type type)
    {
      return _windsor.Resolve(type);
    }

    public T ResolveWithOverrides<T>(params object[] serviceOverrides)
    {
      throw new NotImplementedException();
    }

    public T New<T>(params object[] serviceOverrides)
    {
      throw new NotImplementedException();
    }

    public void Deactivate(object instance)
    {
      _windsor.Release(instance);
    }

    public bool CanResolve<T>()
    {
      return _windsor.Kernel.HasComponent(typeof(T));
    }

    public IEnumerable<ServiceRegistration> RegisteredServices
    {
      get { throw new NotImplementedException(); }
    }

    public void AddPlugin(IServiceContainerPlugin plugin)
    {
      throw new NotImplementedException();
    }

    public void AddListener(IServiceContainerListener listener)
    {
      throw new NotImplementedException();
    }

    public void Initialize()
    {
    }

    public void PrepareForServices()
    {
    }

    public void Start()
    {
    }
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
      _windsor.Dispose();
    }
    #endregion

    private static LifestyleType Convert(Machine.Container.Model.LifestyleType lifestyleType)
    {
      switch (lifestyleType)
      {
        case Machine.Container.Model.LifestyleType.Singleton:
          return LifestyleType.Singleton;
        case Machine.Container.Model.LifestyleType.Transient:
          return LifestyleType.Transient;
      }
      throw new ArgumentException("lifestyleType");
    }

    #region IMachineContainer Members
    public ContainerRegisterer Register
    {
      get { throw new NotImplementedException(); }
    }

    public ContainerResolver Resolve
    {
      get { throw new NotImplementedException(); }
    }
    #endregion
  }
}