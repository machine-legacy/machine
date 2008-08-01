using System;
using System.Threading;

using Machine.Utility.ThreadPool.Workers;

namespace Machine.Utility.ThreadPool
{
  public class BusyWatcher
  {
    private int _total;
    private int _free;

    public int NumberOfWorkers
    {
      get { return Interlocked.Exchange(ref _total, _total); }
    }

    public int NumberOfFreeWorkers
    {
      get { return Interlocked.Exchange(ref _free, _free); }
    }

    public void MarkAsBusy(Worker worker)
    {
      Interlocked.Decrement(ref _free);
    }

    public void MarkAsAvailable(Worker worker)
    {
      Interlocked.Increment(ref _total);
      MarkAsFree(worker);
    }

    public void MarkAsUnavailable(Worker worker)
    {
      MarkAsBusy(worker);
      Interlocked.Decrement(ref _total);
    }

    public void MarkAsFree(Worker worker)
    {
      Interlocked.Increment(ref _free);
    }
  }
}