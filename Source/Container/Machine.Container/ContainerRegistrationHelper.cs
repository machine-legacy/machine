using System;
using System.Collections.Generic;
using System.Reflection;

using Machine.Container.Model;
using Machine.Container.Plugins;
using Machine.Container.Services;
using Machine.Core.Services.Impl;
using Machine.Core.Utility;

namespace Machine.Container
{
  public class ContainerRegistrationHelper
  {
    private readonly IMachineContainer _container;

    public ContainerRegistrationHelper(IMachineContainer container)
    {
      _container = container;
    }

    public virtual void AddAttributedServices(Assembly assembly)
    {
      foreach (Type type in assembly.GetTypes())
      {
        LifestyleAttribute lifestyleAttribute = ReflectionHelper.GetAttribute<LifestyleAttribute>(type, true);
        if (lifestyleAttribute != null)
        {
          switch (lifestyleAttribute.Lifestyle)
          {
            case LifestyleType.Transient:
              _container.Register.Type(type).AsTransient();
              break;
            case LifestyleType.Singleton:
              _container.Register.Type(type).AsSingleton();
              break;
            default:
              throw new NotSupportedException("Not supported lifestyle: " + lifestyleAttribute.Lifestyle);
          }
        }
      }
    }

    public virtual void AddCore()
    {
      _container.Register.Type<Clock>();
      _container.Register.Type<Namer>();
      _container.Register.Type<FileSystem>();
      _container.Register.Type<DotNetObjectActivator>();
      _container.Register.Type<ThreadManager>();
    }

    public virtual void AddServiceCollectionsFrom(Assembly assembly)
    {
      foreach (Type type in assembly.GetTypes())
      {
        if (typeof(IServiceCollection).IsAssignableFrom(type))
        {
          AddServiceCollection(type);
        }
      }
    }

    public virtual void AddServiceCollection<T>() where T : IServiceCollection
    {
      AddServiceCollection(typeof(T));
    }

    public virtual void AddServiceCollection(Type serviceCollectionType)
    {
      if (!typeof(IServiceCollection).IsAssignableFrom(serviceCollectionType)) throw new ArgumentException("serviceCollectionType");
      _container.Register.Type(serviceCollectionType);
      AddServiceCollection((IServiceCollection)_container.Resolve.Object(serviceCollectionType));
    }

    public virtual void AddServiceCollection(IServiceCollection services)
    {
      services.RegisterServices(_container.Register);
    }
  }
}
