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
      _state.AssertCanAddServices();
      ServiceEntry entry = _resolver.CreateEntryIfMissing(serviceType);
      entry.LifestyleType = lifestyleType;
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
      _state.AssertCanAddServices();
      ServiceEntry entry = _resolver.CreateEntryIfMissing(serviceType, implementationType);
      entry.LifestyleType = lifestyleType;
    }

    public void AddService<TService>(object instance)
    {
      AddService(typeof(TService), instance);
    }

    public void AddService(Type serviceType, object instance)
    {
      _state.AssertCanAddServices();
      ServiceEntry entry = _resolver.CreateEntryIfMissing(serviceType);
      IActivator activator = _activatorStrategy.CreateStaticActivator(entry, instance);
      _activatorStore.AddActivator(entry, activator);
    }

    // Resolving
    public T Resolve<T>()
    {
      return (T)Resolve(typeof(T));
    }

    public object Resolve(Type serviceType)
    {
      return ResolveWithOverrides(serviceType);
    }

    public T New<T>(params object[] serviceOverrides)
    {
      AddService<T>(LifestyleType.Transient);
      return ResolveWithOverrides<T>(serviceOverrides);
    }

    public T ResolveWithOverrides<T>(params object[] serviceOverrides)
    {
      return (T)ResolveWithOverrides(typeof(T), serviceOverrides);
    }

    public object ResolveWithOverrides(Type serviceType, params object[] serviceOverrides)
    {
      _state.AssertCanResolve();
      IResolutionServices services = _containerServices.CreateResolutionServices(serviceOverrides);
      ResolvedServiceEntry entry = _resolver.ResolveEntry(services, serviceType, true);
      return entry.Activate(services);
    }
  }
}
