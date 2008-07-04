using System;
using System.Collections.Generic;
using System.Threading;

namespace Machine.Core.Utility
{
  public class PerformanceMeasuringReaderWriterLock : DotNetReaderWriterLock
  {
    public PerformanceMeasuringReaderWriterLock(ReaderWriterLock lok, string name)
      : base(lok, name)
    {
    }

    public override void AcquireReaderLock(Int32 timeout)
    {
      ReaderWriterUsage usage = PerThreadUsages.FindUsage(this, true);
      usage.Start(true);
      base.AcquireReaderLock(timeout);
      usage.Acquired();
    }

    public override void AcquireWriterLock(Int32 timeout)
    {
      ReaderWriterUsage usage = PerThreadUsages.FindUsage(this, true);
      usage.Start(false);
      base.AcquireWriterLock(timeout);
      usage.Acquired();
    }

    public override void UpgradeToWriterLock(Int32 timeout)
    {
      ReaderWriterUsage usage = PerThreadUsages.FindUsage(this, false);
      usage.BeforeUpgrade();
      base.UpgradeToWriterLock(timeout);
      usage.Upgrade();
    }

    public override void ReleaseLock()
    {
      ReaderWriterUsage usage = PerThreadUsages.FindUsage(this, false);
      usage.Release();
      base.ReleaseLock();
    }
  }

  public static class PerThreadUsages
  {
    [ThreadStatic]
    private static Dictionary<IReaderWriterLock, ReaderWriterUsage> _usage;
    [ThreadStatic]
    private static List<ReaderWriterUsage> _usages;

    public static ReaderWriterUsage FindUsage(IReaderWriterLock lok, bool createNew)
    {
      if (_usage == null)
      {
        _usage = new Dictionary<IReaderWriterLock, ReaderWriterUsage>();
        _usages = new List<ReaderWriterUsage>();
      }
      if (!_usage.ContainsKey(lok) || createNew)
      {
        _usage[lok] = new ReaderWriterUsage(lok);
        _usages.Add(_usage[lok]);
      }
      return _usage[lok];
    }

    public static void CopyThreadUsagesToMainCollection(ReaderWriterLockStatistics statistics)
    {
      statistics.AddUsages(_usages);
      _usages.Clear();
    }
  }
}