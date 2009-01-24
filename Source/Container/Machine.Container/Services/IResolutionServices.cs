using System;

using Machine.Container.Model;

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

    IResolvableType CreateResolvableType(Type type);
    IResolvableType CreateResolvableType(ServiceDependency dependency);
  }
}
