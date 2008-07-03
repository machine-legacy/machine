using System;
using System.Collections.Generic;
using System.Threading;

namespace Machine.Core.Utility
{
  public class ReaderWriterLockStatistics
  {
    public static ReaderWriterLockStatistics Singleton = new ReaderWriterLockStatistics();

    private readonly ReaderWriterLock _lock = new ReaderWriterLock();
    private readonly List<ReaderWriterUsage> _usages = new List<ReaderWriterUsage>();

    public void AddUsage(ReaderWriterUsage usage)
    {
      using (RWLock.AsWriter(_lock))
      {
        _usages.Add(usage);
      }
    }

    public IDictionary<IReaderWriterLock, List<ReaderWriterUsage>> GroupByLock()
    {
      using (RWLock.AsReader(_lock))
      {
        Dictionary<IReaderWriterLock, List<ReaderWriterUsage>> byLock = new Dictionary<IReaderWriterLock, List<ReaderWriterUsage>>();
        foreach (ReaderWriterUsage usage in _usages)
        {
          if (!byLock.ContainsKey(usage.Lock))
          {
            byLock[usage.Lock] = new List<ReaderWriterUsage>();
          }
          byLock[usage.Lock].Add(usage);
        }
        return byLock;
      }
    }

    public static void OutputSummaryOfUsages(string name, IEnumerable<ReaderWriterUsage> usages)
    {
      long numberOfAcquires = 0;
      long numberOfReads = 0;
      long numberOfWrites = 0;
      long numberOfUpgrades = 0;
      long totalTicksInLock = 0;
      long totalTicksAsReader = 0;
      long totalTicksAsWriter = 0;
      long totalTicksWaiting = 0;
      long totalTicksWaitingToRead = 0;
      long totalTicksWaitingToWrite = 0;
      foreach (ReaderWriterUsage usage in usages)
      {
        if (usage.WasUpgraded)
        {
          numberOfUpgrades++;
        }
        if (usage.InitiallyAReader || usage.WasUpgraded)
        {
          numberOfReads++;
        }
        else
        {
          numberOfWrites++;
        }
        numberOfAcquires++;
        totalTicksAsWriter += usage.TimeAsWriter;
        totalTicksAsReader += usage.TimeAsReader;
        totalTicksWaiting += usage.TimeWaitingToAcqure;
        totalTicksWaitingToRead += usage.TimeWaitingToRead;
        totalTicksWaitingToWrite += usage.TimeWaitingToWrite;
        totalTicksInLock += usage.TimeSpentInLock;
      }

      TimeSpan totalTimeInLock = new TimeSpan(totalTicksInLock);
      TimeSpan totalTimeAsReader = new TimeSpan(totalTicksAsReader);
      TimeSpan totalTimeAsWriter= new TimeSpan(totalTicksAsWriter);
      TimeSpan totalTimeWaiting = new TimeSpan(totalTicksWaiting);
      TimeSpan totalTimeWaitingToRead = new TimeSpan(totalTicksWaitingToRead);
      TimeSpan totalTimeWaitingToWrite = new TimeSpan(totalTicksWaitingToWrite);

      Console.WriteLine("{0}", name);
      Console.WriteLine("Acquired: {1,5} Reads: {2,5} Writes: {3,5} Upgrades: {4,5} Waiting: {5,14} WaitingToRead: {6,14} WaitingToWrite: {7,14} AsWriter: {8,14} AsReader: {9,10} Total: {10,14}",
        String.Empty,
        numberOfAcquires,
        numberOfReads,
        numberOfWrites,
        numberOfUpgrades,
        totalTimeWaiting.TotalSeconds,
        totalTimeWaitingToRead.TotalSeconds,
        totalTimeWaitingToWrite.TotalSeconds,
        totalTimeAsWriter.TotalSeconds,
        totalTimeAsReader.TotalSeconds,
        totalTimeInLock.TotalSeconds);
    }
  }
}