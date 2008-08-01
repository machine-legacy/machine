using System;
using System.Collections.Generic;

using Machine.Core.Services;

namespace Machine.Utility.ThreadPool
{
  public class QueueOfRunnables : ThreadSafeQueue<IRunnable>
  {
  }
}
