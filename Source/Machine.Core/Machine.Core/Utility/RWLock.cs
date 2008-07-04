using System;
using System.Collections.Generic;
using System.Threading;

namespace Machine.Core.Utility
{
  public delegate bool RwLockGuardCondition();

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

    public static bool UpgradeToWriterIf(ReaderWriterLock lok, RwLockGuardCondition condition)
    {
      return UpgradeToWriterIf(new DotNetReaderWriterLock(lok), condition);
    }

    public static bool UpgradeToWriterIf(IReaderWriterLock lok, RwLockGuardCondition condition)
    {
      if (condition())
      {
        lok.UpgradeToWriterLock();
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
}