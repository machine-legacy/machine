using System;
using System.Collections.Generic;

using Machine.Container.Plugins;
using Machine.Container.Services;
using Machine.Container.Services.Impl;

namespace Machine.Container
{
  public class ContainerBuilder : IContainerBuilder
  {
    readonly CompartmentalizedMachineContainer _container;
    bool _prepared;

    public ContainerBuilder()
      : this(new DefaultContainerInfrastructureFactory())
    {
    }

    public ContainerBuilder(IContainerInfrastructureFactory containerInfrastructureFactory)
    {
      _container = new CompartmentalizedMachineContainer(containerInfrastructureFactory);
      _container.Initialize();
    }

    public void AddPlugin(IServiceContainerPlugin plugin)
    {
      _container.AddPlugin(plugin);
    }

    public void AddListener(IServiceContainerListener listener)
    {
      _container.AddListener(listener);
    }

    public ContainerRegisterer Register
    {
      get
      {
        if (!_prepared)
        {
          _container.PrepareForServices();
          _prepared = true;
        }
        return _container.Register;
      }
    }

    public IResolutionOnlyContainer CreateContainer()
    {
      _container.Start();
      return new MachineContainer(_container);
    }
  }

  public interface IContainerBuilder
  {
    IResolutionOnlyContainer CreateContainer();
  }
}
