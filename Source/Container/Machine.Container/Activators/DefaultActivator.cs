using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Activators
{
  public class DefaultActivator : IActivator
  {
    #region Logging
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(DefaultActivator));
    #endregion

    #region Member Data
    private readonly IObjectFactory _objectFactory;
    private readonly IServiceDependencyInspector _serviceDependencyInspector;
    private readonly IServiceEntryResolver _serviceEntryResolver;
    private readonly ServiceEntry _entry;
    private ResolvedConstructorCandidate _selectedCandidate;
    #endregion

    #region DefaultActivator()
    public DefaultActivator(IObjectFactory objectFactory, IServiceDependencyInspector serviceDependencyInspector, IServiceEntryResolver serviceEntryResolver, ServiceEntry entry)
    {
      _objectFactory = objectFactory;
      _serviceEntryResolver = serviceEntryResolver;
      _serviceDependencyInspector = serviceDependencyInspector;
      _entry = entry;
    }
    #endregion

    #region IActivator Members
    public bool CanActivate(IResolutionServices services)
    {
      using (services.DependencyGraphTracker.Push(_entry))
      {
        if (!TypeCanBeActivated())
        {
          return false;
        }
        /* I don't care if we do this multiple times as we race to get it down for now... */
        if (_selectedCandidate == null)
        {
          _selectedCandidate = ResolveConstructorCandidate(services);
        }
        return _selectedCandidate != null;
      }
    }

    public Activation Activate(IResolutionServices services)
    {
      if (_selectedCandidate == null)
      {
        throw new YouFoundABugException("How can you try and Activate something if it can't be activated: " + _entry);
      }
      object[] parameters = ResolveConstructorDependencies(services);
      object instance = _objectFactory.CreateObject(_selectedCandidate.Candidate, parameters);
      // services.ObjectInstances.Remember();
      return new Activation(_entry, instance, true);
    }

    public void Deactivate(IResolutionServices services, object instance)
    {
    }
    #endregion

    protected virtual ResolvedConstructorCandidate ResolveConstructorCandidate(IResolutionServices services)
    {
      List<ResolvedServiceEntry> resolved = new List<ResolvedServiceEntry>();
      ConstructorCandidate candidate = _serviceDependencyInspector.SelectConstructor(_entry.ConcreteType);
      foreach (ServiceDependency dependency in candidate.Dependencies)
      {
        ResolvedServiceEntry dependencyEntry = _serviceEntryResolver.ResolveEntry(services, dependency.DependencyType, true);
        if (dependencyEntry == null)
        {
          return null;
        }
        _log.Info("Dependency: " + dependencyEntry);
        resolved.Add(dependencyEntry);
      }
      return new ResolvedConstructorCandidate(candidate, resolved);
    }

    protected virtual bool TypeCanBeActivated()
    {
      return _entry.ConcreteType != null && !_entry.ConcreteType.IsPrimitive;
    }

    protected virtual object[] ResolveConstructorDependencies(IResolutionServices services)
    {
      List<object> parameters = new List<object>();
      foreach (ResolvedServiceEntry dependency in _selectedCandidate.ResolvedDependencies)
      {
        Activation activation = dependency.Activate(services);
        activation.AssertIsFullyActivated();
        parameters.Add(activation.Instance);
      }
      return parameters.ToArray();
    }
  }
}