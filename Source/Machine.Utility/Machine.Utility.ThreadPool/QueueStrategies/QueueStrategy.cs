using System;
using System.Collections.Generic;

using Machine.Core.Services;
using Machine.Utility.ThreadPool.Workers;

namespace Machine.Utility.ThreadPool.QueueStrategies
{
  public abstract class QueueStrategy
  {
    public abstract QueueOfRunnables CreateQueueForWorker(Worker worker);
    public abstract void Queue(IRunnable runnable);
    public abstract void RetireQueue(QueueOfRunnables queue);
    public abstract void DrainstopAllQueues();
  }
}
