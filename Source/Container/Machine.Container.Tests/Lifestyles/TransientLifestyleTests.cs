using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

using NUnit.Framework;

using Rhino.Mocks;

namespace Machine.Container.Lifestyles
{
  [TestFixture]
  public class TransientLifestyleTests : ScaffoldTests<TransientLifestyle>
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
        Expect.Call(Get<IActivator>().Activate(Get<IContainerServices>())).Return(_instance);
      });
      _target.Initialize();
      Assert.AreEqual(_instance, _target.Activate(Get<IContainerServices>()));
    }

    [Test]
    public void Create_SecondCall_InvokesDefaultActivatorAgain()
    {
      Run(delegate
      {
        SetupResult.For(Get<IActivatorStrategy>().CreateDefaultActivator(_serviceEntry)).Return(Get<IActivator>());
        Expect.Call(Get<IActivator>().Activate(Get<IContainerServices>())).Return(new object());
        Expect.Call(Get<IActivator>().Activate(Get<IContainerServices>())).Return(_instance);
      });
      _target.Initialize();
      _target.Activate(Get<IContainerServices>());
      Assert.AreEqual(_instance, _target.Activate(Get<IContainerServices>()));
    }
    #endregion

    #region Methods
    protected override TransientLifestyle Create()
    {
      return new TransientLifestyle(Get<IActivatorStrategy>(), _serviceEntry);
    }
    #endregion
  }
}