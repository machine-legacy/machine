using System;
using System.Collections.Generic;

using Machine.Utility.ThreadPool.Workers;

namespace Machine.Utility.ThreadPool
{
  public interface IWorkerFactory
  {
    Worker CreateWorker(BusyWatcher busyWatcher);
  }

  public class WorkerFactoryThreadPool : AbstractThreadPool
  {
    private readonly IWorkerFactory _workerFactory;

    public WorkerFactoryThreadPool(ThreadPoolConfiguration configuration, IWorkerFactory workerFactory) 
      : base(configuration)
    {
      _workerFactory = workerFactory;
    }

    public override void AddConsumerThread()
    {
      AddAndStartWorker(_workerFactory.CreateWorker(this.BusyWatcher), true);
    }
  }
}