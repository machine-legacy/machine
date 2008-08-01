using System;
using System.Collections.Generic;
using System.Threading;

namespace Machine.Utility.ThreadPool
{
  public class ThreadSafeQueue<TType> where TType : class
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger("ThreadSafeQueue<" + typeof(TType).Name + ">");
    private readonly TimeSpan _dequeueWait;
    private readonly Queue<TType> _queue = new Queue<TType>();
    private int _counter;
    private bool _closed;

    public bool IsEmpty
    {
      get { return this.NumberOfItems == 0; }
    }

    public int NumberOfItems
    {
      get { return _counter; }
    }

    public ThreadSafeQueue(TimeSpan dequeueWait)
    {
      _dequeueWait = dequeueWait;
    }

    public ThreadSafeQueue()
      : this(TimeSpan.FromSeconds(1.0))
    {
    }

    public void Enqueue(TType value)
    {
      if (value == null)
      {
        throw new ArgumentNullException("value");
      }
      if (_closed)
      {
        throw new InvalidOperationException("Queue is closed");
      }
      lock (_queue)
      {
        _queue.Enqueue(value);
        _counter++;
        Monitor.Pulse(_queue);
      }
    }

    public TType Dequeue()
    {
      lock (_queue)
      {
        if (_counter <= 0)
        {
          if (!Monitor.Wait(_queue, _dequeueWait))
          {
            return default(TType);
          }
          /* This should be unnecessary */
          if (_counter <= 0)
          {
            return default(TType);
          }
        }
        _counter--;
        return _queue.Dequeue();
      }
    }

    public void Drainstop()
    {
      _closed = true;
      while (true)
      {
        lock (_queue)
        {
          if (_counter <= 0)
          {
            return;
          }
          _log.Info("Drainstopping with " + _counter + " remaining");
          Monitor.Wait(_queue, _dequeueWait);
        }
      }
    }
  }
}