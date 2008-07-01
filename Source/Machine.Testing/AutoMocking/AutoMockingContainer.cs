using System;
using System.Collections.Generic;

using Machine.Container;
using Machine.Container.Services;
using Machine.Container.Services.Impl;

using Rhino.Mocks;

namespace Machine.Testing.AutoMocking
{
  public class AutoMockingContainer : MachineContainer
  {
    private readonly MockRepository _mocks;
    private readonly MockingDependencyResolver _mockingDependencyResolver;

    public AutoMockingContainer(MockRepository mocks)
      : this(mocks, new MockingDependencyResolver(mocks))
    {
    }

    public AutoMockingContainer(MockRepository mocks, MockingDependencyResolver mockingDependencyResolver)
      : base(new CompartmentalizedMachineContainer(new MockingDependencyResolverFactory(mockingDependencyResolver)))
    {
      _mocks = mocks;
      _mockingDependencyResolver = mockingDependencyResolver;
    }

    public virtual TService Get<TService>()
    {
      return _mockingDependencyResolver.Get<TService>();
    }
  }
  public class MockingDependencyResolverFactory : IDependencyResolverFactory
  {
    private readonly MockingDependencyResolver _mockingDependencyResolver;

    public MockingDependencyResolverFactory(MockingDependencyResolver mockingDependencyResolver)
    {
      _mockingDependencyResolver = mockingDependencyResolver;
    }

    #region IDependencyResolverFactory Members
    public IActivatorResolver CreateDependencyResolver()
    {
      return new RootActivatorResolver(new StaticLookupActivatorResolver(), new ActivatorStoreActivatorResolver(), _mockingDependencyResolver, new ThrowsPendingActivatorResolver());
    }
    #endregion
  }
}