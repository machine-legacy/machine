using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace Machine.Core.ValueTypes
{
  /*
  [TestFixture]
  public class ValueTypeHelperTests_Performance
  {
    [Test]
    public void Compare_Performance()
    {
      Stopwatch manual = new Stopwatch();
      Message3ManualComparison m1 = new Message3ManualComparison("M", 1);
      Message3ManualComparison m2 = new Message3ManualComparison("M", 1);

      const int iterations = 100000;

      manual.Start();
      for (int i = 0; i < iterations; ++i)
      {
        Object.Equals(m1, m2);
      }
      manual.Stop();

      Stopwatch automatic = new Stopwatch();
      Message3 a1 = new Message3("A", 1);
      Message3 a2 = new Message3("A", 1);

      automatic.Start();
      for (int i = 0; i < iterations; ++i)
      {
        ValueTypeHelper.AreEqual(a1, a2);
      }
      automatic.Stop();

      Console.WriteLine("Manual: " + manual.Elapsed.TotalMilliseconds / (double)iterations);
      Console.WriteLine("Automatic: " + automatic.Elapsed.TotalMilliseconds / (double)iterations);
    }
  }
  */
}
