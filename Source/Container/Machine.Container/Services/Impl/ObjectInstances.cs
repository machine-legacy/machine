using System;
using System.Collections.Generic;
using System.Threading;

using Machine.Container.Model;
using Machine.Container.Plugins;
using Machine.Core.Utility;

namespace Machine.Container.Services.Impl
{
  public class ObjectInstances : IObjectInstances
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ObjectInstances));

    private readonly IReaderWriterLock _lock = ReaderWriterLockFactory.CreateLock("ObjectInstances");
    private readonly Dictionary<object, ResolvedServiceEntry> _map = new Dictionary<object, ResolvedServiceEntry>();
    private readonly IListenerInvoker _listenerInvoker;

    public ObjectInstances(IListenerInvoker listenerInvoker)
    {
      _listenerInvoker = listenerInvoker;
    }

    public void Remember(ResolvedServiceEntry entry, object instance)
    {
      using (RWLock.AsReader(_lock))
      {
        if (_map.ContainsKey(instance))
        {
          if (!_map[instance].Equals(entry))
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
            return;
          }
          _map[instance] = entry;
          _listenerInvoker.InstanceCreated(entry, instance);
          entry.IncrementActiveInstances();
          _log.Info("Remembering: " + entry + " - " + instance);
        }
      }
    }

    public void Release(IResolutionServices services, object instance)
    {
      using (RWLock.AsWriter(_lock))
      {
        _log.Info("Releasing: " + instance + "(" + instance.GetHashCode() + ")");
        if (!_map.ContainsKey(instance))
        {
          throw new ServiceContainerException("Attempt to release instances NOT created by the container: " + instance);
        }
        ResolvedServiceEntry resolvedEntry = _map[instance];
        _listenerInvoker.InstanceReleased(resolvedEntry, instance);
        resolvedEntry.DecrementActiveInstances();
        resolvedEntry.Release(services, instance );
        _map.Remove(instance);
      }
    }
  }
}