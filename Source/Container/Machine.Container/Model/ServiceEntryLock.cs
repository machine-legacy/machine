using System;
using System.Diagnostics;

using Machine.Core.Utility;

namespace Machine.Container.Model
{
  public class ServiceEntryLock : IDisposable
  {
    private readonly IReaderWriterLock _lock;

    public ServiceEntryLock(string name)
    {
      _lock = ReaderWriterLockFactory.CreateLock("SEL-" + name);
    }

    public ServiceEntryLock AcquireReaderLock()
    {
      _lock.AcquireReaderLock();
      return this;
    }

    public ServiceEntryLock AcquireWriterLock()
    {
      _lock.AcquireWriterLock();
      return this;
    }

    public ServiceEntryLock UpgradeToWriterLock()
    {
      _lock.UpgradeToWriterLock();
      return this;
    }

    #region IDisposable Members
    public void Dispose()
    {
      _lock.ReleaseLock();
    }
    #endregion
  }
}