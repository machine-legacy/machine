using System;
using System.Collections.Generic;

using Machine.Container.Services;

namespace Machine.Container.Model
{
  public class NamedResolvableType : IResolvableType
  {
    private readonly IServiceGraph _serviceGraph;
    private readonly string _name;

    public NamedResolvableType(IServiceGraph serviceGraph, string name)
    {
      _serviceGraph = serviceGraph;
      _name = name;
    }

    public ServiceEntry ToServiceEntry(IResolutionServices services)
    {
      ServiceEntry entry = _serviceGraph.Lookup(_name);
      if (entry != null)
      {
        return entry;
      }
      throw new MissingServiceException(_name);
    }
  }
}
