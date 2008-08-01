using Machine.Core.Services;

namespace Machine.Utility.ThreadPool.Workers
{
  public class RunnableWorker : Worker
  {
    private readonly IWorkerRunnable _runnable;

    public RunnableWorker(IWorkerRunnable runnable)
    {
      _runnable = runnable;
    }

    public override void Run()
    {
      _runnable.Run(this);
    }
  }
}