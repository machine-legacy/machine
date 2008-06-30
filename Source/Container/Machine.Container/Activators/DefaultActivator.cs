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
    public bool CanActivate(IContainerServices services)
    {
      using (services.DependencyGraphTracker.Push(_entry))
      {
        if (!TypeCanBeActivated())
        {
          return false;
        }
        _entry.ConstructorCandidate = CreateConstructorParameters(services);
        if (_entry.ConstructorCandidate == null)
        {
          return false;
        }
        return true;
      }
    }

    public object Activate(IContainerServices services)
    {
      if (_entry.ConstructorCandidate == null)
      {
        throw new YouFoundABugException();
      }
      object[] parameters = ResolveConstructorDependencies(services);
      return _objectFactory.CreateObject(_entry.ConstructorCandidate.Candidate, parameters);
    }

    public void Release(IContainerServices services, object instance)
    {
    }
    #endregion

    protected virtual ResolvedConstructorCandidate CreateConstructorParameters(IContainerServices services)
    {
      List<ResolvedServiceEntry> resolved = new List<ResolvedServiceEntry>();
      ConstructorCandidate candidate = _serviceDependencyInspector.SelectConstructor(_entry.ConcreteType);
      foreach (ServiceDependency dependency in candidate.Dependencies)
      {
        ResolvedServiceEntry dependencyEntry = _serviceEntryResolver.ResolveEntry(services, dependency.DependencyType, false);
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

    protected virtual object[] ResolveConstructorDependencies(IContainerServices services)
    {
      List<object> parameters = new List<object>();
      foreach (ResolvedServiceEntry dependency in _entry.ConstructorCandidate.ResolvedDependencies)
      {
        parameters.Add(dependency.Activate(services));
      }
      return parameters.ToArray();
    }
  }
}