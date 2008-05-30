using System;
using System.Collections.Generic;

using Machine.Container.Plugins;

namespace Machine.Container.Services
{
  public interface IPluginManager
  {
    void AddPlugin(IServiceContainerPlugin plugin);
    void Initialize();
    void Dispose();
  }
}