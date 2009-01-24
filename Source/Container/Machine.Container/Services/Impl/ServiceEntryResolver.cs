using System;
using System.Threading;

using Machine.Container.Model;

namespace Machine.Container.Services.Impl
{
  public class ServiceEntryResolver : IServiceEntryResolver
  {
    #region Logging
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ServiceEntryResolver));
    #endregion

    #region Member Data
    private readonly IServiceGraph _serviceGraph;
    private readonly IServiceEntryFactory _serviceEntryFactory;
    private readonly IActivatorResolver _activatorResolver;
    #endregion

    #region ServiceEntryResolver()
    public ServiceEntryResolver(IServiceGraph serviceGraph, IServiceEntryFactory serviceEntryFactory, IActivatorResolver activatorResolver)
    {
      _serviceGraph = serviceGraph;
      _activatorResolver = activatorResolver;
      _serviceEntryFactory = serviceEntryFactory;
    }
    #endregion

    #region Methods
    public ServiceEntry CreateEntryIfMissing(Type type)
    {
      return CreateEntryIfMissing(type, type);
    }

    public ServiceEntry CreateEntryIfMissing(Type serviceType, Type implementationType)
    {
      ServiceEntry entry = _serviceGraph.Lookup(serviceType);
      if (entry == null)
      {
        // TODO Possible race condition here? -jlewalle
        entry = _serviceEntryFactory.CreateServiceEntry(serviceType, implementationType, LifestyleType.Singleton);
        _serviceGraph.Add(entry);
      }
      return entry;
    }

    public ServiceEntry LookupEntry(Type type)
    {
      return _serviceGraph.Lookup(type);
    }

    public ResolvedServiceEntry ResolveEntry(IResolutionServices services, IResolvableType resolvableType)
    {
      ServiceEntry entry = resolvableType.ToServiceEntry(services);
      using (entry.Lock.AcquireReaderLock())
      {
        /* Never want to auto create activator for the entry we created above. */
        if (entry.CanHaveActivator)
        {
          CreateActivatorForEntryIfNecessary(services, entry);
        }
      }
      IActivator activator = _activatorResolver.ResolveActivator(services, entry);
      if (activator == null)
      {
        return null;
      }
      return new ResolvedServiceEntry(entry, activator, services.ObjectInstances);
    }
    #endregion

    private static void CreateActivatorForEntryIfNecessary(IResolutionServices services, ServiceEntry entry)
    {
      if (!services.ActivatorStore.HasActivator(entry))
      {
        entry.Lock.UpgradeToWriterLock();
        if (services.ActivatorStore.HasActivator(entry))
        {
          return;
        }
        ILifestyle lifestyle = services.LifestyleFactory.CreateLifestyle(entry);
        services.ActivatorStore.AddActivator(entry, lifestyle);
      }
    }
  }
}