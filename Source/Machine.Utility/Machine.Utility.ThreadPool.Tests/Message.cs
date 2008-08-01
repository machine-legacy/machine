using System;
using System.Collections.Generic;
using System.Threading;

namespace Machine.Utility.ThreadPool
{
  public class Message
  {
    public bool WasConsumed;
  }
  public class MessageConsumer : IConsumer<Message>
  {
    public int NumberProcessed;

    #region IConsumer<Message> Members
    public void Consume(Message message)
    {
      Thread.Sleep(TimeSpan.FromSeconds(1.0));
      message.WasConsumed = true;
      Interlocked.Increment(ref NumberProcessed);
    }
    #endregion
  }
  public class MessageBuilder
  {
    public static List<Message> TwentyMessages()
    {
      List<Message> messages = new List<Message>();
      messages.AddRange(MakeMessages(20));
      return messages;
    }

    public static IEnumerable<Message> MakeMessages(int number)
    {
      for (int i = 0; i < number; ++i)
      {
        yield return new Message();
      }
    }
  }
}
