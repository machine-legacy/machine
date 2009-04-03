using System;
using System.Collections.Generic;
using System.Threading;

using Machine.Container.Model;
using Machine.Container.Plugins;
using Machine.Core.Utility;

namespace Machine.Container.Services.Impl
{
  public class ServiceGraph : IServiceGraph
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ServiceGraph));

    private readonly IListenerInvoker _listenerInvoker;
    private readonly IDictionary<Type, ServiceEntry> _map = new Dictionary<Type, ServiceEntry>();
    #if DEBUGGING_LOCKS
    private readonly IReaderWriterLock _lock = ReaderWriterLockFactory.CreateLock("ServiceGraph");
    #else
    private readonly ReaderWriterLock _lock = new ReaderWriterLock();
    #endif
    private readonly List<Type> _registrationOrder = new List<Type>();
    private readonly Dictionary<string, ServiceEntry> _nameLookupCache = new Dictionary<string, ServiceEntry>();
    private readonly Memento<Type, ServiceEntry> _lazyLookups = new Memento<Type, ServiceEntry>();

    public ServiceGraph(IListenerInvoker listenerInvoker)
    {
      _listenerInvoker = listenerInvoker;
    }

    public ServiceEntry Lookup(Type type, LookupFlags flags)
    {
      return LookupLazily(type, flags);
    }

    public ServiceEntry Lookup(Type type)
    {
      return LookupLazily(type, LookupFlags.Default);
    }

    public ServiceEntry Lookup(string name)
    {
      List<ServiceEntry> matches = new List<ServiceEntry>();
      using (RWLock.AsReader(_lock))
      {
        if (RWLock.UpgradeToWriterIf(_lock, delegate() { return !_nameLookupCache.ContainsKey(name); }))
        {
          foreach (KeyValuePair<Type, ServiceEntry> pair in _map)
          {
            if (pair.Value.IsNamed(name))
            {
              matches.Add(pair.Value);
            }
          }
          _nameLookupCache[name] = Select(matches, LookupFlags.Default, name);
        }
        return _nameLookupCache[name];
      }
    }

    public void Add(ServiceEntry entry)
    {
      using (RWLock.AsWriter(_lock))
      {
        _log.Info("Adding: " + entry);
        _map[entry.ServiceType] = entry;
        _registrationOrder.Add(entry.ServiceType);
        _listenerInvoker.OnRegistration(entry);
      }
    }

    public IEnumerable<ServiceRegistration> RegisteredServices
    {
      get
      {
        using (RWLock.AsReader(_lock))
        {
          List<ServiceRegistration> registrations = new List<ServiceRegistration>();
          foreach (Type serviceType in _registrationOrder)
          {
            ServiceEntry serviceEntry = _map[serviceType];
            registrations.Add(new ServiceRegistration(serviceEntry.ServiceType, serviceEntry.ImplementationType));
          }
          return registrations;
        }
      }
    }

    protected virtual ServiceEntry LookupLazily(Type type, LookupFlags flags)
    {
      using (RWLock.AsReader(_lock))
      {
        List<ServiceEntry> matches = new List<ServiceEntry>();
        foreach (Type key in _registrationOrder)
        {
          ServiceEntry entry = _map[key];
          if (type.IsAssignableFrom(entry.ImplementationType))
          {
            matches.Add(entry);
          }
        }
        return Select(matches, flags, type.ToString());
      }
    }

    private static ServiceEntry Select(IList<ServiceEntry> matches, LookupFlags flags, string beingLookedUp)
    {
      if (matches.Count == 1)
      {
        return matches[0];
      }
      if (matches.Count > 1 && (flags & LookupFlags.ThrowIfAmbiguous) == LookupFlags.ThrowIfAmbiguous)
      {
        throw new AmbiguousServicesException(beingLookedUp);
      }
      return null;
    }
  }
}