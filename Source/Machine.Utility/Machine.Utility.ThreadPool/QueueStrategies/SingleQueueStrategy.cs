using System;
using System.Collections.Generic;

using Machine.Core.Services;
using Machine.Utility.ThreadPool.Workers;

namespace Machine.Utility.ThreadPool.QueueStrategies
{
  public class SingleQueueStrategy : QueueStrategy
  {
    private readonly IQueue _queue;

    public SingleQueueStrategy()
      : this(new QueueOfRunnables())
    {
    }

    public SingleQueueStrategy(IQueue queue)
    {
      _queue = queue;
    }

    public override IQueue CreateQueueForWorker(Worker worker)
    {
      return _queue;
    }

    public override void Queue(IRunnable runnable)
    {
      _queue.Enqueue(runnable);
    }

    public override void RetireQueue(IQueue queue)
    {
    }

    public override void DrainstopAllQueues()
    {
      _queue.Drainstop();
    }
  }
}