using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Plugins;

namespace Machine.Container
{
  public interface IResolutionOnlyContainer : IDisposable
  {
    ContainerResolver Resolve { get; }
    IEnumerable<ServiceRegistration> RegisteredServices { get; }
    void Deactivate(object instance);
    bool CanResolve<T>();
    bool CanResolve(Type type);
  }

  public interface IMachineContainer : IResolutionOnlyContainer
  {
    void AddPlugin(IServiceContainerPlugin plugin);
    void AddListener(IServiceContainerListener listener);

    void Initialize();
    void PrepareForServices();
    void Start();

    ContainerRegisterer Register { get; }
  }
}