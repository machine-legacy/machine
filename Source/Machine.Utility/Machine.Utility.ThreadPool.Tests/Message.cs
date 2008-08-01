using System;
using System.Collections.Generic;
using System.Threading;

using Machine.Utility.ThreadPool.QueueStrategies;

namespace Machine.Utility.ThreadPool
{
  public class Message : IHasQueuingAffinity<string>
  {
    private bool _wasConsumed;
    private readonly string _name;

    public bool WasConsumed
    {
      get { return _wasConsumed; }
      set { _wasConsumed = value; }
    }

    public string QueueAffinityKey
    {
      get { return _name; }
    }

    public Message(string name)
    {
      _name = name;
    }
  }
  public class MessageConsumer : IConsumer<Message>
  {
    private long _numberOfMessagesProcessed;

    public long NumberOfMessagesProcessed
    {
      get { return _numberOfMessagesProcessed; }
    }

    #region IConsumer<Message> Members
    public void Consume(Message message)
    {
      Thread.Sleep(TestFrameworkSettings.TimeToConsumeMessage);
      message.WasConsumed = true;
      Interlocked.Increment(ref _numberOfMessagesProcessed);
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
      string[] names = new string[]
      {
        "Larry", "Curley", "Moe"
      };
      Random random = new Random();
      for (int i = 0; i < number; ++i)
      {
        yield return new Message(names[random.Next(names.Length)]);
      }
    }
  }
}
