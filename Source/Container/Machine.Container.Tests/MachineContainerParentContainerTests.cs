using Machine.Container.Plugins.Disposition;
using Machine.Container.Plugins.ParentContainers;
using Machine.Container.Services.Impl;

using NUnit.Framework;

namespace Machine.Container
{
  [TestFixture]
  public class MachineContainerParentContainerTests : MachineContainerTestsFixture
  {
    #region Member Data
    private MachineContainer _parentContainer;
    private MachineContainer _childContainer;
    #endregion

    #region Test Setup and Teardown Methods
    public override void Setup()
    {
      base.Setup();
      _parentContainer = new MachineContainer();
      _parentContainer.Initialize();
      _parentContainer.AddPlugin(new DisposablePlugin());
      _parentContainer.PrepareForServices();
      _parentContainer.Start();

      _childContainer = new MachineContainer();
      _childContainer.Initialize();
      _childContainer.AddPlugin(new DisposablePlugin());
      _childContainer.AddPlugin(new ParentContainer(_parentContainer));
      _childContainer.PrepareForServices();
      _childContainer.Start();
    }
    #endregion

    #region Test Methods
    [Test]
    public void CanResolve_Service_In_Parent_Directly_In_Child_Is_True_When_Its_Registered()
    {
      _parentContainer.Register.Type<Service1>();
      Assert.IsTrue(_childContainer.CanResolve<IService1>());
    }

    [Test]
    public void CanResolve_Service_In_Child_That_Has_Missing_Dependency_IsFalse()
    {
      _childContainer.Register.Type<Service2DependsOn1>();
      Assert.IsFalse(_childContainer.CanResolve<IService1>());
    }

    [Test]
    public void CanResolve_Service_In_Child_That_Has_Dependency_In_Parent_IsTrue()
    {
      _parentContainer.Register.Type<Service1>();
      _childContainer.Register.Type<Service2DependsOn1>();
      Assert.IsTrue(_childContainer.CanResolve<IService2>());
    }

    [Test]
    public void Resolve_Service_In_Parent_Directly_In_Child_Is_True_When_Its_Registered()
    {
      _parentContainer.Register.Type<Service1>();
      Assert.AreEqual(_parentContainer.Resolve.Object<IService1>(), _childContainer.Resolve.Object<IService1>());
    }

    [Test]
    public void Resolve_Child_Service_That_Depends_On_Parent_Service_Works()
    {
      _parentContainer.Register.Type<Service1>();
      _childContainer.Register.Type<Service2DependsOn1>();
      Assert.IsNotNull(_childContainer.Resolve.Object<IService2>());
    }
    #endregion
  }
}