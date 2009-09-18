using System;
using System.Collections.Generic;
using System.Reflection;

using Machine.Container.Model;
using Machine.Container.Plugins;
using Machine.Container.Plugins.Disposition;
using Machine.Container.Services;
using Machine.Core.Services.Impl;
using Machine.Core.Utility;

namespace Machine.Container
{
  public class ContainerRegistrationHelper
  {
    readonly IMachineContainer _container;

    public ContainerRegisterer Register
    {
      get { return _container.Register; }
    }

    public ContainerRegistrationHelper(IMachineContainer container)
    {
      _container = container;
    }

    public virtual void AddAttributedServicesFrom(Assembly assembly)
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
      _container.Register.Type<DotNetDotNet>();
      _container.Register.Type<DotNetEnvironment>();
    }

    public virtual void AddPlugins(IEnumerable<IServiceContainerPlugin> plugins)
    {
      foreach (IServiceContainerPlugin plugin in plugins)
      {
        _container.AddPlugin(plugin);
      }
    }

    public virtual void PrepareForServices()
    {
      _container.PrepareForServices();
    }

    public virtual void AddStandardPlugins(IEnumerable<IServiceContainerPlugin> plugins)
    {
      AddPlugins(new[] { new DisposablePlugin() });
      AddPlugins(plugins);
    }

    public virtual void AddServiceCollection<T>() where T : IServiceCollection
    {
      AddServiceCollection(typeof(T));
    }

    public virtual void AddServiceCollection(Type type)
    {
      if (!typeof(IServiceCollection).IsAssignableFrom(type))
        throw new ArgumentException("type");
      _container.Register.Type(type);
      AddServiceCollection((IServiceCollection)_container.Resolve.Object(type));
    }

    public virtual void StartContainer()
    {
      _container.Start();
    }

    public virtual void Start<T>() where T : IStartable
    {
      _container.Register.Type(typeof(T));
      IStartable startable = (IStartable)_container.Resolve.Object(typeof(T));
      startable.Start();
    }

    public virtual void AddServiceCollectionsFrom(Assembly assembly)
    {
      List<Type> types = new List<Type>();
      foreach (Type type in assembly.GetTypes())
      {
        if (typeof(IServiceCollection).IsAssignableFrom(type))
        {
          _container.Register.Type(type);
          types.Add(type);
        }
      }
      foreach (Type type in types)
      {
        AddServiceCollection((IServiceCollection)_container.Resolve.Object(type));
      }
    }

    public virtual void AddServiceCollection(IServiceCollection services)
    {
      services.RegisterServices(_container.Register);
    }
  }
}
