using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Lifestyles
{
  public class SingletonLifestyle : TransientLifestyle
  {
    #region Member Data
    private object _instance;
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
      if (_instance == null)
      {
        return base.CanActivate(services);
      }
      return true;
    }

    public override object Activate(IResolutionServices services)
    {
      using (this.Entry.Lock.AcquireReaderLock())
      {
        if (_instance == null)
        {
          this.Entry.Lock.UpgradeToWriterLock();
          if (_instance == null)
          {
            _instance = base.Activate(services);
          }
        }
        return _instance;
      }
    }

    public override void Release(IResolutionServices services, object instance)
    {
      if (_instance == null)
      {
        throw new YouFoundABugException("You managed to re-release something?");
      }
      _instance = null;
    }
    #endregion
  }
}
