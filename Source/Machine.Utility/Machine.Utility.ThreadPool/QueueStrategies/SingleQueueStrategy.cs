using System;
using System.Collections.Generic;

using Machine.Core.Services;
using Machine.Utility.ThreadPool.Workers;

namespace Machine.Utility.ThreadPool.QueueStrategies
{
  public class SingleQueueStrategy : QueueStrategy
  {
    private readonly QueueOfRunnables _queue = new QueueOfRunnables();

    public override QueueOfRunnables CreateQueueForWorker(Worker worker)
    {
      return _queue;
    }

    public override void Queue(IRunnable runnable)
    {
      _queue.Enqueue(runnable);
    }

    public override void RetireQueue(QueueOfRunnables queue)
    {
    }

    public override void DrainstopAllQueues()
    {
      _queue.Drainstop();
    }
  }
}