using System;
using System.Collections.Generic;
using System.Threading;

using Machine.Core.Services;
using Machine.Core.Utility;
using Machine.Utility.ThreadPool.Workers;

namespace Machine.Utility.ThreadPool.QueueStrategies
{
  public class QueuePerWorkerStrategy : QueueStrategy
  {
    private readonly List<QueueOfRunnables> _queues = new List<QueueOfRunnables>();
    private readonly ReaderWriterLock _lock = new ReaderWriterLock();
    private int _index;

    public override QueueOfRunnables CreateQueueForWorker(Worker worker)
    {
      using (RWLock.AsWriter(_lock))
      {
        QueueOfRunnables queue = new QueueOfRunnables();
        _queues.Add(queue);
        return queue;
      }
    }

    public override void RetireQueue(QueueOfRunnables queue)
    {
      using (RWLock.AsWriter(_lock))
      {
        _queues.Remove(queue);
        RetireQueueUnderLock(queue);
      }
      queue.Drainstop();
    }

    public override void DrainstopAllQueues()
    {
      using (RWLock.AsReader(_lock))
      {
        foreach (QueueOfRunnables queue in _queues)
        {
          queue.Drainstop();
        }
      }
    }

    public override void Queue(IRunnable runnable)
    {
      using (RWLock.AsReader(_lock))
      {
        SelectQueue(_lock, _queues, runnable).Enqueue(runnable);
      }
    }

    protected virtual QueueOfRunnables SelectQueue(ReaderWriterLock lok, List<QueueOfRunnables> queues, IRunnable runnable)
    {
      _index = (++_index % queues.Count);
      return queues[_index];
    }

    protected virtual void RetireQueueUnderLock(QueueOfRunnables queue)
    {
    }
  }
}
