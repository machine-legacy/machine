using System;
using System.Collections.Generic;

using Machine.Container.Configuration;
using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container
{
  public class ContainerRegisterer
  {
    private readonly IContainerServices _containerServices;

    public ContainerRegisterer(IContainerServices containerServices)
    {
      _containerServices = containerServices;
    }

    public virtual GenericRegistrationConfigurer<TType> Type<TType>()
    {
      ServiceEntry entry = _containerServices.ServiceEntryResolver.CreateEntryIfMissing(typeof(TType));
      return new GenericRegistrationConfigurer<TType>(_containerServices, entry);
    }

    public virtual PlainRegistrationConfigurer Type(Type type)
    {
      ServiceEntry entry = _containerServices.ServiceEntryResolver.CreateEntryIfMissing(type);
      return new PlainRegistrationConfigurer(_containerServices, entry);
    }
  }
}