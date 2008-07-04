using System;

namespace Machine.Core.Utility
{
  public interface IReaderWriterLock
  {
    string Name
    {
      get;
    }
    void AcquireReaderLock(Int32 timeout);
    void AcquireWriterLock(Int32 timeout);
    void UpgradeToWriterLock(Int32 timeout);
    void AcquireReaderLock();
    void AcquireWriterLock();
    void UpgradeToWriterLock();
    bool UpgradeToWriterIf(RwLockGuardCondition condition);
    void ReleaseLock();
  }
}