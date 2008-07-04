using System;
using System.Collections.Generic;

using Machine.Container.Lifestyles;
using Machine.Container.Model;

namespace Machine.Container.Services.Impl
{
  public class LifestyleFactory : ILifestyleFactory
  {
    #region Member Data
    private readonly IActivatorFactory _activatorFactory;
    #endregion

    #region LifestyleFactory()
    public LifestyleFactory(IActivatorFactory activatorFactory)
    {
      _activatorFactory = activatorFactory;
    }
    #endregion

    #region ILifestyleFactory Members
    public ILifestyle CreateLifestyle(ServiceEntry entry)
    {
      switch (entry.LifestyleType)
      {
        case LifestyleType.Singleton:
          return CreateSingletonLifestyle(entry);
        case LifestyleType.Transient:
          return CreateTransientLifestyle(entry);
      }
      throw new ArgumentException("entry");
    }
    #endregion

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
  }
}