using System;
using System.Threading;

namespace Machine.Core.Utility
{
  public class DotNetReaderWriterLock : IReaderWriterLock
  {
    private readonly ReaderWriterLock _lock;
    private readonly string _name;

    public DotNetReaderWriterLock()
     : this(new ReaderWriterLock(), String.Empty)
    {
    }

    public DotNetReaderWriterLock(ReaderWriterLock lok)
     : this(lok, String.Empty)
    {
    }

    public DotNetReaderWriterLock(ReaderWriterLock lok, string name)
    {
      _lock = lok;
      _name = name;
    }

    #region IReaderWriterLock Members
    public string Name
    {
      get { return _name; }
    }

    public virtual void AcquireReaderLock(Int32 timeout)
    {
      _lock.AcquireReaderLock(timeout);
    }

    public void AcquireReaderLock()
    {
      AcquireReaderLock(Timeout.Infinite);
    }

    public virtual void AcquireWriterLock(Int32 timeout)
    {
      _lock.AcquireWriterLock(timeout);
    }

    public void AcquireWriterLock()
    {
      AcquireWriterLock(Timeout.Infinite);
    }

    public virtual void UpgradeToWriterLock(Int32 timeout)
    {
      _lock.UpgradeToWriterLock(timeout);
    }

    public void UpgradeToWriterLock()
    {
      UpgradeToWriterLock(Timeout.Infinite);
    }

    public virtual void ReleaseLock()
    {
      _lock.ReleaseLock();
    }

    public virtual bool UpgradeToWriterIf(RwLockGuardCondition condition)
    {
      return RWLock.UpgradeToWriterIf(this, condition);
    }
    #endregion
  }
}