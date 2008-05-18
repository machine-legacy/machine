using System;
using System.Collections.Generic;
using Machine.Container.Services;

namespace Machine.Container.Plugins
{
  public interface IServiceContainerPlugin : IDisposable
  {
    void Initialize(IHighLevelContainer container);
  }
}
