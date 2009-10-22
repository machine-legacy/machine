using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services.Impl
{
  public class DefaultContainerInfrastructureFactory : IContainerInfrastructureFactory
  {
    #region IContainerInfrastructureFactory Members
    public virtual IRootActivatorResolver CreateDependencyResolver()
    {
      RootActivatorResolverChain resolver = new RootActivatorResolverChain();
      resolver.AddLast(new StaticLookupActivatorResolver());
      resolver.AddLast(new ActivatorStoreActivatorResolver());
      resolver.AddLast(new ThrowsPendingActivatorResolver());
      return resolver;
    }

    public virtual IInstanceTrackingPolicy CreateInstanceTrackingPolicy()
    {
      return new DoNotTrackInstances();
    }

    public virtual IRootActivatorFactory CreateActivatorFactory(IServiceEntryResolver entryResolver)
    {
      RootActivatorFactoryChain activatorFactory = new RootActivatorFactoryChain();
      activatorFactory.AddFirst(new DefaultActivatorFactory(new DotNetObjectFactory(), new ServiceDependencyInspector(), entryResolver));
      return activatorFactory;
    }
    #endregion
  }
}