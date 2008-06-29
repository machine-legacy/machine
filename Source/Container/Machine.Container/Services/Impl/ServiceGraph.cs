using System;
using System.Collections.Generic;
using System.Threading;

using Machine.Container.Model;
using Machine.Core.Utility;

namespace Machine.Container.Services.Impl
{
  public class ServiceGraph : IServiceGraph
  {
    #region Logging
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ServiceGraph));
    #endregion

    #region Member Data
    private readonly Dictionary<Type, ServiceEntry> _map = new Dictionary<Type, ServiceEntry>();
    private readonly ReaderWriterLock _lock = new ReaderWriterLock();
    #endregion

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
      }
    }

    public IEnumerable<ServiceRegistration> RegisteredServices
    {
      get
      {
        using (RWLock.AsReader(_lock))
        {
          List<ServiceRegistration> registrations = new List<ServiceRegistration>();
          foreach (ServiceEntry serviceEntry in _map.Values)
          {
            registrations.Add(new ServiceRegistration(serviceEntry.ServiceType, serviceEntry.ImplementationType));
          }
          return registrations;
        }
      }
    }
    #endregion

    public ServiceEntry LookupLazily(Type type, bool throwIfAmbiguous)
    {
      using (RWLock.AsReader(_lock))
      {
        List<ServiceEntry> matches = new List<ServiceEntry>();
        foreach (ServiceEntry entry in _map.Values)
        {
          if (type.IsAssignableFrom(entry.ServiceType))
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