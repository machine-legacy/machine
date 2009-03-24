using System;
using System.Collections.Generic;
using System.Threading;
using Machine.Utility.ThreadPool.QueueStrategies;
using NUnit.Framework;

namespace Machine.Utility.ThreadPool
{
  [TestFixture]
  public class QueuedPeriodicallyOverTime : ThreadPoolFixture
  {
    [Test]
    public void Queued_Randomly_For_Twenty_Seconds()
    {
      ThreadPool pool = new ThreadPool(ThreadPoolConfiguration.FiveAndTen, new SingleQueueStrategy());
      pool.Start();
      DateTime startedAt = DateTime.Now;
      while (DateTime.Now - startedAt < TimeSpan.FromSeconds(10.0))
      {
        foreach (Message message in MessageBuilder.MakeMessages(Random.Next(10)))
        {
          pool.Queue(new MessageConsumer(), message);
        }
        Thread.Sleep(TimeSpan.FromSeconds(Random.NextDouble()));
      }
      pool.Stop();
    }
  }
}
