using System;
using System.Collections.Generic;

using Machine.Core.Services;

namespace Machine.Utility.ThreadPool
{
  public interface IQueue
  {
    void Enqueue(IRunnable runnable);
    IRunnable Dequeue();
    void Drainstop();
  }
}
