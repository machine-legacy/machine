using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Plugins
{
  public interface IServiceContainerListener
  {
    void ServiceRegistered(ServiceEntry entry);
    void InstanceCreated(ServiceEntry entry, object instance);
    void InstanceReleased(ServiceEntry entry, object instance);
  }
}
