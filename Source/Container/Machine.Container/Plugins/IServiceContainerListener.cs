using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Plugins
{
  public interface IServiceContainerListener : IDisposable
  {
    void Initialize(IMachineContainer container);
    void PreparedForServices();
    void ServiceRegistered(ServiceEntry entry);
    void Started();
    void InstanceCreated(ResolvedServiceEntry entry, object instance);
    void InstanceReleased(ResolvedServiceEntry entry, object instance);
  }
}
