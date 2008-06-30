using System;
using System.Collections.Generic;

namespace Machine.Container.Services
{
  public interface IActivator
  {
    bool CanActivate(IContainerServices services);
    object Activate(IContainerServices services);
    void Release(IContainerServices services, object instance);
  }
}