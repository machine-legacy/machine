using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Plugins.Starter
{
  public class StartablePlugin : AbstractServiceContainerListener, IServiceContainerPlugin
  {
    private readonly List<ServiceEntry> _startables = new List<ServiceEntry>();

    public virtual void Initialize(PluginServices services)
    {
    }

    public virtual void ReadyForServices(PluginServices services)
    {
    }

    public override void OnRegistration(ServiceEntry entry)
    {
      Type startable = typeof(IStartable);
      if (startable.IsAssignableFrom(entry.ServiceType))
      {
        _startables.Add(entry);
      }
    }

    public override void OnActivation(ResolvedServiceEntry entry, Activation activation)
    {
    }

    public override void Started()
    {
      foreach (ServiceEntry entry in _startables)
      {
        IStartable startable = (IStartable)this.Container.Resolve.Object(entry.ServiceType);
        startable.Start();
      }
    }
  }
}
