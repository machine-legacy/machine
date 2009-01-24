using System;
using System.Collections.Generic;

using Machine.Container.Services;

namespace Machine.Container.Model
{
  public class ResolvableType : IResolvableType
  {
    protected readonly IServiceGraph _serviceGraph;
    protected readonly IServiceEntryFactory _serviceEntryFactory;
    protected readonly Type _type;

    public ResolvableType(IServiceGraph serviceGraph, IServiceEntryFactory serviceEntryFactory, Type type)
    {
      _serviceGraph = serviceGraph;
      _serviceEntryFactory = serviceEntryFactory;
      _type = type;
    }

    public virtual ServiceEntry ToServiceEntry(IResolutionServices services)
    {
      ServiceEntry entry = _serviceGraph.Lookup(_type, services.Flags);
      if (entry != null)
      {
        return entry;
      }
      return _serviceEntryFactory.CreateServiceEntry(_type, _type, LifestyleType.Override);
    }
  }
}
