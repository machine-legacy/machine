using Machine.Core.Services;

namespace Machine.Utility.ThreadPool
{
  public class ConsumingRunnable<TType> : IRunnable
  {
    private readonly IConsumer<TType> _consumer;
    private readonly TType _value;

    public virtual TType Value
    {
      get { return _value; }
    }

    public IConsumer<TType> Consumer
    {
      get { return _consumer; }
    }

    public ConsumingRunnable(IConsumer<TType> consumer, TType value)
    {
      _consumer = consumer;
      _value = value;
    }

    public void Run()
    {
      _consumer.Consume(_value);
    }
  }
}