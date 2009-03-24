using System;

using Machine.Core.Services;
using Machine.Core.Utility;

using Machine.Utility.ThreadPool.QueueStrategies;

namespace Machine.Utility.ThreadPool.Workers
{
  public class ConsumerWorker : Worker
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ConsumerWorker));
    private readonly IQueue _queue;
    private readonly QueueStrategy _queueStrategy;
    private readonly BusyWatcher _busyWatcher;
    private DateTime _lastUsedAt = DateTime.Now;

    public ConsumerWorker(QueueStrategy queueStrategy, BusyWatcher busyWatcher)
    {
      _queueStrategy = queueStrategy;
      _busyWatcher = busyWatcher;
      _queue = _queueStrategy.CreateQueueForWorker(this);
    }

    public override bool CanBeShrunk
    {
      get { return DateTime.Now - _lastUsedAt > TimeSpan.FromSeconds(5.0); }
    }

    public override void Start()
    {
      base.Start();
      _busyWatcher.MarkAsAvailable(this);
    }

    public override void Stop()
    {
      _queueStrategy.RetireQueue(_queue);
      base.Stop();
    }

    public override void Join()
    {
      base.Join();
      _busyWatcher.MarkAsUnavailable(this);
    }

    public override void Run()
    {
      while (this.IsAlive)
      {
        try
        {
          using (IScope scope = _queue.CreateScope())
          {
            IRunnable runnable = scope.Dequeue();
            if (runnable != null)
            {
              using (new PerformanceWatcher(_log, "Processed"))
              {
                MarkAsBusy();
                try
                {
                  runnable.Run();
                  scope.Complete();
                }
                finally
                {
                  MarkAsFree();
                }
              }
            }
          }
        }
        catch (Exception error)
        {
          _log.Error(error);
        }
      }
    }

    protected void MarkAsBusy()
    {
      _busyWatcher.MarkAsBusy(this);
    }

    protected void MarkAsFree()
    {
      _busyWatcher.MarkAsFree(this);
      _lastUsedAt = DateTime.Now;
    }
  }
}