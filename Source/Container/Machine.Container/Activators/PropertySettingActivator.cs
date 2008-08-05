using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Activators
{
  public class PropertySettingActivator : IActivator
  {
    private readonly IActivator _target;
    private readonly ServiceEntry _entry;

    public PropertySettingActivator(IActivator target, ServiceEntry entry)
    {
      _target = target;
      _entry = entry;
    }

    #region IActivator Members
    public bool CanActivate(IResolutionServices services)
    {
      return _target.CanActivate(services);
    }

    public Activation Activate(IResolutionServices services)
    {
      Activation activation = _target.Activate(services);
      if (_entry.HasPropertySettings())
      {
        _entry.PropertySettings.Apply(activation.Instance);
      }
      return activation;
    }

    public void Deactivate(IResolutionServices services, object instance)
    {
      _target.Deactivate(services, instance);
    }
    #endregion
  }
}
