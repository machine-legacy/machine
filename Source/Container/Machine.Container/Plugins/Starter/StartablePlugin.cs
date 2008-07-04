using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Plugins.Starter
{
  public class StartablePlugin : AbstractServiceContainerListener, IServiceContainerPlugin
  {
    private readonly List<ServiceEntry> _startables = new List<ServiceEntry>();

    public override void ServiceRegistered(ServiceEntry entry)
    {
      Type startable = typeof(IStartable);
      if (startable.IsAssignableFrom(entry.ImplementationType))
      {
        _startables.Add(entry);
      }
    }

    public override void InstanceCreated(ResolvedServiceEntry entry, Activation activation)
    {
    }

    public override void Started()
    {
      foreach (ServiceEntry entry in _startables)
      {
        IStartable startable = (IStartable)this.Container.Resolve.Object(entry.ImplementationType);
        startable.Start();
      }
    }
  }
  public interface IStartable
  {
    void Start();
  }
}
