using System;
using System.Collections.Generic;

using Machine.Container.Services;

using NUnit.Framework;

using Rhino.Mocks;

namespace Machine.Container.Activators
{
  [TestFixture]
  public class LifestyleActivatorTests : ScaffoldTests<LifestyleActivator>
  {
    #region Member Data
    object _instance;
    #endregion

    #region Test Methods
    [Test]
    public void CanActivate_Always_DefersToLifestyle()
    {
      _instance = new object();
      Run(delegate { Expect.Call(Get<ILifestyle>().CanActivate(Get<IContainerServices>())).Return(true); });
      Assert.IsTrue(_target.CanActivate(Get<IContainerServices>()));
    }

    [Test]
    public void Create_Always_DefersToLifestyle()
    {
      _instance = new object();
      Run(delegate { Expect.Call(Get<ILifestyle>().Activate(Get<IContainerServices>())).Return(_instance); });
      object instance = _target.Activate(Get<IContainerServices>());
      Assert.AreEqual(_instance, instance);
    }
    #endregion
  }
}