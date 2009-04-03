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
    ContainerResolver Resolve { get; }

    IEnumerable<ServiceRegistration> RegisteredServices { get; }

    void Deactivate(object instance);
    bool CanResolve<T>();
    bool CanResolve(Type type);
  }

  public interface IHighLevelContainer : IMachineContainer
  {
    void Add(Type serviceType, LifestyleType lifestyleType);
    void Add<TImpl>();
    void Add<TImpl>(LifestyleType lifestyleType);
    void Add<TImpl>(object instance);
    void Add(Type serviceType, object instance);
    T ResolveObject<T>();
    object ResolveObject(Type type);
    T ResolveWithOverrides<T>(params object[] serviceOverrides);
    T New<T>(params object[] serviceOverrides);
  }
}