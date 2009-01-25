using System;
using System.Collections.Generic;

using Machine.Container.Lifestyles;
using Machine.Container.Model;

namespace Machine.Container.Services.Impl
{
  public class LifestyleFactory : ILifestyleFactory
  {
    private readonly IActivatorFactory _activatorFactory;

    public LifestyleFactory(IActivatorFactory activatorFactory)
    {
      _activatorFactory = activatorFactory;
    }

    public ILifestyle CreateLifestyle(ServiceEntry entry)
    {
      switch (entry.LifestyleType)
      {
        case LifestyleType.Singleton:
          return CreateSingletonLifestyle(entry);
        case LifestyleType.Transient:
          return CreateTransientLifestyle(entry);
        case LifestyleType.PerWebRequest:
          return CreateWebRequestLifestyle(entry);
        case LifestyleType.PerThread:
          return CreatePerThreadLifestyle(entry);
        case LifestyleType.PerWebRequestAndPerThreadHybrid:
          return CreatePerWebRequestAndPerThreadHybridLifestyle(entry);
      }
      throw new ArgumentException("entry");
    }

    public ILifestyle CreateSingletonLifestyle(ServiceEntry entry)
    {
      ILifestyle lifestyle = new SingletonLifestyle(_activatorFactory, entry);
      lifestyle.Initialize();
      return lifestyle;
    }

    public ILifestyle CreateTransientLifestyle(ServiceEntry entry)
    {
      ILifestyle lifestyle = new TransientLifestyle(_activatorFactory, entry);
      lifestyle.Initialize();
      return lifestyle;
    }

    public ILifestyle CreateWebRequestLifestyle(ServiceEntry entry)
    {
      ILifestyle lifestyle = new WebRequestLifestyle(_activatorFactory, entry);
      lifestyle.Initialize();
      return lifestyle;
    }

    public ILifestyle CreatePerThreadLifestyle(ServiceEntry entry)
    {
      ILifestyle lifestyle = new PerThreadLifestyle(_activatorFactory, entry);
      lifestyle.Initialize();
      return lifestyle;
    }

    public ILifestyle CreatePerWebRequestAndPerThreadHybridLifestyle(ServiceEntry entry)
    {
      ILifestyle lifestyle = new HybridLifestyle(_activatorFactory, entry);
      lifestyle.Initialize();
      return lifestyle;
    }
  }
}