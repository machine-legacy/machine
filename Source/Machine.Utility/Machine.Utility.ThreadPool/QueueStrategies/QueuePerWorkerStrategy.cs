using System;
using System.Collections.Generic;

using Machine.Core.Services;
using Machine.Utility.ThreadPool.Workers;

namespace Machine.Utility.ThreadPool.QueueStrategies
{
  public class QueuePerWorkerStrategy : QueueStrategy
  {
    private readonly List<QueueOfRunnables> _queues = new List<QueueOfRunnables>();
    private readonly object _lock = new object();
    private int _index;

    public override QueueOfRunnables CreateQueueForWorker(Worker worker)
    {
      lock (_lock)
      {
        QueueOfRunnables queue = new QueueOfRunnables();
        _queues.Add(queue);
        return queue;
      }
    }

    public override void RetireQueue(QueueOfRunnables queue)
    {
      lock (_lock)
      {
        _queues.Remove(queue);
      }
      queue.Drainstop();
    }

    public override void DrainstopAllQueues()
    {
      lock (_lock)
      {
        foreach (QueueOfRunnables queue in _queues)
        {
          queue.Drainstop();
        }
      }
    }

    public override void Queue(IRunnable runnable)
    {
      lock (_lock)
      {
        SelectQueue(_queues, runnable).Enqueue(runnable);
      }
    }

    protected virtual QueueOfRunnables SelectQueue(List<QueueOfRunnables> queues, IRunnable runnable)
    {
      _index = (++_index % queues.Count);
      return queues[_index];
    }
  }
}
