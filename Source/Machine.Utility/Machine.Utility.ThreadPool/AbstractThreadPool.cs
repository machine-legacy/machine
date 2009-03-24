using System;
using System.Collections.Generic;

using Machine.Core.Services;
using Machine.Core.Services.Impl;
using Machine.Core.Utility;
using Machine.Utility.ThreadPool.Workers;

namespace Machine.Utility.ThreadPool
{
  public abstract class AbstractThreadPool : IDisposable
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ThreadPool));
    private readonly IThreadManager _threadManager;
    private readonly List<Worker> _workers = new List<Worker>();
    private readonly BusyWatcher _busyWatcher;
    private readonly object _lock = new object();
    private ThreadPoolConfiguration _configuration;
    private bool _started;
    private bool _stopped;

    protected bool IsStopped
    {
      get { return _stopped; }
    }

    public BusyWatcher BusyWatcher
    {
      get { return _busyWatcher; }
    }

    protected AbstractThreadPool(ThreadPoolConfiguration configuration)
    {
      _configuration = configuration;
      _threadManager = new ThreadManager();
      _busyWatcher = new BusyWatcher();
    }

    public void ChangeConfiguration(ThreadPoolConfiguration configuration)
    {
      lock (_lock)
      {
        _configuration = configuration;
      }
    }

    public void Start()
    {
      if (_started) throw new InvalidOperationException("Already been started");
      _started = true;
      for (int i = 0; i < _configuration.MinimumNumberOfThreads; ++i)
      {
        AddConsumerThread();
      }
      AddWorkerRunnable(new Watchdog(_threadManager, this));
    }

    public void Stop()
    {
      Stop(false);
    }

    private void Stop(bool dispose)
    {
      if (_stopped)
      {
        if (dispose)
        {
          return;
        }
        throw new InvalidOperationException("Already been stopped");
      }
      _stopped = true;
      lock (_lock)
      {
        StopAllWorkers();
      }
      CleanupAllWorkers();
    }

    public abstract void AddConsumerThread();

    public void AddWorkerRunnable(IWorkerRunnable runnable)
    {
      AddAndStartWorker(new RunnableWorker(runnable), false);
    }

    public void AdjustPoolSizeIfNecessary()
    {
      if (_stopped)
      {
        return;
      }
      GrowIfNecessary();
      ShrinkOfPossible();
    }

    public void Dispose()
    {
      Stop(true);
    }

    protected void AddAndStartWorker(Worker worker, bool ignoreIfMaximumReached)
    {
      lock (_lock)
      {
        if (ignoreIfMaximumReached && _busyWatcher.NumberOfWorkers == _configuration.MaximumNumberOfThreads)
        {
          return;
        }
        _log.Info("Added new worker");
        worker.Initialize(_threadManager.CreateThread(worker));
        _workers.Add(worker);
        worker.Start();
      }
    }

    protected virtual void StopAllWorkers()
    {
      foreach (Worker worker in _workers)
      {
        worker.Stop();
      }
    }

    protected virtual void CleanupAllWorkers()
    {
      while (_workers.Count > 0)
      {
        lock (_lock)
        {
          foreach (Worker worker in Enumerate.AndChange(_workers))
          {
            if (!worker.IsRunning)
            {
              JoinAndRemove(worker);
            }
          }
        }
      }
    }

    protected virtual void JoinAndRemove(Worker worker)
    {
      _workers.Remove(worker);
      worker.Stop();
      worker.Join();
    }

    protected virtual void GrowIfNecessary()
    {
      if (_busyWatcher.NumberOfFreeWorkers == 0)
      {
        if (CanGrow)
        {
          _log.Info("Growing...");
          AddConsumerThread();
        }
      }
    }

    protected virtual void ShrinkOfPossible()
    {
      if (!CanShrink)
      {
        return;
      }
      lock (_lock)
      {
        foreach (Worker worker in Enumerate.AndChange(_workers))
        {
          if (worker.CanBeShrunk && CanShrink)
          {
            _log.Info("Shrinking...");
            JoinAndRemove(worker);
          }
        }
      }
    }

    protected bool CanGrow
    {
      get { return _busyWatcher.NumberOfWorkers < _configuration.MaximumNumberOfThreads; }
    }

    protected bool CanShrink
    {
      get { return _busyWatcher.NumberOfWorkers > _configuration.MinimumNumberOfThreads; }
    }
  }
}