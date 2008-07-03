using System;
using System.Diagnostics;

namespace Machine.Core.Utility
{
  public class ReaderWriterUsage
  {
    private readonly IReaderWriterLock _lock;
    private readonly Stopwatch _timer = new Stopwatch();
    private bool _initiallyAReader;
    private bool _wasUpgraded;
    private long _timeAcquired;
    private long _timeStartedUpgrade;
    private long _timeUpgraded;
    private long _timeReleased;

    public IReaderWriterLock Lock
    {
      get { return _lock; }
    }

    public bool InitiallyAReader
    {
      get { return _initiallyAReader; }
    }

    public bool WasUpgraded
    {
      get { return _wasUpgraded; }
    }

    public long TimeUpgraded
    {
      get { return _timeUpgraded; }
    }

    public long TimeReleased
    {
      get { return _timeReleased; }
    }

    public ReaderWriterUsage(IReaderWriterLock lok)
    {
      _lock = lok;
    }

    public void Start(bool initiallyAReader)
    {
      _timer.Start();
      _initiallyAReader = initiallyAReader;
    }

    public void Acquired()
    {
      _timeAcquired = _timer.ElapsedTicks;
    }

    public void BeforeUpgrade()
    {
      _wasUpgraded = true;
      _timeStartedUpgrade = _timer.ElapsedTicks;
    }

    public void Upgrade()
    {
      _timeUpgraded = _timer.ElapsedTicks;
    }

    public void Release()
    {
      _timeReleased = _timer.ElapsedTicks;
    }

    public long TimeWaitingToAcqure
    {
      get { return this.TimeWaitingToRead + this.TimeWaitingToWrite; }
    }

    public long TimeSpentNotWaiting
    {
      get
      {
        return _timeReleased - this.TimeWaitingToAcqure;
      }
    }

    public long TimeSpentInLock
    {
      get { return _timeReleased; }
    }

    public long TimeAsWriter
    {
      get
      {
        if (_initiallyAReader)
        {
          if (_wasUpgraded)
          {
            return _timeReleased - _timeUpgraded;
          }
          return 0;
        }
        return _timeReleased - _timeAcquired;
      }
    }

    public long TimeAsReader
    {
      get
      {
        if (!_initiallyAReader)
        {
          return 0;
        }
        if (_wasUpgraded)
        {
          return _timeStartedUpgrade - _timeAcquired;
        }
        return _timeReleased - _timeAcquired;
      }
    }

    public long TimeWaitingToRead
    {
      get
      {
        if (_initiallyAReader)
        {
          return _timeAcquired;
        }
        return 0;
      }
    }

    public long TimeWaitingToWrite
    {
      get
      {
        if (_initiallyAReader)
        {
          if (_wasUpgraded)
          {
            return _timeUpgraded - _timeStartedUpgrade;
          }
          return 0;
        }
        return _timeAcquired;
      }
    }
  }
}