using log4net.Appender;
using log4net.Layout;

namespace Machine.Utility.ThreadPool
{
  public static class TestFrameworkLogging
  {
    private static bool _loggingIsSetup;
    public static void SetupLogging()
    {
      if (_loggingIsSetup)
      {
        return;
      }
      _loggingIsSetup = true;
      DebugAppender appender = new DebugAppender();
      appender.Layout = new PatternLayout(@"%p %c %m%n");
      log4net.Config.BasicConfigurator.Configure(appender);
    }
  }
}