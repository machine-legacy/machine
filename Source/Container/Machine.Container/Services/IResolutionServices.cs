using System;

using Machine.Container.Plugins;

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
  }
}
