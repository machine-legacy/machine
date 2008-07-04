using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Lifestyles
{
  public class TransientLifestyle : ILifestyle
  {
    #region Member Data
    private readonly IActivatorStrategy _activatorStrategy;
    private readonly ServiceEntry _entry;
    private IActivator _defaultActivator;
    #endregion

    protected ServiceEntry Entry
    {
      get { return _entry; }
    }

    #region TransientLifestyle()
    public TransientLifestyle(IActivatorStrategy activatorStrategy, ServiceEntry serviceEntry)
    {
      _activatorStrategy = activatorStrategy;
      _entry = serviceEntry;
    }
    #endregion

    #region ILifestyle Members
    public virtual void Initialize()
    {
      _defaultActivator = _activatorStrategy.CreateDefaultActivator(_entry);
    }

    public virtual bool CanActivate(IResolutionServices services)
    {
      return _defaultActivator.CanActivate(services);
    }

    public virtual Activation Activate(IResolutionServices services)
    {
      return _defaultActivator.Activate(services);
    }

    public virtual void Release(IResolutionServices services, object instance)
    {
      _defaultActivator.Release(services, instance);
    }
    #endregion
  }
}
