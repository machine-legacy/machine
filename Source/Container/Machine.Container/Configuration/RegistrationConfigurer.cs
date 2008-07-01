using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Configuration
{
  public class RegistrationConfigurer
  {
    readonly IActivatorStrategy _activatorStrategy;
    readonly IActivatorStore _activatorStore;
    readonly ServiceEntry _entry;

    public RegistrationConfigurer(IActivatorStrategy activatorStrategy, IActivatorStore activatorStore, ServiceEntry entry)
    {
      _activatorStore = activatorStore;
      _activatorStrategy = activatorStrategy;
      _entry = entry;
    }

    public RegistrationConfigurer AsSingleton
    {
      get
      {
        _entry.LifestyleType = LifestyleType.Singleton;
        return this;
      }
    }

    public RegistrationConfigurer AsTransient
    {
      get
      {
        _entry.LifestyleType = LifestyleType.Transient;
        return this;
      }
    }

    public RegistrationConfigurer Provides(Type serviceType)
    {
      _entry.ServiceType = serviceType;
      return this;
    }

    public RegistrationConfigurer Provides<TService>()
    {
      return Provides(typeof(TService));
    }

    public RegistrationConfigurer Is(object instance)
    {
      IActivator activator = _activatorStrategy.CreateStaticActivator(_entry, instance);
      _activatorStore.AddActivator(_entry, activator);
      return this;
    }
  }
}
