using System;
using System.Collections.Generic;

using Machine.Container.Services;

namespace Machine.Container.Model
{
  public class ResolvableType : IResolvableType
  {
    private readonly IServiceGraph _serviceGraph;
    private readonly IServiceEntryFactory _serviceEntryFactory;
    private readonly Type _type;
    ServiceEntry _entry;

    public ResolvableType(IServiceGraph serviceGraph, IServiceEntryFactory serviceEntryFactory, Type type)
    {
      _serviceGraph = serviceGraph;
      _serviceEntryFactory = serviceEntryFactory;
      _type = type;
    }

    public ServiceEntry ToServiceEntry(IResolutionServices services)
    {
      if (_entry == null)
      {
        _entry = _serviceGraph.Lookup(_type, services.Flags);
        if (_entry != null)
        {
          return _entry;
        }
        return _serviceEntryFactory.CreateServiceEntry(_type, LifestyleType.Override);
      }
      return _entry;
    }
  }
}
