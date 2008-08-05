using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services.Impl
{
  public class ParentContainerActivatorResolver : IActivatorResolver
  {
    #region IActivatorResolver Members
    public IActivator ResolveActivator(IResolutionServices services, ServiceEntry entry)
    {
      throw new NotImplementedException();
    }
    #endregion
  }
}
