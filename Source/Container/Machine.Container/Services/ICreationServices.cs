using System;
using System.Collections.Generic;

using Machine.Container.Activators;
using Machine.Container.Model;

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
  public class DependencyGraphTracker : IDisposable
  {
    private readonly Stack<ServiceEntry> _progress = new Stack<ServiceEntry>();

    public string BuildProgressMessage(ServiceEntry entry)
    {
      return ResolutionMessageBuilder.BuildMessage(entry, _progress);
    }

    public IDisposable Push(ServiceEntry entry)
    {
      if (_progress.Contains(entry))
      {
        throw new CircularDependencyException(BuildProgressMessage(entry));
      }
      _progress.Push(entry);
      return this;
    }

    #region IDisposable Members
    public void Dispose()
    {
      _progress.Pop();
    }
    #endregion
  }
}
