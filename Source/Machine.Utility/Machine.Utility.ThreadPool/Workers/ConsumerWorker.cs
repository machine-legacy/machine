using System;

using Machine.Core.Services;
using Machine.Core.Utility;

using Machine.Utility.ThreadPool.QueueStrategies;

namespace Machine.Utility.ThreadPool.Workers
{
  public interface IWorkerMonitor
  {
    bool CanBeShrunk { get; }
    void Unavailable();
    void Available();
    void Busy();
    void Free();
  }

  public class WorkerMonitor : IWorkerMonitor
  {
    readonly BusyWatcher _busyWatcher;
    readonly Worker _worker;
    private DateTime _lastUsedAt = DateTime.Now;

    public WorkerMonitor(BusyWatcher busyWatcher, Worker worker)
    {
      _worker = worker;
      _busyWatcher = busyWatcher;
    }
    
    public bool CanBeShrunk
    {
      get { return DateTime.Now - _lastUsedAt > TimeSpan.FromSeconds(5.0); }
    }

    public void Unavailable()
    {
      _busyWatcher.MarkAsUnavailable(_worker);
    }

    public void Available()
    {
      _busyWatcher.MarkAsAvailable(_worker);
    }

    public void Busy()
    {
      _busyWatcher.MarkAsBusy(_worker);
    }

    public void Free()
    {
      _busyWatcher.MarkAsFree(_worker);
      _lastUsedAt = DateTime.Now;
    }
  }

  public abstract class AbstractWorker : Worker
  {
    private readonly IWorkerMonitor _workerMonitor;

    protected AbstractWorker(BusyWatcher busyWatcher)
    {
      _workerMonitor = new WorkerMonitor(busyWatcher, this);
    }

    public override bool CanBeShrunk
    {
      get { return _workerMonitor.CanBeShrunk; }
    }

    public override void Start()
    {
      base.Start();
      _workerMonitor.Available();
    }

    public override void Join()
    {
      base.Join();
      _workerMonitor.Unavailable();
    }

    public virtual void Free()
    {
      _workerMonitor.Free();
    }

    public virtual void Busy()
    {
      _workerMonitor.Free();
    }

    public override void Run()
    {
      while (this.IsAlive)
      {
        try
        {
          WhileAlive();
        }
        catch (Exception error)
        {
          Error(error);
        }
      }
    }

    public virtual void Error(Exception error)
    {
    }

    public virtual void WhileAlive()
    {
    }
  }

  public class ConsumerWorker : AbstractWorker
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ConsumerWorker));
    private readonly IQueue _queue;
    private readonly QueueStrategy _queueStrategy;

    public ConsumerWorker(QueueStrategy queueStrategy, BusyWatcher busyWatcher)
      : base(busyWatcher)
    {
      _queueStrategy = queueStrategy;
      _queue = _queueStrategy.CreateQueueForWorker(this);
    }

    public override void Stop()
    {
      _queueStrategy.RetireQueue(_queue);
      base.Stop();
    }

    public override void WhileAlive()
    {
      using (IScope scope = _queue.CreateScope())
      {
        IRunnable runnable = scope.Dequeue();
        if (runnable != null)
        {
          using (new PerformanceWatcher(_log, "Processed"))
          {
            Busy();
            try
            {
              runnable.Run();
              scope.Complete();
            }
            finally
            {
              Free();
            }
          }
        }
      }
    }

    public override void Error(Exception error)
    {
      _log.Error(error);
    }
  }
}