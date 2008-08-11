using Machine.Container.Plugins.Disposition;
using Machine.Container.Plugins.ObjectFactories;
using Machine.Container.Services.Impl;

using NUnit.Framework;

namespace Machine.Container
{
  [TestFixture]
  public class MachineContainerObjectFactoriesTests : MachineContainerTestsFixture
  {
    #region Member Data
    private MachineContainer _machineContainer;
    #endregion

    #region Test Setup and Teardown Methods
    public override void Setup()
    {
      base.Setup();
      _machineContainer = new MachineContainer();
      _machineContainer.Initialize();
      _machineContainer.AddPlugin(new DisposablePlugin());
      _machineContainer.AddPlugin(new FactoriesPlugin());
      _machineContainer.PrepareForServices();
      _machineContainer.Start();
    }
    #endregion

    #region Test Methods
    [Test]
    public void CanResolve_Service_That_We_Have_Factory_For_IsTrue()
    {
      _machineContainer.Register.Type<Service1Factory>();
      Assert.IsTrue(_machineContainer.CanResolve<IService1>());
    }

    [Test]
    public void Resolve_Service_That_We_Have_Factory_For_IsNewInstance()
    {
      _machineContainer.Register.Type<Service1Factory>();
      IService1 s1 = _machineContainer.Resolve.Object<IService1>();
      IService1 s2 = _machineContainer.Resolve.Object<IService1>();
      Assert.AreNotEqual(s1, s2);
    }
    #endregion
  }
  public class Service1Factory : IFactory<IService1>
  {
    #region IFactory<IService1> Members
    public IService1 Create()
    {
      return new SimpleService1();
    }

    public void Deactivate(IService1 instance)
    {
    }
    #endregion
  }
}