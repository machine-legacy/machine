using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services
{
  public interface IServiceEntryResolver
  {
    ServiceEntry CreateEntryIfMissing(Type type);
    ResolvedServiceEntry ResolveEntry(IResolutionServices services, IResolvableType resolvableType);
  }
}