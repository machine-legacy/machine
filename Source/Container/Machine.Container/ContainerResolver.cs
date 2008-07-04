using System;
using System.Collections.Generic;
using System.Threading;

using Machine.Container.Model;
using Machine.Container.Services;
using Machine.Core.Utility;

namespace Machine.Container
{
  public class ContainerResolver
  {
    private readonly IContainerServices _containerServices;
    private readonly ContainerRegisterer _containerRegisterer;

    public ContainerResolver(IContainerServices containerServices, ContainerRegisterer containerRegisterer)
    {
      _containerServices = containerServices;
      _containerRegisterer = containerRegisterer;
    }

    public object Object(Type type)
    {
      return Resolve(type);
    }

    public object Object(Type type, params object[] overrides)
    {
      return Resolve(type, overrides);
    }

    public T Object<T>()
    {
      return (T)Object(typeof(T));
    }

    public T Object<T>(params object[] overrides)
    {
      return (T)Object(typeof(T), overrides);
    }

    public T New<T>(params object[] overrides)
    {
      _containerRegisterer.Type<T>().AsTransient();
      return Object<T>(overrides);
    }

    public IList<T> All<T>()
    {
      List<T> found = new List<T>();
      foreach (ServiceRegistration registration in _containerServices.ServiceGraph.RegisteredServices)
      {
        if (typeof(T).IsAssignableFrom(registration.ImplementationType))
        {
          found.Add((T)Object(registration.ImplementationType));
        }
      }
      return found;
    }

    protected virtual object Resolve(Type type, params object[] overrides)
    {
      IResolutionServices services = _containerServices.CreateResolutionServices(overrides);
      ResolvedServiceEntry entry = _containerServices.ServiceEntryResolver.ResolveEntry(services, type, true);
      Activation activation = entry.Activate(services);
      activation.AssertIsFullyActivated();
      return activation.Instance;
    }
  }
}