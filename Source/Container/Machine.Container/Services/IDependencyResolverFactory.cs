using System;
using System.Collections.Generic;

namespace Machine.Container.Services
{
  public interface IDependencyResolverFactory
  {
    IActivatorResolver CreateDependencyResolver();
  }
}
