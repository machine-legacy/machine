using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;
using Machine.Core;

namespace Machine.Container.Lifestyles
{
  public class PerThreadLifestyle : TransientLifestyle
  {
    [ThreadStatic]
    private static Activation _activation;

    public PerThreadLifestyle(IActivatorFactory activatorFactory, ServiceEntry serviceEntry)
      : base(activatorFactory, serviceEntry)
    {
    }

    public override bool CanActivate(IResolutionServices services)
    {
      if (_activation == null)
      {
        return base.CanActivate(services);
      }
      return true;
    }

    public override Activation Activate(IResolutionServices services)
    {
      if (_activation == null)
      {
        Activation firstActivation = base.Activate(services);
        _activation = new Activation(firstActivation);
        return firstActivation;
      }
      return _activation;
    }

    public override void Deactivate(IResolutionServices services, object instance)
    {
      if (_activation == null)
      {
        throw new YouFoundABugException("You managed to re-release something?");
      }
      _activation = null;
      base.Deactivate(services, instance);
    }
  }
}