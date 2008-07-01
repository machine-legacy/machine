using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

using NUnit.Framework;

using Rhino.Mocks;

namespace Machine.Container.Lifestyles
{
  [TestFixture]
  public class SingletonLifestyleTests : ScaffoldTests<SingletonLifestyle>
  {
    #region Member Data
    readonly object _instance = new object();
    readonly ServiceEntry _serviceEntry = ServiceEntryHelper.NewEntry();
    #endregion

    #region Test Methods
    [Test]
    public void Initialize_Always_CreateDefaultActivator()
    {
      Run(
        delegate { SetupResult.For(Get<IActivatorStrategy>().CreateDefaultActivator(_serviceEntry)).Return(Get<IActivator>()); });
      _target.Initialize();
      _mocks.Verify(Get<IActivatorStrategy>());
    }

    [Test]
    public void Create_FirstCall_InvokesDefaultActivator()
    {
      Run(delegate
      {
        SetupResult.For(Get<IActivatorStrategy>().CreateDefaultActivator(_serviceEntry)).Return(Get<IActivator>());
        Expect.Call(Get<IActivator>().Activate(Get<IResolutionServices>())).Return(_instance);
      });
      _target.Initialize();
      Assert.AreEqual(_instance, _target.Activate(Get<IResolutionServices>()));
    }

    [Test]
    public void Create_SecondCall_ReturnsPrevious()
    {
      Run(delegate
      {
        SetupResult.For(Get<IActivatorStrategy>().CreateDefaultActivator(_serviceEntry)).Return(Get<IActivator>());
        Expect.Call(Get<IActivator>().Activate(Get<IResolutionServices>())).Return(_instance);
      });
      _target.Initialize();
      Assert.AreEqual(_instance, _target.Activate(Get<IResolutionServices>()));
      Assert.AreEqual(_instance, _target.Activate(Get<IResolutionServices>()));
    }
    #endregion

    #region Methods
    protected override SingletonLifestyle Create()
    {
      return new SingletonLifestyle(Get<IActivatorStrategy>(), _serviceEntry);
    }
    #endregion
  }
}