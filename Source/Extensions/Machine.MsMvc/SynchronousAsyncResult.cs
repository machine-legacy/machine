using System;
using System.Threading;

namespace Machine.MsMvc
{
  public class SynchronousAsyncResult : IAsyncResult
  {
    public bool IsCompleted
    {
      get { return true; }
    }

    public WaitHandle AsyncWaitHandle
    {
      get { throw new NotSupportedException("Not expecting a call to AsyncWaitHandle..."); }
    }

    public object AsyncState
    {
      get { return null; }
    }

    public bool CompletedSynchronously
    {
      get { return true; }
    }
  }
}