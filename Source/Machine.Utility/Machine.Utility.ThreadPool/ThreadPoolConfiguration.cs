using System;

namespace Machine.Utility.ThreadPool
{
  public class ThreadPoolConfiguration
  {
    public static ThreadPoolConfiguration FiveAndTwenty = new ThreadPoolConfiguration(5, 20);
    public static ThreadPoolConfiguration OneAndTwo = new ThreadPoolConfiguration(1, 2);
    public static ThreadPoolConfiguration OneAndOne = new ThreadPoolConfiguration(1, 1);
    public static ThreadPoolConfiguration FiveAndTen = new ThreadPoolConfiguration(5, 10);

    private int _minimumNumberOfThreads = 5;
    private int _maximumNumberOfThreads = 20;

    public int MinimumNumberOfThreads
    {
      get { return _minimumNumberOfThreads; }
      set { _minimumNumberOfThreads = value; }
    }

    public int MaximumNumberOfThreads
    {
      get { return _maximumNumberOfThreads; }
      set { _maximumNumberOfThreads = value; }
    }

    public ThreadPoolConfiguration()
    {
    }

    public ThreadPoolConfiguration(int minimumNumberOfThreads, int maximumNumberOfThreads)
    {
      _maximumNumberOfThreads = maximumNumberOfThreads;
      _minimumNumberOfThreads = minimumNumberOfThreads;
    }
  }
}