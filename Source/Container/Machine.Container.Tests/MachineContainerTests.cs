using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Plugins.Disposition;
using Machine.Container.Services.Impl;

using NUnit.Framework;

namespace Machine.Container
{
  [TestFixture]
  public class MachineContainerTests : MachineContainerTestsFixture
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
      _machineContainer.PrepareForServices();
      _machineContainer.Start();
    }
    #endregion

    #region Test Methods
    [Test]
    public void AddServiceNoInterface_NoDependencies_ResolvesEntry()
    {
      _machineContainer.AddService<IService1, Service1>();
      Assert.IsNotNull(_machineContainer.ResolveObject<IService1>());
    }

    [Test]
    public void AddServiceNoInterface_SingleDependency_ResolvesEntry()
    {
      _machineContainer.AddService<Service2DependsOn1>();
      _machineContainer.AddService<SimpleService1>();
      Assert.IsNotNull(_machineContainer.ResolveObject<IService2>());
    }

    [Test]
    public void HasService_DoesNot_IsFalse()
    {
      Assert.IsFalse(_machineContainer.HasService<IService1>());
    }

    [Test]
    public void HasService_Does_IsTrue()
    {
      _machineContainer.AddService<IService1, SimpleService1>();
      Assert.IsTrue(_machineContainer.HasService<IService1>());
    }

    [Test]
    public void HasService_DoesButNotUnderInterface_IsTrue()
    {
      _machineContainer.AddService<SimpleService1>();
      Assert.IsTrue(_machineContainer.HasService<IService1>());
    }

    [Test]
    [ExpectedException(typeof(PendingDependencyException))]
    public void AddService_SingleDependencyNotThere_Throws()
    {
      _machineContainer.AddService<IService2, Service2DependsOn1>();
      _machineContainer.ResolveObject<IService2>();
    }

    [Test]
    public void AddService_SingleDependencyThere_ResolvesInstance()
    {
      _machineContainer.AddService<IService2, Service2DependsOn1>();
      _machineContainer.AddService<IService1, SimpleService1>();
      Assert.IsNotNull(_machineContainer.ResolveObject<IService2>());
    }

    [Test]
    public void AddService_LazilySingleDependencyThere_ResolvesInstance()
    {
      _machineContainer.AddService<Service2DependsOn1>();
      _machineContainer.AddService<SimpleService1>();
      Assert.IsNotNull(_machineContainer.ResolveObject<IService2>());
    }

    [Test]
    [ExpectedException(typeof(AmbiguousServicesException))]
    public void AddService_LazilyMultipleDependencies_ThrowsAmbiguous()
    {
      _machineContainer.AddService<Service1DependsOn2>();
      _machineContainer.AddService<SimpleService1>();
      _machineContainer.ResolveObject<IService1>();
    }

    [Test]
    public void AddService_CircularDependency_Throws()
    {
      _machineContainer.AddService<IService2, Service2DependsOn1>();
      _machineContainer.AddService<IService1, Service1DependsOn2>();
    }

    [Test]
    [ExpectedException(typeof(ServiceResolutionException))]
    public void AddService_Duplicate_Throws()
    {
      _machineContainer.AddService<IService2, Service2DependsOn1>();
      _machineContainer.AddService<IService2, Service1DependsOn2>();
    }

    [Test]
    public void AddService_WeirdOverlappingOfServices_Throws()
    {
      _machineContainer.AddService<Service1>();
      _machineContainer.AddService<Service1DependsOn2>();
      // _machineContainer.Resolve.Object<IService1>();
    }

    [Test]
    public void AddService_WeirdOverlapping_Throws()
    {
      _machineContainer.AddService<SimpleService2>();
      _machineContainer.AddService<IService1, Service1>();
      _machineContainer.AddService<Service1DependsOn2>();
      _machineContainer.Resolve.Object<Service1>();
      _machineContainer.Resolve.Object<Service1DependsOn2>();
    }

    [Test]
    [ExpectedException(typeof(CircularDependencyException))]
    public void Resolve_CircularDependency_Throws()
    {
      _machineContainer.AddService<IService2, Service2DependsOn1>();
      _machineContainer.AddService<IService1, Service1DependsOn2>();
      _machineContainer.ResolveObject<IService2>();
    }

    [Test]
    [ExpectedException(typeof(PendingDependencyException))]
    public void Resolve_WaitingDependencies_Throws()
    {
      _machineContainer.AddService<IService2, Service2DependsOn1>();
      _machineContainer.ResolveObject<IService2>();
    }

    [Test]
    public void Resolve_NotWaitingDependencies_Works()
    {
      _machineContainer.AddService<IService2, Service2DependsOn1>();
      _machineContainer.AddService<IService1, SimpleService1>();
      Assert.IsNotNull(_machineContainer.ResolveObject<IService2>());
    }

    [Test]
    public void Resolve_Singleton_YieldsSameInstances()
    {
      _machineContainer.AddService<IService1, SimpleService1>();
      IService1 service1 = _machineContainer.ResolveObject<IService1>();
      IService1 service2 = _machineContainer.ResolveObject<IService1>();
      Assert.AreEqual(service1, service2);
    }

    [Test]
    public void Resolve_Transient_YieldsMultipleInstances()
    {
      _machineContainer.AddService<IService1, SimpleService1>(LifestyleType.Transient);
      IService1 service1 = _machineContainer.ResolveObject<IService1>();
      IService1 service2 = _machineContainer.ResolveObject<IService1>();
      Assert.AreNotEqual(service1, service2);
    }

    [Test]
    public void ResolveWithOverrides_WithOverrides_UsesOverride()
    {
      _machineContainer.AddService<IService1, Service1DependsOn2>(LifestyleType.Transient);
      Assert.IsNotNull(_machineContainer.ResolveWithOverrides<IService1>(new SimpleService2()));
    }

    [Test]
    [ExpectedException(typeof(ServiceContainerException))]
    public void ReleaseNotCreatedByContainer_Throws()
    {
      _machineContainer.AddService<IService1, SimpleService1>(LifestyleType.Transient);
      _machineContainer.Release(new SimpleService1());
    }

    [Test]
    public void ReleaseFirstTime_JustDoesThat()
    {
      _machineContainer.AddService<IService1, SimpleService1>(LifestyleType.Transient);
      IService1 service1 = _machineContainer.ResolveObject<IService1>();
      _machineContainer.Release(service1);
    }

    [Test]
    [ExpectedException(typeof(ServiceContainerException))]
    public void ReleaseSecondTime_Throws()
    {
      _machineContainer.AddService<IService1, SimpleService1>(LifestyleType.Transient);
      IService1 service1 = _machineContainer.ResolveObject<IService1>();
      _machineContainer.Release(service1);
      _machineContainer.Release(service1);
    }

    [Test]
    public void ReleaseADisposable_CallsDispose()
    {
      _machineContainer.AddService<IDisposableService, DisposableService>();
      IDisposableService disposable = _machineContainer.ResolveObject<IDisposableService>();
      _machineContainer.Release(disposable);
      Assert.IsTrue(disposable.IsDisposed);
    }

    [Test]
    [ExpectedException(typeof(AmbiguousServicesException))]
    public void LotsOfServicesWithAmbigiousDependency_Throws()
    {
      _machineContainer.AddService<IService1, Service1>();
      _machineContainer.AddService<Service2DependsOn1>();
      _machineContainer.AddService<SimpleService1>();
      IService2 service2 = _machineContainer.ResolveObject<IService2>();
    }

    [Test]
    public void LotsOfServices_Works()
    {
      ContainerResolver resolve = _machineContainer.Resolve;
      _machineContainer.AddService<IService1, Service1>();
      _machineContainer.AddService<Service2DependsOn1>();
      _machineContainer.AddService<SimpleService2>();
      IService1 service1a = _machineContainer.ResolveObject<Service1>();
      IService1 service1b = _machineContainer.ResolveObject<IService1>();
      IService2 service2a = _machineContainer.ResolveObject<Service2DependsOn1>();
      IService2 service2b = _machineContainer.ResolveObject<SimpleService2>();
      List<IService2> services = new List<IService2>(resolve.All<IService2>());
      Assert.AreEqual(2, services.Count);
    }
    #endregion
  }
  public interface IExampleService
  {
  }
  public class Service1 : IService1, IExampleService
  {
  }
  public class Service2DependsOn1 : IService2, IExampleService
  {
    private readonly IService1 _s1;
    public Service2DependsOn1(IService1 s1)
    {
      _s1 = s1;
    }
  }
  public class Service1DependsOn2 : IService1
  {
    private readonly IService2 _s2;
    public Service1DependsOn2(IService2 s2)
    {
      _s2 = s2;
    }
  }
  public class SimpleService1 : IService1
  {
  }
  public class SimpleService2 : IService2, IExampleService
  {
  }
  public interface IDisposableService
  {
    bool IsDisposed
    {
      get;
    }
  }
  public class DisposableService : IDisposableService, IDisposable
  {
    private bool _disposed;

    public bool IsDisposed
    {
      get { return _disposed; }
    }

    #region IDisposable Members
    public void Dispose()
    {
      _disposed = true;
    }
    #endregion
  }
}
