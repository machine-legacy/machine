using System;
using System.Collections.Generic;

using Machine.Container.Services;

namespace Machine.Container.Plugins
{
  public class PluginServices
  {
    private readonly IMachineContainer _container;
    private readonly IRootActivatorResolver _resolver;

    public IMachineContainer Container
    {
      get { return _container; }
    }

    public IRootActivatorResolver Resolver
    {
      get { return _resolver; }
    }

    public PluginServices(IMachineContainer container, IRootActivatorResolver resolver)
    {
      _container = container;
      _resolver = resolver;
    }
  }
}