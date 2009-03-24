using System;
using System.Threading;

using Machine.Core.Services;
using Machine.Utility.ThreadPool.Workers;

namespace Machine.Utility.ThreadPool
{
  public class Watchdog : IWorkerRunnable
  {
    private readonly IThreadManager _threadManager;
    private readonly AbstractThreadPool _pool;

    public Watchdog(IThreadManager threadManager, AbstractThreadPool pool)
    {
      _threadManager = threadManager;
      _pool = pool;
    }

    public void Run(Worker worker)
    {
      while (worker.IsAlive)
      {
        _pool.AdjustPoolSizeIfNecessary();
        _threadManager.Sleep(TimeSpan.FromSeconds(1.0));
      }
    }
  }
}