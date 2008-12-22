using System;
using System.Collections.Generic;

using Machine.Core.Services;

namespace Machine.Utility.ThreadPool
{
  public class QueueOfRunnables : ThreadSafeQueue<IRunnable>, IQueue
  {
    public IScope CreateScope()
    {
      return new QueueOfRunnablesScope(this);
    }

    class QueueOfRunnablesScope : IScope
    {
      readonly QueueOfRunnables _queueOfRunnables;

      public QueueOfRunnablesScope(QueueOfRunnables queueOfRunnables)
      {
        _queueOfRunnables = queueOfRunnables;
      }

      public IRunnable Dequeue()
      {
        return _queueOfRunnables.Dequeue();
      }

      public void Complete()
      {
      }

      public void Dispose()
      {
      }
    }
  }
}
