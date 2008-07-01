using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Plugins;

namespace Machine.Container.Services
{
  public interface IMachineContainer : IDisposable
  {
    void AddPlugin(IServiceContainerPlugin plugin);
    void AddListener(IServiceContainerListener listener);

    void Initialize();
    void PrepareForServices();
    void Start();

    ContainerRegisterer Register { get; }
    ContainerResolver Resolver { get; }

    IEnumerable<ServiceRegistration> RegisteredServices { get; }

    void Release(object instance);
    bool HasService<T>();
  }
  public interface IHighLevelContainer : IMachineContainer
  {
    void AddService(Type serviceType, LifestyleType lifestyleType);
    void AddService<TService>();
    void AddService<TService>(Type implementation);
    void AddService<TService, TImpl>(LifestyleType lifestyleType);
    void AddService<TService, TImpl>();
    void AddService<TService>(LifestyleType lifestyleType);
    void AddService<TService>(object instance);
    void AddService(Type serviceType, object instance);
    T Resolve<T>();
    object Resolve(Type type);
    T ResolveWithOverrides<T>(params object[] serviceOverrides);
    T New<T>(params object[] serviceOverrides);
  }
}