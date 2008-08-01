using System;
using System.Collections.Generic;

using Machine.Core.Services;

namespace Machine.Utility.ThreadPool.Workers
{
  public abstract class Worker : IRunnable
  {
    private IThread _thread;
    private bool _alive;

    public virtual bool CanBeShrunk
    {
      get { return false; }
    }

    public bool IsRunning
    {
      get { return _thread.IsRunning; }
    }

    public bool IsAlive
    {
      get { return _alive; }
    }

    public IThread Thread
    {
      get { return _thread; }
    }

    public virtual void Start()
    {
      _alive = true;
      _thread.Start();
    }

    public virtual void Stop()
    {
      _alive = false;
    }

    public virtual void Join()
    {
      _thread.Join();
    }

    public virtual void Initialize(IThread thread)
    {
      _thread = thread;
    }

    public abstract void Run();
  }
}