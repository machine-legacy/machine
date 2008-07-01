using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Activators
{
  public class StaticActivator : IActivator
  {
    #region Member Data
    private readonly ServiceEntry _entry;
    private readonly object _instance;
    #endregion

    #region StaticActivator()
    public StaticActivator(ServiceEntry entry, object instance)
    {
      _entry = entry;
      _instance = instance;
    }
    #endregion

    #region IActivator Members
    public bool CanActivate(IResolutionServices services)
    {
      return true;
    }

    public object Activate(IResolutionServices services)
    {
      return _instance;
    }

    public void Release(IResolutionServices services, object instance)
    {
    }
    #endregion
  }
}