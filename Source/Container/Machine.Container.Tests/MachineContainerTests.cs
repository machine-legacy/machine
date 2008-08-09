using System;
using System.Collections.Generic;

using Castle.Core.Interceptor;

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
    public void CanResolve_DoesNot_IsFalse()
    {
      Assert.IsFalse(_machineContainer.CanResolve<IService1>());
    }

    [Test]
    public void CanResolve_Does_IsTrue()
    {
      _machineContainer.Add<IService1, SimpleService1>();
      Assert.IsTrue(_machineContainer.CanResolve<IService1>());
    }

    [Test]
    public void CanResolve_DoesButNotUnderInterface_IsTrue()
    {
      _machineContainer.Add<SimpleService1>();
      Assert.IsTrue(_machineContainer.CanResolve<IService1>());
    }

    [Test]
    public void Resolve_By_Interface_No_Dependencies_Resolves_Entry()
    {
      _machineContainer.Add<IService1, Service1>();
      Assert.IsNotNull(_machineContainer.ResolveObject<IService1>());
    }

    [Test]
    public void Resolve_By_Interface_Single_Dependency_Resolves_Entry()
    {
      _machineContainer.Add<Service2DependsOn1>();
      _machineContainer.Add<SimpleService1>();
      Assert.IsNotNull(_machineContainer.ResolveObject<IService2>());
    }

    [Test]
    [ExpectedException(typeof(PendingDependencyException))]
    public void Resolve_Single_Dependency_Not_There_Throws()
    {
      _machineContainer.Add<IService2, Service2DependsOn1>();
      _machineContainer.ResolveObject<IService2>();
    }

    [Test]
    public void Resolve_Single_Dependency_There_Resolves_Instance()
    {
      _machineContainer.Add<IService2, Service2DependsOn1>();
      _machineContainer.Add<IService1, SimpleService1>();
      Assert.IsNotNull(_machineContainer.ResolveObject<IService2>());
    }

    [Test]
    public void Resolve_Lazily_Single_Dependency_There_Resolves_Instance()
    {
      _machineContainer.Add<Service2DependsOn1>();
      _machineContainer.Add<SimpleService1>();
      Assert.IsNotNull(_machineContainer.ResolveObject<IService2>());
    }

    [Test]
    [ExpectedException(typeof(AmbiguousServicesException))]
    public void Resolve_Lazily_Multiple_Dependencies_Throws_Ambiguous()
    {
      _machineContainer.Add<Service1DependsOn2>();
      _machineContainer.Add<SimpleService1>();
      _machineContainer.ResolveObject<IService1>();
    }

    [Test]
    public void Add_Circular_Dependency_Does_Nothing()
    {
      _machineContainer.Add<IService2, Service2DependsOn1>();
      _machineContainer.Add<IService1, Service1DependsOn2>();
    }

    [Test]
    public void Add_Replaces_The_Old_Service()
    {
      _machineContainer.Add<IService2, Service2DependsOn1>();
      _machineContainer.Add<IService2, SimpleService2>();
    }

    [Test]
    public void Resolve_Weird_Overlapping_Of_Services_Throws()
    {
      _machineContainer.Add<Service1>();
      _machineContainer.Add<SimpleService1>();
      _machineContainer.Resolve.Object<Service1>();
      _machineContainer.Resolve.Object<SimpleService1>();
    }

    [Test]
    public void Resolve_Weird_Overlapping_Throws()
    {
      _machineContainer.Add<SimpleService2>();
      _machineContainer.Add<IService1, Service1>();
      _machineContainer.Add<Service1DependsOn2>();
      _machineContainer.Resolve.Object<Service1>();
      _machineContainer.Resolve.Object<Service1DependsOn2>();
    }

    [Test]
    [ExpectedException(typeof(CircularDependencyException))]
    public void Resolve_Circular_Dependency_Throws()
    {
      _machineContainer.Add<IService2, Service2DependsOn1>();
      _machineContainer.Add<IService1, Service1DependsOn2>();
      _machineContainer.ResolveObject<IService2>();
    }

    [Test]
    [ExpectedException(typeof(PendingDependencyException))]
    public void Resolve_Waiting_Dependencies_Throws()
    {
      _machineContainer.Add<IService2, Service2DependsOn1>();
      _machineContainer.ResolveObject<IService2>();
    }

    [Test]
    public void Resolve_Not_Waiting_Dependencies_Works()
    {
      _machineContainer.Add<IService2, Service2DependsOn1>();
      _machineContainer.Add<IService1, SimpleService1>();
      Assert.IsNotNull(_machineContainer.ResolveObject<IService2>());
    }

    [Test]
    public void Resolve_Singleton_Yields_Same_Instances()
    {
      _machineContainer.Add<IService1, SimpleService1>();
      IService1 service1 = _machineContainer.ResolveObject<IService1>();
      IService1 service2 = _machineContainer.ResolveObject<IService1>();
      Assert.AreEqual(service1, service2);
    }

    [Test]
    [ExpectedException(typeof(ServiceContainerException))]
    public void Resolve_And_Change_Entry_Throws()
    {
      _machineContainer.Add<IService1, SimpleService1>();
      IService1 service1 = _machineContainer.ResolveObject<IService1>();
      _machineContainer.Register.Type<IService1>().AsTransient();
    }

    [Test]
    public void Resolve_And_Deactivate_Change_Entry_Works()
    {
      _machineContainer.Add<IService1, SimpleService1>();
      IService1 service1 = _machineContainer.ResolveObject<IService1>();
      _machineContainer.Deactivate(service1);
      _machineContainer.Register.Type<IService1>().AsTransient();
      _machineContainer.ResolveObject<IService1>();
    }

    [Test]
    public void Resolve_Transient_Yields_Multiple_Instances()
    {
      _machineContainer.Add<IService1, SimpleService1>(LifestyleType.Transient);
      IService1 service1 = _machineContainer.ResolveObject<IService1>();
      IService1 service2 = _machineContainer.ResolveObject<IService1>();
      Assert.AreNotEqual(service1, service2);
    }

    [Test]
    public void Resolve_With_Overrides_With_Overrides_Uses_Override()
    {
      _machineContainer.Add<IService1, Service1DependsOn2>(LifestyleType.Transient);
      Assert.IsNotNull(_machineContainer.ResolveWithOverrides<IService1>(new SimpleService2()));
    }

    [Test]
    [ExpectedException(typeof(ServiceContainerException))]
    public void Deactivate_Not_Created_By_Container_Throws()
    {
      _machineContainer.Add<IService1, SimpleService1>(LifestyleType.Transient);
      _machineContainer.Deactivate(new SimpleService1());
    }

    [Test]
    public void Deactivate_First_Time_Just_Does_That()
    {
      _machineContainer.Add<IService1, SimpleService1>(LifestyleType.Transient);
      IService1 service1 = _machineContainer.ResolveObject<IService1>();
      _machineContainer.Deactivate(service1);
    }

    [Test]
    [ExpectedException(typeof(ServiceContainerException))]
    public void Deactivate_Second_Time_Throws()
    {
      _machineContainer.Add<IService1, SimpleService1>(LifestyleType.Transient);
      IService1 service1 = _machineContainer.ResolveObject<IService1>();
      _machineContainer.Deactivate(service1);
      _machineContainer.Deactivate(service1);
    }

    [Test]
    public void Deactivate_A_Disposable_Calls_Dispose()
    {
      _machineContainer.Add<IDisposableService, DisposableService>();
      IDisposableService disposable = _machineContainer.ResolveObject<IDisposableService>();
      _machineContainer.Deactivate(disposable);
      Assert.IsTrue(disposable.IsDisposed);
    }

    [Test]
    public void Deactivate_A_Singleton_Creates_New_Instance_Afterwards()
    {
      _machineContainer.Add<IService1, SimpleService1>();
      IService1 service1 = _machineContainer.ResolveObject<IService1>();
      _machineContainer.Deactivate(service1);
      IService1 service2 = _machineContainer.ResolveObject<IService1>();
      Assert.AreNotEqual(service1, service2);
    }

    [Test]
    [ExpectedException(typeof(AmbiguousServicesException))]
    public void Lots_Of_Services_With_AmbigiousDependency_Throws()
    {
      _machineContainer.Add<IService1, Service1>();
      _machineContainer.Add<Service2DependsOn1>();
      _machineContainer.Add<SimpleService1>();
      IService2 service2 = _machineContainer.ResolveObject<IService2>();
    }

    [Test]
    public void Lots_Of_Services_Works()
    {
      ContainerResolver resolve = _machineContainer.Resolve;
      _machineContainer.Add<IService1, Service1>();
      _machineContainer.Add<Service2DependsOn1>();
      _machineContainer.Add<SimpleService2>();
      IService1 service1a = _machineContainer.ResolveObject<Service1>();
      IService1 service1b = _machineContainer.ResolveObject<IService1>();
      IService2 service2a = _machineContainer.ResolveObject<Service2DependsOn1>();
      IService2 service2b = _machineContainer.ResolveObject<SimpleService2>();
      List<IService2> services = new List<IService2>(resolve.All<IService2>());
      Assert.AreEqual(2, services.Count);
    }

    [Test]
    public void Resolve_Generic_Type_Works()
    {
      _machineContainer.Add<StringSomething>();
      ISomething<string> something = _machineContainer.ResolveObject<ISomething<string>>();
      Assert.IsNotNull(something);
    }

    [Test]
    public void Registering_With_Property_Settings_Will_Use_Them()
    {
      MockPropertySettings propertySettings = new MockPropertySettings();
      _machineContainer.Register.Type<Service1>().Using(propertySettings);
      IService1 service = _machineContainer.ResolveObject<IService1>();
      Assert.IsTrue(propertySettings.WasApplied);
    }

    [Test]
    public void Registering_With_Dictionary_Property_Settings_Applies_Them_Correctly()
    {
      DictionaryPropertySettings settings = new DictionaryPropertySettings();
      settings["ServiceKey"] = "Jacob";
      _machineContainer.Register.Type<Service1>().Using(settings);
      Service1 service = _machineContainer.ResolveObject<Service1>();
      Assert.AreEqual("Jacob", service.ServiceKey);
    }
    #endregion
  }
  public class MockPropertySettings : IPropertySettings
  {
    private bool _wasApplied;

    public bool WasApplied
    {
      get { return _wasApplied; }
    }

    #region IPropertySettings Members
    public void Apply(object instance)
    {
      _wasApplied = true;
    }
    #endregion
  }
  public interface ISomething<TType>
  {
    void DoSomethingTo(TType value);
  }
  public class StringSomething : ISomething<String>
  {
    #region ISomething<string> Members
    public void DoSomethingTo(string value)
    {
    }
    #endregion
  }
  public interface IExampleService
  {
  }
  public class Service1 : IService1, IExampleService
  {
    private string _serviceKey;

    public string ServiceKey
    {
      get { return _serviceKey; }
      set { _serviceKey = value; }
    }

    public void SayHello()
    {
    }
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
    public void SayHello()
    {
    }
  }
  public class SimpleService1 : IService1
  {
    public void SayHello()
    {
    }
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
  public class SimpleInterceptor : IInterceptor
  {
    private readonly List<IInvocation> _invocations = new List<IInvocation>();

    public List<IInvocation> Invocations
    {
      get { return _invocations; }
    }

    #region IInterceptor Members
    public void Intercept(IInvocation invocation)
    {
      _invocations.Add(invocation);
      invocation.Proceed();
    }
    #endregion
  }
}
