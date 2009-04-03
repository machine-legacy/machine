using System;
using System.Collections.Generic;

using Machine.Container.Plugins.Disposition;
using Machine.Container.Services.Impl;

using NUnit.Framework;

namespace Machine.Container
{
  public class with_container : MachineContainerTestsFixture
  {
    protected MachineContainer _machineContainer;

    public override void Setup()
    {
      base.Setup();
      _machineContainer = new MachineContainer();
      _machineContainer.Initialize();
      _machineContainer.AddPlugin(new DisposablePlugin());
      _machineContainer.PrepareForServices();
      _machineContainer.Start();
    }
  }

  [TestFixture]
  public class with_container_after_creating_singleton : with_container
  {
    [Test]
    public void reset_after_creating_singleton_disposable_calls_dispose()
    {
      _machineContainer.Register.Type<DisposableService>();
      DisposableService s1 = _machineContainer.Resolve.Object<DisposableService>();
      _machineContainer.Reset();
      Assert.IsTrue(s1.IsDisposed);
    }
  }

  [TestFixture]
  public class with_container_after_creating_disposable_singleton : with_container
  {
    [Test]
    public void reset_after_creating_singleton_disposable_calls_dispose()
    {
      _machineContainer.Register.Type<DisposableService>();
      DisposableService s1 = _machineContainer.Resolve.Object<DisposableService>();
      _machineContainer.Reset();
      Assert.IsTrue(s1.IsDisposed);
    }
  }
}
