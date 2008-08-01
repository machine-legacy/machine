using System;
using System.Collections.Generic;
using System.Threading;

using NUnit.Framework;

namespace Machine.Utility.ThreadPool
{
  [TestFixture]
  public class QueuedPeriodicallyOverTime : ThreadPoolFixture
  {
    [Test]
    public void Queued_Randomly_For_Twenty_Seconds()
    {
      Random random = new Random((Int32)DateTime.Now.Ticks);
      ThreadPool pool = new ThreadPool(ThreadPoolConfiguration.FiveAndTen);
      pool.Start();
      DateTime startedAt = DateTime.Now;
      while (DateTime.Now - startedAt < TimeSpan.FromSeconds(20.0))
      {
        foreach (Message message in MessageBuilder.MakeMessages(random.Next(10)))
        {
          pool.Queue(new MessageConsumer(), message);
        }
        Thread.Sleep(TimeSpan.FromSeconds(random.NextDouble()));
      }
      pool.Stop();
    }
  }
}
