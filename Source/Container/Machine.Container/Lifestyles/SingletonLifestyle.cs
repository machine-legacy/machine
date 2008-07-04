using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Lifestyles
{
  public class SingletonLifestyle : TransientLifestyle
  {
    #region Member Data
    private Activation _firstActivation;
    private Activation _cachedActivation;
    #endregion

    #region SingletonLifestyle()
    public SingletonLifestyle(IActivatorStrategy activatorStrategy, ServiceEntry entry)
     : base(activatorStrategy, entry)
    {
    }
    #endregion

    #region ILifestyle Members
    public override bool CanActivate(IResolutionServices services)
    {
      if (_firstActivation == null)
      {
        return base.CanActivate(services);
      }
      return true;
    }

    public override Activation Activate(IResolutionServices services)
    {
      using (this.Entry.Lock.AcquireReaderLock())
      {
        if (_firstActivation == null)
        {
          this.Entry.Lock.UpgradeToWriterLock();
          if (_firstActivation == null)
          {
            _firstActivation = base.Activate(services);
            _cachedActivation = new Activation(_firstActivation);
            return _firstActivation;
          }
        }
        return _cachedActivation;
      }
    }

    public override void Release(IResolutionServices services, object instance)
    {
      if (_firstActivation == null)
      {
        throw new YouFoundABugException("You managed to re-release something?");
      }
      _firstActivation = null;
      _cachedActivation = null;
    }
    #endregion
  }
}
