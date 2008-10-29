using System;
using System.Threading;

namespace Machine.MsMvc
{
  public class AsynchronousAsyncResult : IAsyncResult
  {
    bool _isCompleted;
    bool _completeSync;
    object _state;
    AutoResetEvent _reset = new AutoResetEvent(false);

    public void SetIsCompleted(object state)
    {
      _state = state;
      _isCompleted = true;
      _reset.Set();
    }

    public void CompleteSync()
    {
      _isCompleted = true;
      _completeSync = true;
    }

    public bool IsCompleted
    {
      get { return _isCompleted; }
    }

    public WaitHandle AsyncWaitHandle
    {
      get { return _reset; }
    }

    public object AsyncState
    {
      get { return _state; }
    }

    public bool CompletedSynchronously
    {
      get { return _completeSync; }
    }
  }
}