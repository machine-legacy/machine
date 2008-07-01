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

    public RegistrationConfigurer Type<TType>()
    {
      return Type(typeof(TType));
    }

    public RegistrationConfigurer Type(Type type)
    {
      ServiceEntry entry = _containerServices.ServiceEntryResolver.CreateEntryIfMissing(type);
      return new RegistrationConfigurer(_containerServices, entry);
    }
  }
}