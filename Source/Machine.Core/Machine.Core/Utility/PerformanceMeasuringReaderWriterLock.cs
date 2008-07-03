using System;
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
      ReaderWriterUsage usage = PerThreadUsages.GetUsage(this, true);
      usage.Start(true);
      base.AcquireReaderLock(timeout);
      usage.Acquired();
    }

    public override void AcquireWriterLock(Int32 timeout)
    {
      ReaderWriterUsage usage = PerThreadUsages.GetUsage(this, true);
      usage.Start(false);
      base.AcquireWriterLock(timeout);
      usage.Acquired();
    }

    public override void UpgradeToWriterLock(Int32 timeout)
    {
      ReaderWriterUsage usage = PerThreadUsages.GetUsage(this, false);
      usage.BeforeUpgrade();
      base.UpgradeToWriterLock(timeout);
      usage.Upgrade();
    }

    public override void ReleaseLock()
    {
      ReaderWriterUsage usage = PerThreadUsages.GetUsage(this, false);
      usage.Release();
      base.ReleaseLock();
      ReaderWriterLockStatistics.Singleton.AddUsage(usage);
    }
  }
}