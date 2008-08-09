using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services
{
  public enum LookupFlags 
  {
    None = 0,
    ThrowIfAmbiguous = 1,
    ThrowIfUnable = 2,
    Default = ThrowIfAmbiguous | ThrowIfUnable
  }
  public interface IServiceGraph
  {
    ServiceEntry Lookup(Type type, LookupFlags flags);
    ServiceEntry Lookup(Type type);
    void Add(ServiceEntry entry);
    IEnumerable<ServiceRegistration> RegisteredServices { get; }
  }
}