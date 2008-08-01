using System;
using System.Collections.Generic;

using Machine.Utility.ThreadPool.Workers;

namespace Machine.Utility.ThreadPool
{
  public interface IWorkerRunnable
  {
    void Run(Worker worker);
  }
}
