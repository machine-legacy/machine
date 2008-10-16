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

    ResolvableType CreateResolvableType(Type type);
    ResolvableType CreateResolvableType(ServiceDependency dependency);
  }
}
