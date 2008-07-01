using System;
using System.Collections.Generic;

namespace Machine.Container.Services
{
  public interface IActivator
  {
    bool CanActivate(IResolutionServices services);
    object Activate(IResolutionServices services);
    void Release(IResolutionServices services, object instance);
  }
}