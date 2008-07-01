using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services
{
  public interface IActivatorResolver
  {
    IActivator ResolveActivator(IResolutionServices services, ServiceEntry serviceEntry);
  }
}
