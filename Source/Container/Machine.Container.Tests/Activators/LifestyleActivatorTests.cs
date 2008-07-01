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
      Run(delegate { Expect.Call(Get<ILifestyle>().CanActivate(Get<IResolutionServices>())).Return(true); });
      Assert.IsTrue(_target.CanActivate(Get<IResolutionServices>()));
    }

    [Test]
    public void Create_Always_DefersToLifestyle()
    {
      _instance = new object();
      Run(delegate { Expect.Call(Get<ILifestyle>().Activate(Get<IResolutionServices>())).Return(_instance); });
      object instance = _target.Activate(Get<IResolutionServices>());
      Assert.AreEqual(_instance, instance);
    }
    #endregion
  }
}