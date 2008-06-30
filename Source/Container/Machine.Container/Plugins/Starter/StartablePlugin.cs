using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Plugins.Starter
{
  public class StartablePlugin : AbstractServiceContainerListener, IServiceContainerPlugin
  {
    public override void ServiceRegistered(ServiceEntry entry)
    {
      base.ServiceRegistered(entry);
    }

    public override void InstanceCreated(ResolvedServiceEntry entry, object instance)
    {
      base.InstanceCreated(entry, instance);
    }

    public override void Started()
    {
      base.Started();
    }
  }
  public interface IStartable
  {
    void Start();
  }
}
