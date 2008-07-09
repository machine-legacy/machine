using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services
{
  public interface IActivatorFactory
  {
    IActivator CreateStaticActivator(ServiceEntry entry, object instance);
    IActivator CreateDefaultActivator(ServiceEntry entry);
  }
  public interface IRootActivatorFactory : IActivatorFactory, IChain<IActivatorFactory>
  {
  }
}