using System;

namespace Machine.Container.Services
{
  public interface IResolutionServices : IInternalServices
  {
    IOverrideLookup Overrides
    {
      get;
    }

    DependencyGraphTracker DependencyGraphTracker
    {
      get;
    }

    LookupFlags Flags
    {
      get;
    }
  }
}
