using System;

using Machine.Container.Services;

namespace Machine.Container.Model
{
  public class ResolvableParameterType : IResolvableType
  {
    private readonly IServiceGraph _serviceGraph;
    private readonly IServiceEntryFactory _serviceEntryFactory;
    private readonly Type _type;
    private readonly ServiceDependency _dependency;

    public ResolvableParameterType(IServiceGraph serviceGraph, IServiceEntryFactory serviceEntryFactory, ServiceDependency dependency)
    {
      _serviceGraph = serviceGraph;
      _serviceEntryFactory = serviceEntryFactory;
      _type = dependency.DependencyType;
      _dependency = dependency;
    }

    public ServiceEntry ToServiceEntry(IResolutionServices services)
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