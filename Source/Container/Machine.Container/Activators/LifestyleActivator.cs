using System;
using System.Collections.Generic;

using Machine.Container.Services;

namespace Machine.Container.Activators
{
  [Obsolete]
  public class LifestyleActivator : IActivator
  {
    #region Member Data
    private readonly ILifestyle _lifestyle;
    #endregion

    #region LifestyleActivator()
    public LifestyleActivator(ILifestyle lifestyle)
    {
      _lifestyle = lifestyle;
    }
    #endregion

    #region IActivator Members
    public bool CanActivate(IContainerServices services)
    {
      return _lifestyle.CanActivate(services);
    }

    public object Activate(IContainerServices services)
    {
      return _lifestyle.Activate(services);
    }

    public void Release(IContainerServices services, object instance)
    {
      _lifestyle.Release(services, instance);
    }
    #endregion
  }
}
