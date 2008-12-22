using System;
using System.Collections.Generic;

using Machine.Core.Services;

namespace Machine.Utility.ThreadPool
{
  public interface IQueue
  {
    void Enqueue(IRunnable runnable);
    IScope CreateScope();
    void Drainstop();
  }
  public interface IScope : IDisposable
  {
    IRunnable Dequeue();
    void Complete();
  }
}
