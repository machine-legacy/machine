using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace Machine.Utility.ThreadPool
{
  public class ThreadPoolFixture
  {
    [SetUp]
    public void Setup()
    {
      TestFrameworkLogging.SetupLogging();
    }
  }
}
