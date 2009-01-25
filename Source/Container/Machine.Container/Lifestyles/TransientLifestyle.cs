using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Lifestyles
{
  public class TransientLifestyle : ILifestyle
  {
    private readonly IActivatorFactory _activatorFactory;
    private readonly ServiceEntry _entry;
    private IActivator _defaultActivator;

    protected ServiceEntry Entry
    {
      get { return _entry; }
    }

    public TransientLifestyle(IActivatorFactory activatorFactory, ServiceEntry serviceEntry)
    {
      _activatorFactory = activatorFactory;
      _entry = serviceEntry;
    }

    public virtual void Initialize()
    {
      _defaultActivator = _activatorFactory.CreateDefaultActivator(_entry);
    }

    public virtual bool CanActivate(IResolutionServices services)
    {
      return _defaultActivator.CanActivate(services);
    }

    public virtual Activation Activate(IResolutionServices services)
    {
      return _defaultActivator.Activate(services);
    }

    public virtual void Deactivate(IResolutionServices services, object instance)
    {
      _defaultActivator.Deactivate(services, instance);
    }
  }
}
