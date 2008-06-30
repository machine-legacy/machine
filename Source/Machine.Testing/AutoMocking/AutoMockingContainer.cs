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
    readonly MockRepository _mocks;
    MockingDependencyResolver _mockingDependencyResolver;

    public AutoMockingContainer(MockRepository mocks)
    {
      _mocks = mocks;
    }

    protected override IActivatorResolver CreateDependencyResolver()
    {
      _mockingDependencyResolver = new MockingDependencyResolver(_mocks);
      return new RootActivatorResolver(new StaticLookupActivatorResolver(), new ActivatorStoreActivatorResolver(),
        _mockingDependencyResolver, new ThrowsPendingActivatorResolver());
    }

    public virtual TService Get<TService>()
    {
      return _mockingDependencyResolver.Get<TService>();
    }
  }
}