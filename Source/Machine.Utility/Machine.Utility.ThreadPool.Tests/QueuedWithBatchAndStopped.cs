using System;
using System.Collections.Generic;

using Machine.Utility.ThreadPool.QueueStrategies;

using NUnit.Framework;

namespace Machine.Utility.ThreadPool
{
  [TestFixture]
  public class QueuedWithBatchAndStopped : ThreadPoolFixture
  {
    [Test]
    public void Queued_One_Item()
    {
      Message message = new Message("Jacob");
      ThreadPool pool = new ThreadPool(ThreadPoolConfiguration.OneAndTwo, new SingleQueueStrategy());
      pool.Start();
      pool.Queue(new MessageConsumer(), message);
      pool.Stop();
      Assert.IsTrue(message.WasConsumed);
    }

    [Test]
    public void Queued_Twenty_Items()
    {
      List<Message> messages = MessageBuilder.TwentyMessages();
      ThreadPool pool = new ThreadPool(ThreadPoolConfiguration.FiveAndTen, new SingleQueueStrategy());
      pool.Start();
      foreach (Message message in messages)
      {
        pool.Queue(new MessageConsumer(), message);
      }
      pool.Stop();
      foreach (Message message in messages)
      {
        Assert.IsTrue(message.WasConsumed);
      }
    }

    [Test]
    public void Queued_Twenty_Items_With_Affinity_Strategy()
    {
      List<Message> messages = MessageBuilder.TwentyMessages();
      ThreadPool pool = new ThreadPool(ThreadPoolConfiguration.FiveAndTen, new QueueAffinityStrategy<Message, string>());
      pool.Start();
      foreach (Message message in messages)
      {
        pool.Queue(new MessageConsumer(), message);
      }
      pool.Stop();
      foreach (Message message in messages)
      {
        Assert.IsTrue(message.WasConsumed);
      }
    }
  }
}
