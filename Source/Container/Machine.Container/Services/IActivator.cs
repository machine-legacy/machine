using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services
{
  public interface IActivator
  {
    bool CanActivate(IResolutionServices services);
    Activation Activate(IResolutionServices services);
    void Release(IResolutionServices services, object instance);
  }
}