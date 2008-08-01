using System;
using System.Collections.Generic;

namespace Machine.Utility.ThreadPool
{
  public static class TestFrameworkSettings
  {
    public static TimeSpan TimeToConsumeMessage = TimeSpan.FromSeconds(0.1);
    public static Random Random = new Random();
  }
}
