using System;
using System.Collections.Generic;
using System.Reflection;

using Machine.Container.Model;
using Machine.Container.Services;
using Machine.Core.Services.Impl;
using Machine.Core.Utility;

namespace Machine.Container
{
  public class ContainerRegistrationHelper
  {
    private readonly IHighLevelContainer _container;

    public ContainerRegistrationHelper(IHighLevelContainer container)
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
              _container.AddService(type, LifestyleType.Transient);
              break;
            case LifestyleType.Singleton:
              _container.AddService(type, LifestyleType.Singleton);
              break;
            default:
              throw new NotSupportedException("Not supported lifestyle: " + lifestyleAttribute.Lifestyle);
          }
        }
      }
    }

    public virtual void AddCore()
    {
      _container.AddService<Clock>();
      _container.AddService<Namer>();
      _container.AddService<FileSystem>();
      _container.AddService<DotNetObjectActivator>();
      _container.AddService<ThreadManager>();
    }
  }
}
