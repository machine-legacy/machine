using System;
using System.Diagnostics;

using System.Threading;
using Machine.Core.Utility;

namespace Machine.Container.Model
{
  public class ServiceEntryLock : IDisposable
  {
    #if DEBUGGING_LOCKS
    private readonly IReaderWriterLock _lock;

    public ServiceEntryLock(string name)
    {
      _lock = ReaderWriterLockFactory.CreateLock("SEL-" + name);
    }
    #else
    private readonly ReaderWriterLock _lock;

    public ServiceEntryLock(string name)
    {
      _lock = new ReaderWriterLock();
    }
    #endif

    public ServiceEntryLock AcquireReaderLock()
    {
      _lock.AcquireReaderLock(Timeout.Infinite);
      return this;
    }

    public ServiceEntryLock AcquireWriterLock()
    {
      _lock.AcquireWriterLock(Timeout.Infinite);
      return this;
    }

    public ServiceEntryLock UpgradeToWriterLock()
    {
      _lock.UpgradeToWriterLock(Timeout.Infinite);
      return this;
    }

    public void Dispose()
    {
      _lock.ReleaseLock();
    }
  }
}