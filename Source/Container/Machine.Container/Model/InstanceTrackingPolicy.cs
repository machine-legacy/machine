using System;
using System.Collections.Generic;

using Machine.Core.Utility;

namespace Machine.Container.Model
{
  public interface IInstanceTrackingPolicy
  {
    TrackingStatus Remember(ResolvedServiceEntry entry, Activation activation);
    ResolvedServiceEntry RetrieveAndForget(object instance);
  }

  public enum TrackingStatus
  {
    New,
    Old
  }

  public class GlobalActivationScope : IInstanceTrackingPolicy
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(GlobalActivationScope));

    private readonly IReaderWriterLock _lock = ReaderWriterLockFactory.CreateLock("ObjectInstances");
    private readonly Dictionary<object, RemberedActivation> _map = new Dictionary<object, RemberedActivation>();

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
          _lock.UpgradeToWriterLock();
          if (_map.ContainsKey(instance))
          {
            /* Not checking again is bad because multiple people can come back with the SAME instance,
             * that first time and then block here until they register them, so we call the listeners 
             * multiple times and increment more than we should. */
            return TrackingStatus.Old;
          }
          _map[instance] = new RemberedActivation(entry, activation);
          return TrackingStatus.New;
        }
      }
      return TrackingStatus.Old;
    }

    public ResolvedServiceEntry RetrieveAndForget(object instance)
    {
      using (RWLock.AsWriter(_lock))
      {
        _log.Info("Releasing: " + instance + "(" + instance.GetHashCode() + ")");
        if (!_map.ContainsKey(instance))
        {
          throw new ServiceContainerException("Attempt to release instances NOT created by the container: " + instance);
        }
        RemberedActivation entry = _map[instance];
        _map.Remove(instance);
        return entry.ResolvedEntry;
      }
    }
  }

  public class PerThreadActivationScope : IInstanceTrackingPolicy
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(PerThreadActivationScope));

    [ThreadStatic]
    private static Dictionary<object, RemberedActivation> _map;

    public TrackingStatus Remember(ResolvedServiceEntry entry, Activation activation)
    {
      object instance = activation.Instance;
      if (_map == null)
      {
        _map = new Dictionary<object, RemberedActivation>();
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
        _map[instance] = new RemberedActivation(entry, activation);
        return TrackingStatus.New;
      }
      return TrackingStatus.Old;
    }

    public ResolvedServiceEntry RetrieveAndForget(object instance)
    {
      if (_map == null)
      {
        throw new ServiceContainerException("You're trying to release before getting anything on this thread: " + instance);
      }
      if (_map.ContainsKey(instance))
      {
        RemberedActivation entry = _map[instance];
        _map.Remove(instance);
        return entry.ResolvedEntry;
      }
      throw new ServiceContainerException("Attempt to release instances NOT created by the container OR by another thread: " + instance);
    }
  }

  public class RemberedActivation
  {
    public ResolvedServiceEntry ResolvedEntry
    {
      get; private set;
    }

    public Activation Activation
    {
      get; private set;
    }

    public RemberedActivation(ResolvedServiceEntry resolvedEntry, Activation activation)
    {
      Activation = activation;
      ResolvedEntry = resolvedEntry;
    }

    public override bool Equals(object obj)
    {
      RemberedActivation other = obj as RemberedActivation;
      if (other != null)
      {
        return other.ResolvedEntry.Equals(this.ResolvedEntry) && other.Activation.Equals(this.Activation);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return this.ResolvedEntry.GetHashCode() ^ this.Activation.GetHashCode();
    }
  }
}
