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
    public void AddService<TService>()
    {
      AddService<TService>(LifestyleType.Singleton);
    }

    public void AddService<TService>(LifestyleType lifestyleType)
    {
      AddService(typeof(TService), lifestyleType);
    }

    public void AddService(Type serviceType, LifestyleType lifestyleType)
    {
      Register.Type(serviceType).WithLifestyle(lifestyleType);
    }

    public void AddService<TService>(Type implementationType)
    {
      AddService(typeof(TService), implementationType, LifestyleType.Singleton);
    }

    public void AddService<TService, TImpl>()
    {
      AddService<TService, TImpl>(LifestyleType.Singleton);
    }

    public void AddService<TService, TImpl>(LifestyleType lifestyleType)
    {
      AddService(typeof(TService), typeof(TImpl), lifestyleType);
    }

    public void AddService(Type serviceType, Type implementationType, LifestyleType lifestyleType)
    {
      Register.Type(implementationType).Provides(serviceType).WithLifestyle(lifestyleType);
    }

    public void AddService<TService>(object instance)
    {
      AddService(typeof(TService), instance);
    }

    public void AddService(Type serviceType, object instance)
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
      return Resolve.WithOverrides(type, overrides);
    }
  }
}
