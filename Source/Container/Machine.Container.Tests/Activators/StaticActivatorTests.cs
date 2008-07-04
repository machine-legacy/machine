using System;
using System.Collections.Generic;

using Machine.Container.Services;
using Machine.Container.Model;

using NUnit.Framework;

namespace Machine.Container.Activators
{
  [TestFixture]
  public class StaticActivatorTests : MachineContainerTestsFixture
  {
    #region Member Data
    private StaticActivator _activator;
    private ServiceEntry _entry;
    private SimpleService1 _instance;
    #endregion

    #region Test Setup and Teardown Methods
    public override void Setup()
    {
      base.Setup();
      _entry = ServiceEntryHelper.NewEntry();
      _instance = new SimpleService1();
      _activator = new StaticActivator(_entry, _instance);
    }
    #endregion

    #region Test Methods
    [Test]
    public void Create_Always_ReturnsInstance()
    {
      Activation activation = new Activation(_entry, _instance);
      Assert.AreEqual(activation, _activator.Activate(Get<IResolutionServices>()));
      Assert.AreEqual(activation, _activator.Activate(Get<IResolutionServices>()));
    }
    #endregion
  }
}