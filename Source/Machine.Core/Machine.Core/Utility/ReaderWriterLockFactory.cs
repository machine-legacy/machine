using System;
using System.Threading;

namespace Machine.Core.Utility
{
  public static class ReaderWriterLockFactory
  {
    public static IReaderWriterLock CreateLock(string name)
    {
      // return new DotNetReaderWriterLock();
      return new PerformanceMeasuringReaderWriterLock(new ReaderWriterLock(), name);
    }
  }
}