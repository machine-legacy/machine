using System;
using System.Collections.Generic;

using Machine.Container.Activators;
using Machine.Container.Model;

using NUnit.Framework;

namespace Machine.Container.Services.Impl
{
  [TestFixture]
  public class DefaultActivatorStrategyTests : MachineContainerTestsFixture
  {
    #region Member Data
    private IObjectFactory _objectFactory;
    private IServiceDependencyInspector _serviceDependencyInspector;
    private IServiceEntryResolver _serviceEntryResolver;
    private DefaultActivatorFactory _activatorFactory;
    private ServiceEntry _entry;
    #endregion

    #region Test Setup and Teardown Methods
    public override void Setup()
    {
      base.Setup();
      _entry = ServiceEntryHelper.NewEntry();
      _objectFactory = _mocks.StrictMock<IObjectFactory>();
      _serviceDependencyInspector = _mocks.DynamicMock<IServiceDependencyInspector>();
      _serviceEntryResolver = _mocks.DynamicMock<IServiceEntryResolver>();
      _activatorFactory = new DefaultActivatorFactory(_objectFactory, _serviceDependencyInspector, _serviceEntryResolver);
    }
    #endregion

    #region Test Methods
    [Test]
    public void CreateActivatorInstance_ReturnsInstanceActivator_ReturnsSameOne()
    {
      Assert.IsInstanceOfType(typeof(StaticActivator), _activatorFactory.CreateStaticActivator(_entry, new Service1()));
    }

    [Test]
    public void CreateDefaultActivator_Always_IsDefaultActivator()
    {
      Assert.IsInstanceOfType(typeof(DefaultActivator), _activatorFactory.CreateDefaultActivator(_entry));
    }
    #endregion
  }
}