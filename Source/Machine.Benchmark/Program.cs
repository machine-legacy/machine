using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Machine.Container;
using Machine.Container.Services;

namespace Machine.Benchmark
{
  public class Program
  {
    public static void Main(string[] args)
    {
      {
        PlainUseCase useCase = new PlainUseCase();
        useCase.Run(0);
        Console.WriteLine("Plain:            {0}", Measure(useCase.Run, 10000));
        GC.Collect();
      }

      {
        MachineUseCase useCase = new MachineUseCase();
        Console.WriteLine("Machine:          {0}", Measure(useCase.Run, 10000));
        GC.Collect();
      }

      {
        MachineUseCase useCase = new MachineUseCase();
        useCase.Run(0);
        Console.WriteLine("Machine (Primed): {0}", Measure(useCase.Run, 10000));
        GC.Collect();
      }

      Console.ReadKey();
    }

    private static long Measure(Action<Int32> action, int iterations)
    {
      GC.Collect();
      var watch = Stopwatch.StartNew();

      for (int i = 0; i < iterations; i++)
      {
        action(i);
      }

      return watch.ElapsedTicks;
    }
  }

  public abstract class UseCase
  {
    public abstract void Run(Int32 iteration);
  }

  public class MachineUseCase : UseCase
  {
    readonly IMachineContainer _container;

    public MachineUseCase()
    {
      _container = new MachineContainer();
      _container.Initialize();
      _container.PrepareForServices();
      _container.Register.Type<Logger>();
      _container.Register.Type<WebApp>();
      _container.Register.Type<Database>();
      _container.Register.Type<ErrorHandler>();
      _container.Register.Type<Authenticator>();
      _container.Register.Type<StockQuote>();
      _container.Start();
    }

    public override void Run(int iteration)
    {
      _container.Resolve.Object<IWebApp>();//.Run();
    }
  }

  public class PlainUseCase : UseCase
  {
    public override void Run(Int32 iteration)
    {
      var app = new WebApp(
        new Authenticator(
          new Logger(),
          new ErrorHandler(
            new Logger()
          ),
          new Database(
            new Logger(),
            new ErrorHandler(
              new Logger()
            )
          )
        ),
        new StockQuote(
          new Logger(),
          new ErrorHandler(
            new Logger()
          ),
          new Database(
            new Logger(),
            new ErrorHandler(
              new Logger()
            )
          )
        )
      );

      app.Run();
    }
  }

  public interface IStockQuote
  {
  }

  public class StockQuote : IStockQuote
  {
    readonly ILogger _logger;
    readonly IErrorHandler _errorHandler;
    readonly IDatabase _database;

    public StockQuote(ILogger logger, IErrorHandler errorHandler, IDatabase database)
    {
      _logger = logger;
      _errorHandler = errorHandler;
      _database = database;
    }
  }

  public interface IWebApp
  {
    void Run();
  }

  public class WebApp : IWebApp
  {
    readonly IAuthenticator _authenticator;
    readonly IStockQuote _stockQuote;

    public WebApp(IAuthenticator authenticator, IStockQuote stockQuote)
    {
      _authenticator = authenticator;
      _stockQuote = stockQuote;
    }

    public void Run()
    {
    }
  }

  public interface IDatabase
  {
  }

  public interface IAuthenticator
  {
  }

  public class Logger : ILogger
  {
  }

  public interface IErrorHandler
  {
  }

  public class ErrorHandler : IErrorHandler
  {
    readonly ILogger _logger;

    public ErrorHandler(ILogger logger)
    {
      _logger = logger;
    }
  }

  public interface ILogger
  {
  }

  public class Database : IDatabase
  {
    readonly ILogger _logger;
    readonly IErrorHandler _errorHandler;

    public Database(ILogger logger, IErrorHandler errorHandler)
    {
      _logger = logger;
      _errorHandler = errorHandler;
    }
  }

  public class Authenticator : IAuthenticator
  {
    readonly ILogger _logger;
    readonly IErrorHandler _handler;
    readonly IDatabase _database;

    public Authenticator(ILogger logger, IErrorHandler handler, IDatabase database)
    {
      _logger = logger;
      _handler = handler;
      _database = database;
    }
  }
}
