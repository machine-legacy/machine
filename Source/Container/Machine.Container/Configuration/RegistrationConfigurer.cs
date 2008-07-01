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
      IActivator activator = _containerServices.ActivatorStrategy.CreateStaticActivator(_entry, instance);
      _containerServices.ActivatorStore.AddActivator(_entry, activator);
      return this;
    }

    public RegistrationConfigurer WithLifestyle(LifestyleType type)
    {
      _entry.LifestyleType = type;
      return this;
    }
  }
}
