using System;
using System.Collections.Generic;

using Machine.Container.Model;

using NUnit.Framework;
using Rhino.Mocks;

namespace Machine.Container.Services.Impl
{
  [TestFixture]
  public class RootActivatorResolverTests : ScaffoldTests<RootActivatorResolverChain>
  {
    #region Member Data
    private readonly ServiceEntry _entry = ServiceEntryHelper.NewEntry();
    private IActivatorResolver _resolver1;
    private IActivatorResolver _resolver2;
    #endregion

    #region Test Methods
    [Test]
    [ExpectedException(typeof(ServiceContainerException))]
    public void ResolveActivator_CantResolveAtAll_ThrowsAfterTrying()
    {
      Run(delegate
      {
        Expect.Call(_resolver1.ResolveActivator(Get<IResolutionServices>(), _entry)).Return(null);
        Expect.Call(_resolver2.ResolveActivator(Get<IResolutionServices>(), _entry)).Return(null);
      });
      _target.ResolveActivator(Get<IResolutionServices>(), _entry);
    }

    [Test]
    public void CanResolveDependency_GetsResolved_Returns()
    {
      Run(delegate
      {
        Expect.Call(_resolver1.ResolveActivator(Get<IResolutionServices>(), _entry)).Return(Get<IActivator>());
      });
      Assert.AreEqual(Get<IActivator>(), _target.ResolveActivator(Get<IResolutionServices>(), _entry));
    }
    #endregion

    #region Methods
    protected override RootActivatorResolverChain Create()
    {
      _resolver1 = _mocks.DynamicMock<IActivatorResolver>();
      _resolver2 = _mocks.DynamicMock<IActivatorResolver>();
      RootActivatorResolverChain resolver = new RootActivatorResolverChain();
      resolver.AddLast(_resolver1);
      resolver.AddLast(_resolver2);
      return resolver;
    }
    #endregion
  }
}