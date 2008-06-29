using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Plugins
{
  public interface IServiceContainerListener : IDisposable
  {
    void Initialize(IHighLevelContainer container);
    void PreparedForServices();
    void ServiceRegistered(ServiceEntry entry);
    void Started();
    void InstanceCreated(ServiceEntry entry, object instance);
    void InstanceReleased(ServiceEntry entry, object instance);
  }
}
