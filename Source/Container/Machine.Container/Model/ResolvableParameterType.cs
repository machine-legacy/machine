using System;

using Machine.Container.Services;

namespace Machine.Container.Model
{
  public class ResolvableParameterType : ResolvableType
  {
    private readonly ServiceDependency _dependency;

    public ResolvableParameterType(IServiceGraph serviceGraph, IServiceEntryFactory serviceEntryFactory, ServiceDependency dependency)
      : base(serviceGraph, serviceEntryFactory, dependency.DependencyType)
    {
      _dependency = dependency;
    }

    public override ServiceEntry ToServiceEntry(IResolutionServices services)
    {
      ServiceEntry entry = _serviceGraph.Lookup(_type, services.Flags);
      if (entry != null)
      {
        return entry;
      }
      return _serviceEntryFactory.CreateServiceEntry(_dependency);
    }
  }
}