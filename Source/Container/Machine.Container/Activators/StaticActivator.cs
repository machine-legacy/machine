using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Activators
{
  public class StaticActivator : IActivator
  {
    #region Member Data
    private readonly Activation _activation;
    #endregion

    #region StaticActivator()
    public StaticActivator(ServiceEntry entry, object instance)
    {
      _activation = new Activation(entry, instance, false);
    }
    #endregion

    #region IActivator Members
    public bool CanActivate(IResolutionServices services)
    {
      return true;
    }

    public Activation Activate(IResolutionServices services)
    {
      return _activation;
    }

    public void Release(IResolutionServices services, object instance)
    {
      throw new ServiceContainerException("Releasing staticly registered instances is weird!");
    }
    #endregion
  }
}