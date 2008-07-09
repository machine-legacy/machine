using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Configuration
{
  public class RegistrationConfigurer
  {
    private readonly IContainerServices _containerServices;
    private readonly ServiceEntry _entry;

    public RegistrationConfigurer(IContainerServices containerServices, ServiceEntry entry)
    {
      _containerServices = containerServices;
      _entry = entry;
    }

    public RegistrationConfigurer AsSingleton()
    {
      return WithLifestyle(LifestyleType.Singleton);
    }

    public RegistrationConfigurer AsTransient()
    {
      return WithLifestyle(LifestyleType.Transient);
    }

    public RegistrationConfigurer WithLifestyle(LifestyleType type)
    {
      _entry.LifestyleType = type;
      return this;
    }

    public RegistrationConfigurer ImplementedBy(Type type)
    {
      _entry.ImplementationType = type;
      return this;
    }

    public RegistrationConfigurer ImplementedBy<TType>()
    {
      return ImplementedBy(typeof(TType));
    }

    public RegistrationConfigurer Provides(Type type)
    {
      _entry.ServiceType = type;
      return this;
    }

    public RegistrationConfigurer Provides<TType>()
    {
      return Provides(typeof(TType));
    }

    public RegistrationConfigurer Is(object instance)
    {
      IActivator activator = _containerServices.ActivatorFactory.CreateStaticActivator(_entry, instance);
      _containerServices.ActivatorStore.AddActivator(_entry, activator);
      return this;
    }

    public RegistrationConfigurer Intercept(Type type)
    {
      _containerServices.StatePolicy.AssertSupports(SupportedFeature.Interceptors);
      _entry.AddInterceptor(type);
      return this;
    }

    public RegistrationConfigurer Intercept<TType>()
    {
      return Intercept(typeof(TType));
    }
  }
}
