using System;
using System.Collections.Generic;
using System.Threading;

using Machine.Core.Utility;

namespace Machine.Container.Model
{
  public interface IInstanceTrackingPolicy
  {
    TrackingStatus Remember(ResolvedServiceEntry entry, Activation activation);
    RememberedActivation RetrieveAndForget(object instance);
    IEnumerable<RememberedActivation> RetrieveAndForgetAll();
  }

  public enum TrackingStatus
  {
    New,
    Old
  }

  public class GlobalActivationScope : IInstanceTrackingPolicy
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(GlobalActivationScope));

    #if DEBUGGING_LOCKS
    private readonly IReaderWriterLock _lock = ReaderWriterLockFactory.CreateLock("ObjectInstances");
    #else
    private readonly ReaderWriterLock _lock = new ReaderWriterLock();
    #endif
    private readonly Dictionary<object, RememberedActivation> _map = new Dictionary<object, RememberedActivation>();

    public TrackingStatus Remember(ResolvedServiceEntry entry, Activation activation)
    {
      object instance = activation.Instance;
      using (RWLock.AsReader(_lock))
      {
        if (_map.ContainsKey(instance))
        {
          if (!_map[instance].ResolvedEntry.Equals(entry))
          {
            throw new InvalidOperationException("Already have instance for: " + entry);
          }
        }
        else
        {
          _lock.UpgradeToWriterLock(Timeout.Infinite);
          if (_map.ContainsKey(instance))
          {
            /* Not checking again is bad because multiple people can come back with the SAME instance,
             * that first time and then block here until they register them, so we call the listeners 
             * multiple times and increment more than we should. */
            return TrackingStatus.Old;
          }
          _map[instance] = new RememberedActivation(entry, activation);
          return TrackingStatus.New;
        }
      }
      return TrackingStatus.Old;
    }

    public RememberedActivation RetrieveAndForget(object instance)
    {
      using (RWLock.AsWriter(_lock))
      {
        _log.Info("Deactivating: " + instance + "(" + instance.GetHashCode() + ")");
        if (!_map.ContainsKey(instance))
        {
          throw new ServiceContainerException("Attempt to deactivate instance NOT created by the container: " + instance);
        }
        RememberedActivation entry = _map[instance];
        _map.Remove(instance);
        return entry;
      }
    }

    public IEnumerable<RememberedActivation> RetrieveAndForgetAll()
    {
      using (RWLock.AsWriter(_lock))
      {
        List<RememberedActivation> activations = new List<RememberedActivation>(_map.Values);
        _map.Clear();
        return activations;
      }
    }
  }

  public class PerThreadActivationScope : IInstanceTrackingPolicy
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(PerThreadActivationScope));

    [ThreadStatic]
    private static Dictionary<object, RememberedActivation> _map;

    public TrackingStatus Remember(ResolvedServiceEntry entry, Activation activation)
    {
      object instance = activation.Instance;
      if (_map == null)
      {
        _map = new Dictionary<object, RememberedActivation>();
      }
      if (_map.ContainsKey(instance))
      {
        if (!_map[instance].ResolvedEntry.Equals(entry))
        {
          throw new InvalidOperationException("Already have instance for: " + entry);
        }
      }
      else
      {
        _map[instance] = new RememberedActivation(entry, activation);
        return TrackingStatus.New;
      }
      return TrackingStatus.Old;
    }

    public RememberedActivation RetrieveAndForget(object instance)
    {
      if (_map == null)
      {
        throw new ServiceContainerException("You're trying to deactivate before getting anything on this thread: " + instance);
      }
      if (_map.ContainsKey(instance))
      {
        RememberedActivation entry = _map[instance];
        _map.Remove(instance);
        return entry;
      }
      throw new ServiceContainerException("Attempt to deactivate instance NOT created by the container OR by another thread: " + instance);
    }

    public IEnumerable<RememberedActivation> RetrieveAndForgetAll()
    {
      List<RememberedActivation> activations = new List<RememberedActivation>(_map.Values);
      _map.Clear();
      return activations;
    }
  }

  public class DoNotTrackInstances : IInstanceTrackingPolicy
  {
    #region IInstanceTrackingPolicy Members
    public TrackingStatus Remember(ResolvedServiceEntry entry, Activation activation)
    {
      return TrackingStatus.Old;
    }

    public RememberedActivation RetrieveAndForget(object instance)
    {
      return null;
    }

    public IEnumerable<RememberedActivation> RetrieveAndForgetAll()
    {
      return new RememberedActivation[0];
    }
    #endregion
  }
}
