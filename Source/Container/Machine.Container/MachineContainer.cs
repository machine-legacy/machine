using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Plugins;
using Machine.Container.Services;

namespace Machine.Container
{
  public class MachineContainer : IMachineContainer
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

    public void Reset()
    {
      _container.Reset();
    }

    public bool CanResolve<T>()
    {
      return CanResolve(typeof(T));
    }

    public bool CanResolve(Type type)
    {
      return _container.CanResolve(type);
    }

    public void Dispose()
    {
      _container.Dispose();
    }
  }
}
