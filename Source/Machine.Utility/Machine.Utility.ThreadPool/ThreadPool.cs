using System;
using Machine.Utility.ThreadPool.QueueStrategies;
using Machine.Utility.ThreadPool.Workers;

namespace Machine.Utility.ThreadPool
{
  public class ThreadPool : AbstractThreadPool
  {
    private readonly QueueStrategy _queueStrategy;

    public ThreadPool(ThreadPoolConfiguration configuration, QueueStrategy queueStrategy)
      : base(configuration)
    {
      _queueStrategy = queueStrategy;
    }

    public override void AddConsumerThread()
    {
      AddAndStartWorker(new ConsumerWorker(_queueStrategy, this.BusyWatcher), true);
    }

    public void Queue<TType>(IConsumer<TType> consumer, TType value)
    {
      if (this.IsStopped) throw new InvalidOperationException("Already been stopped");
      _queueStrategy.Queue(new ConsumingRunnable<TType>(consumer, value));
    }

    protected override void StopAllWorkers()
    {
      _queueStrategy.DrainstopAllQueues();
      base.StopAllWorkers();
    }
  }
}
