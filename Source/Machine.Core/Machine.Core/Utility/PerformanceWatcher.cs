using System;
using System.Collections.Generic;
using System.Diagnostics;

using log4net;

namespace Machine.Core.Utility
{
  public class PerformanceWatcher : IDisposable
  {
    private static readonly ILog _primaryLog = LogManager.GetLogger(typeof(PerformanceWatcher));
    private readonly string _name;
    private readonly Stopwatch _stopwatch;
    private readonly ILog _log;

    public PerformanceWatcher(string name)
     : this(_primaryLog, name)
    {
    }

    public PerformanceWatcher(ILog log, string name)
    {
      _log = log;
      _name = name;
      _stopwatch = new Stopwatch();
      _stopwatch.Start();
    }

    public void Dispose()
    {
      _stopwatch.Stop();
      _log.Info(MakeMessage());
    }

    private string MakeMessage()
    {
      return String.Format("{0}: {1}", _name, _stopwatch.Elapsed.TotalSeconds);
    }
  }
}
