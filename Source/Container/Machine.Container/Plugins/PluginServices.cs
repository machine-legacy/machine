using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Plugins
{
  public class PluginServices
  {
    private readonly ContainerStatePolicy _statePolicy;
    private readonly IMachineContainer _container;
    private readonly IRootActivatorResolver _resolver;
    private readonly IRootActivatorFactory _factory;

    public ContainerStatePolicy StatePolicy
    {
      get { return _statePolicy; }
    }

    public IMachineContainer Container
    {
      get { return _container; }
    }

    public IRootActivatorResolver Resolver
    {
      get { return _resolver; }
    }

    public IRootActivatorFactory Factory
    {
      get { return _factory; }
    }

    public PluginServices(ContainerStatePolicy statePolicy, IMachineContainer container, IRootActivatorResolver resolver, IRootActivatorFactory factory)
    {
      _statePolicy = statePolicy;
      _factory = factory;
      _container = container;
      _resolver = resolver;
    }
  }
}