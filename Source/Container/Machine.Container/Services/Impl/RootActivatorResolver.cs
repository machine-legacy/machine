using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services.Impl
{
  public class RootActivatorResolver : IRootActivatorResolver
  {
    private readonly List<IActivatorResolver> _resolvers = new List<IActivatorResolver>();

    #region IRootActivatorResolver Members
    public void AddFirst(IActivatorResolver resolver)
    {
      _resolvers.Insert(0, resolver);
    }

    public void AddAfter(Type type, IActivatorResolver resolver)
    {
      _resolvers.Insert(_resolvers.IndexOf(FindByType(type)) + 1, resolver);
    }

    public void AddBefore(Type type, IActivatorResolver resolver)
    {
      _resolvers.Insert(_resolvers.IndexOf(FindByType(type)), resolver);
    }

    public void AddLast(IActivatorResolver resolver)
    {
      _resolvers.Add(resolver);
    }

    public void Replace(Type type, IActivatorResolver resolver)
    {
      int index = _resolvers.IndexOf(FindByType(type));
      _resolvers.Insert(index + 1, resolver);
      _resolvers.RemoveAt(index);
    }
    #endregion

    #region IActivatorResolver Members
    public IActivator ResolveActivator(IResolutionServices services, ServiceEntry entry)
    {
      foreach (IActivatorResolver resolver in _resolvers)
      {
        IActivator activator = resolver.ResolveActivator(services, entry);
        if (activator != null)
        {
          return activator;
        }
      }
      throw new ServiceContainerException();
    }
    #endregion

    private IActivatorResolver FindByType(Type type)
    {
      foreach (IActivatorResolver resolver in _resolvers)
      {
        if (type.IsInstanceOfType(resolver))
        {
          return resolver;
        }
      }
      throw new ServiceContainerException("Unable to find ActivatorResolver of type: " + type.FullName);
    }
  }
}