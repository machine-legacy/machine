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
}