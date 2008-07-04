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
    #region Logging
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ServiceGraph));
    #endregion

    #region Member Data
    private readonly IListenerInvoker _listenerInvoker;
    private readonly IDictionary<Type, ServiceEntry> _map = new Dictionary<Type, ServiceEntry>();
    private readonly IReaderWriterLock _lock = ReaderWriterLockFactory.CreateLock("ServiceGraph");
    private readonly List<Type> _registrationOrder = new List<Type>();
    #endregion

    public ServiceGraph(IListenerInvoker listenerInvoker)
    {
      _listenerInvoker = listenerInvoker;
    }

    #region IServiceGraph Members
    public ServiceEntry Lookup(Type type, bool throwIfAmbiguous)
    {
      return LookupLazily(type, throwIfAmbiguous);
    }

    public ServiceEntry Lookup(Type type)
    {
      return LookupLazily(type, true);
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
    #endregion

    protected virtual ServiceEntry LookupLazily(Type type, bool throwIfAmbiguous)
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
        if (matches.Count == 1)
        {
          return matches[0];
        }
        else if (matches.Count > 1 && throwIfAmbiguous)
        {
          throw new AmbiguousServicesException(type.ToString());
        }
        return null;
      }
    }
  }
}