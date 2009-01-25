using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Lifestyles
{
  public class HybridLifestyle : ILifestyle
  {
    private readonly AspDotNet _aspDotNet;
    private readonly WebRequestLifestyle _perWebRequestLifestyle;
    private readonly PerThreadLifestyle _perThreadLifestyle;

    public HybridLifestyle(IActivatorFactory activatorFactory, ServiceEntry entry)
    {
      _aspDotNet = new AspDotNet();
      _perWebRequestLifestyle = new WebRequestLifestyle(activatorFactory, entry);
      _perThreadLifestyle = new PerThreadLifestyle(activatorFactory, entry);
    }

    public bool CanActivate(IResolutionServices services)
    {
      return ChooseLifestyle().CanActivate(services);
    }

    public Activation Activate(IResolutionServices services)
    {
      return ChooseLifestyle().Activate(services);
    }

    public void Deactivate(IResolutionServices services, object instance)
    {
      ChooseLifestyle().Deactivate(services, instance);
    }

    public void Initialize()
    {
      _perWebRequestLifestyle.Initialize();
      _perThreadLifestyle.Initialize();
    }

    private ILifestyle ChooseLifestyle()
    {
      if (_aspDotNet.IsAvailable)
      {
        return _perWebRequestLifestyle;
      }
      return _perThreadLifestyle;
    }
  }
}