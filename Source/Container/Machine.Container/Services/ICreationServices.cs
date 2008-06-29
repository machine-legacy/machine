using Machine.Container.Activators;

namespace Machine.Container.Services
{
  public interface ICreationServices
  {
    IActivatorStore ActivatorStore
    {
      get;
    }

    IActivatorStrategy ActivatorStrategy
    {
      get;
    }

    ILifestyleFactory LifestyleFactory
    {
      get;
    }

    IOverrideLookup Overrides
    {
      get;
    }

    IServiceEntryResolver ServiceEntryResolver
    {
      get;
    }

    DependencyGraphTracker DependencyGraphTracker
    {
      get;
    }
  }
}
