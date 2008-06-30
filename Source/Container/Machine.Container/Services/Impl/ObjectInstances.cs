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
    private readonly ReaderWriterLock _lock = new ReaderWriterLock();
    private readonly Dictionary<object, ResolvedServiceEntry> _map = new Dictionary<object, ResolvedServiceEntry>();
    private readonly IListenerInvoker _listenerInvoker;

    public ObjectInstances(IListenerInvoker listenerInvoker)
    {
      _listenerInvoker = listenerInvoker;
    }

    public void Remember(ResolvedServiceEntry entry, object instance)
    {
      using (RWLock.AsWriter(_lock))
      {
        if (_map.ContainsKey(instance) && !_map[instance].Equals(entry))
        {
          throw new InvalidOperationException("Already have instance!");
        }
        _map[instance] = entry;
        _listenerInvoker.InstanceCreated(entry, instance);
      }
    }

    public void Release(ICreationServices services, object instance)
    {
      using (RWLock.AsWriter(_lock))
      {
        if (!_map.ContainsKey(instance))
        {
          throw new ServiceContainerException("Cannot release instances not created by the container!");
        }
        _listenerInvoker.InstanceReleased(_map[instance], instance);
        _map.Remove(instance);
      }
    }
  }
}