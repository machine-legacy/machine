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
    void OnRegistration(ServiceEntry entry);
    void Started();
    void OnActivation(ResolvedServiceEntry entry, Activation activation);
    void OnDeactivation(ResolvedServiceEntry entry, Deactivation deactivation);
  }
}
