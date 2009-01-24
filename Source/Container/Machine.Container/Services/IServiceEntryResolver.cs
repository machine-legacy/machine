using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services
{
  public interface IServiceEntryResolver
  {
    ServiceEntry CreateEntryIfMissing(Type type);
    ServiceEntry CreateEntryIfMissing(Type serviceType, Type implementationType);
    ServiceEntry LookupEntry(Type type);
    ResolvedServiceEntry ResolveEntry(IResolutionServices services, IResolvableType resolvableType);
  }
}