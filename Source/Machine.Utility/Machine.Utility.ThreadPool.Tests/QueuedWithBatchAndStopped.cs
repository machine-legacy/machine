using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace Machine.Utility.ThreadPool
{
  [TestFixture]
  public class QueuedWithBatchAndStopped : ThreadPoolFixture
  {
    [Test]
    public void Queued_One_Item()
    {
      Message message = new Message();
      ThreadPool pool = new ThreadPool(ThreadPoolConfiguration.OneAndTwo);
      pool.Start();
      pool.Queue(new MessageConsumer(), message);
      pool.Stop();
      Assert.IsTrue(message.WasConsumed);
    }

    [Test]
    public void Queued_Twenty_Items()
    {
      List<Message> messages = MessageBuilder.TwentyMessages();
      ThreadPool pool = new ThreadPool(ThreadPoolConfiguration.FiveAndTen);
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
