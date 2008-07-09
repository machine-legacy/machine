using System;
using System.Collections.Generic;

using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace Machine.Core.Utility
{
  public static class LoggingHelper
  {
    public static void ConfigureConsoleLogging()
    {
      log4net.Appender.ConsoleAppender appender = new log4net.Appender.ConsoleAppender();
      appender.Layout = new log4net.Layout.PatternLayout("%-5p %c{1} %m%n");
      log4net.Config.BasicConfigurator.Configure(appender);
    }

    public static void Disable(string loggingArea)
    {
      Logger logger = (Logger)LogManager.GetLogger(loggingArea).Logger;
      logger.Level = Level.Off;
    }
  }
}
