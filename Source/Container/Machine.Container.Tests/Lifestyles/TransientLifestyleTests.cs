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
    readonly object _instance = new SimpleService1();
    readonly ServiceEntry _entry = ServiceEntryHelper.NewEntry();
    #endregion

    #region Test Methods
    [Test]
    public void Initialize_Always_CreateDefaultActivator()
    {
      Run(
        delegate { SetupResult.For(Get<IActivatorStrategy>().CreateDefaultActivator(_entry)).Return(Get<IActivator>()); });
      _target.Initialize();
      _mocks.Verify(Get<IActivatorStrategy>());
    }

    [Test]
    public void Create_FirstCall_InvokesDefaultActivator()
    {
      Activation activation = new Activation(_entry, _instance);
      Run(delegate
      {
        SetupResult.For(Get<IActivatorStrategy>().CreateDefaultActivator(_entry)).Return(Get<IActivator>());
        Expect.Call(Get<IActivator>().Activate(Get<IResolutionServices>())).Return(activation);
      });
      _target.Initialize();
      Assert.AreEqual(activation, _target.Activate(Get<IResolutionServices>()));
    }

    [Test]
    public void Create_SecondCall_InvokesDefaultActivatorAgain()
    {
      Activation activation1 = new Activation(_entry, new SimpleService1());
      Activation activation2 = new Activation(_entry, _instance);
      Run(delegate
      {
        SetupResult.For(Get<IActivatorStrategy>().CreateDefaultActivator(_entry)).Return(Get<IActivator>());
        Expect.Call(Get<IActivator>().Activate(Get<IResolutionServices>())).Return(activation1);
        Expect.Call(Get<IActivator>().Activate(Get<IResolutionServices>())).Return(activation2);
      });
      _target.Initialize();
      _target.Activate(Get<IResolutionServices>());
      Assert.AreEqual(activation2, _target.Activate(Get<IResolutionServices>()));
    }
    #endregion

    #region Methods
    protected override TransientLifestyle Create()
    {
      return new TransientLifestyle(Get<IActivatorStrategy>(), _entry);
    }
    #endregion
  }
}