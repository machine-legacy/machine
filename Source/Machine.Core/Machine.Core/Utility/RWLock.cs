using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Machine.Core.Utility
{
  public static class RWLock
  {
    public static IDisposable AsWriter(ReaderWriterLock theLock)
    {
      theLock.AcquireWriterLock(Timeout.Infinite);
      return new RWLockWrapper(new DotNetReaderWriterLock(theLock));
    }

    public static IDisposable AsReader(ReaderWriterLock theLock)
    {
      theLock.AcquireReaderLock(Timeout.Infinite);
      return new RWLockWrapper(new DotNetReaderWriterLock(theLock));
    }

    public static IDisposable AsWriter(IReaderWriterLock theLock)
    {
      theLock.AcquireWriterLock(Timeout.Infinite);
      return new RWLockWrapper(theLock);
    }

    public static IDisposable AsReader(IReaderWriterLock theLock)
    {
      theLock.AcquireReaderLock(Timeout.Infinite);
      return new RWLockWrapper(theLock);
    }

    public static bool UpgradeToWriterIf(ReaderWriterLock rwLock, GuardCondition condition)
    {
      if (condition())
      {
        rwLock.UpgradeToWriterLock(Timeout.Infinite);
        if (condition())
        {
          return true;
        }
      }
      return false;
    }
  }

  public class RWLockWrapper : IDisposable
  {
    private readonly IReaderWriterLock _readerWriterLock;

    public RWLockWrapper(IReaderWriterLock readerWriterLock)
    {
      _readerWriterLock = readerWriterLock;
    }

    public void Dispose()
    {
      _readerWriterLock.ReleaseLock();
    }
  }

  public delegate bool GuardCondition();

  public class BetterReaderWriterLock
  {
    private readonly ReaderWriterLock _lock;

    public BetterReaderWriterLock()
    {
      _lock = new ReaderWriterLock();
    }

    public void AcquireReaderLock()
    {
      _lock.AcquireReaderLock(Timeout.Infinite);
    }

    public void AcquireWriterLock()
    {
      _lock.AcquireWriterLock(Timeout.Infinite);
    }

    public void UpgradeToWriterLock()
    {
      _lock.UpgradeToWriterLock(Timeout.Infinite);
    }

    public bool UpgradeToWriterIf(GuardCondition condition)
    {
      return RWLock.UpgradeToWriterIf(_lock, condition);
    }

    public void ReleaseLock()
    {
      _lock.ReleaseLock();
    }
  }

  public static class PerThreadUsages
  {
    [ThreadStatic]
    private static Dictionary<IReaderWriterLock, ReaderWriterUsage> _usage;

    public static ReaderWriterUsage GetUsage(IReaderWriterLock lok, bool createNew)
    {
      if (_usage == null)
      {
        _usage = new Dictionary<IReaderWriterLock, ReaderWriterUsage>();
      }
      if (!_usage.ContainsKey(lok) || createNew)
      {
        _usage[lok] = new ReaderWriterUsage(lok);
      }
      return _usage[lok];
    }
  }
}