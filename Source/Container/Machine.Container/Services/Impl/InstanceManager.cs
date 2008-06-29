using System;
using System.Collections.Generic;
using System.Threading;

using Machine.Container.Model;
using Machine.Core.Utility;

namespace Machine.Container.Services.Impl
{
  public class InstanceManager
  {
    private readonly IPluginManager _pluginManager;
    private readonly ReaderWriterLock _lock = new ReaderWriterLock();
    private readonly Dictionary<object, ResolvedServiceEntry> _map = new Dictionary<object, ResolvedServiceEntry>();

    public InstanceManager(IPluginManager pluginManager)
    {
      _pluginManager = pluginManager;
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
      }
    }

    public void Release(object instance)
    {
      using (RWLock.AsWriter(_lock))
      {
        _map[instance].Activator.Release(instance);
        _map.Remove(instance);
      }
    }

    public void Forget(object instance)
    {
      using (RWLock.AsWriter(_lock))
      {
        _map.Remove(instance);
      }
    }
  }
}