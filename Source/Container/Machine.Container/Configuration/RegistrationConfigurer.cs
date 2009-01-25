using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Configuration
{
  public abstract class RegistrationConfigurer
  {
    private readonly IContainerServices _containerServices;
    private readonly ServiceEntry _entry;

    protected RegistrationConfigurer(IContainerServices containerServices, ServiceEntry entry)
    {
      _containerServices = containerServices;
      _entry = entry;
    }

    public RegistrationConfigurer Named(string name)
    {
      _entry.Name = name;
      return this;
    }

    public RegistrationConfigurer AsSingleton()
    {
      return WithLifestyle(LifestyleType.Singleton);
    }

    public RegistrationConfigurer AsTransient()
    {
      return WithLifestyle(LifestyleType.Transient);
    }

    public RegistrationConfigurer AsPerWebRequest()
    {
      return WithLifestyle(LifestyleType.PerWebRequest);
    }

    public RegistrationConfigurer AsPerThread()
    {
      return WithLifestyle(LifestyleType.PerThread);
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

    public RegistrationConfigurer Using(IPropertySettings propertySettings)
    {
      _entry.PropertySettings = propertySettings;
      return this;
    }

    protected RegistrationConfigurer IsStaticInstance(object instance)
    {
      _entry.AssertIsAcceptableInstance(instance);
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
  public class GenericRegistrationConfigurer<TType> : RegistrationConfigurer
  {
    public GenericRegistrationConfigurer(IContainerServices containerServices, ServiceEntry entry)
      : base(containerServices, entry)
    {
    }

    public RegistrationConfigurer Is(TType instance)
    {
      return IsStaticInstance(instance);
    }
  }
  public class PlainRegistrationConfigurer : RegistrationConfigurer
  {
    public PlainRegistrationConfigurer(IContainerServices containerServices, ServiceEntry entry)
      : base(containerServices, entry)
    {
    }

    public RegistrationConfigurer Is(object instance)
    {
      return IsStaticInstance(instance);
    }
  }
}
