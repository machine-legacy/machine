using System;
using System.Collections.Generic;

using Machine.Container.Configuration;
using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container
{
  public class MachineContainer : CompartmentalizedMachineContainer, IHighLevelContainer
  {
    // Adding Services / Registration
    public void Add<TService>()
    {
      Add<TService>(LifestyleType.Singleton);
    }

    public void Add<TService>(LifestyleType lifestyleType)
    {
      Add(typeof(TService), lifestyleType);
    }

    public void Add(Type serviceType, LifestyleType lifestyleType)
    {
      Register.Type(serviceType).WithLifestyle(lifestyleType);
    }

    public void Add<TService>(Type implementationType)
    {
      Add(typeof(TService), implementationType, LifestyleType.Singleton);
    }

    public void Add<TService, TImpl>()
    {
      Add<TService, TImpl>(LifestyleType.Singleton);
    }

    public void Add<TService, TImpl>(LifestyleType lifestyleType)
    {
      Add(typeof(TService), typeof(TImpl), lifestyleType);
    }

    public void Add(Type serviceType, Type implementationType, LifestyleType lifestyleType)
    {
      Register.Type(implementationType).Provides(serviceType).WithLifestyle(lifestyleType);
    }

    public void Add<TService>(object instance)
    {
      Add(typeof(TService), instance);
    }

    public void Add(Type serviceType, object instance)
    {
      Register.Type(serviceType).Is(instance);
    }

    // Resolving
    public T ResolveObject<T>()
    {
      return (T)ResolveObject(typeof(T));
    }

    public object ResolveObject(Type type)
    {
      return ResolveWithOverrides(type);
    }

    public T New<T>(params object[] overrides)
    {
      return Resolve.New<T>(overrides);
    }

    public T ResolveWithOverrides<T>(params object[] overrides)
    {
      return (T)ResolveWithOverrides(typeof(T), overrides);
    }

    public object ResolveWithOverrides(Type type, params object[] overrides)
    {
      return Resolve.Object(type, overrides);
    }
  }
}
