using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Machine.Container.Model
{
  public class ConstructorCandidate
  {
    private readonly List<ServiceDependency> _dependencies = new List<ServiceDependency>();
    private readonly ConstructorInfo _runtimeInfo;

    public ReadOnlyCollection<ServiceDependency> Dependencies
    {
      get { return new ReadOnlyCollection<ServiceDependency>(_dependencies); }
    }

    public ConstructorInfo RuntimeInfo
    {
      get { return _runtimeInfo; }
    }

    public ConstructorCandidate(ConstructorInfo runtimeInfo)
    {
      _runtimeInfo = runtimeInfo;
    }

    public void AddParameterDependency(ServiceDependency dependency)
    {
      _dependencies.Add(dependency);
    }
  }
  public class ResolvedConstructorCandidate
  {
    private readonly ConstructorCandidate _candidate;
    private readonly IList<ResolvedServiceEntry> _resolvedDependencies;

    public ConstructorCandidate Candidate
    {
      get { return _candidate; }
    }

    public IList<ResolvedServiceEntry> ResolvedDependencies
    {
      get { return _resolvedDependencies; }
    }

    public ResolvedConstructorCandidate(ConstructorCandidate candidate, IList<ResolvedServiceEntry> resolvedDependencies)
    {
      _candidate = candidate;
      _resolvedDependencies = resolvedDependencies;
    }
  }
}