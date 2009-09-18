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
    private MachineContainer _machineContainer;

    public override void Setup()
    {
      base.Setup();
      _machineContainer = new MachineContainer();
      _machineContainer.Initialize();
      _machineContainer.AddPlugin(new DisposablePlugin());
      _machineContainer.PrepareForServices();
      _machineContainer.Start();
    }

    [Test]
    public void CanResolve_DoesNot_IsFalse()
    {
      Assert.IsFalse(_machineContainer.CanResolve<IService1>());
    }

    [Test]
    public void CanResolve_Does_IsTrue()
    {
      _machineContainer.Register.Type<SimpleService1>();
      Assert.IsTrue(_machineContainer.CanResolve<IService1>());
    }

    [Test]
    public void CanResolve_With_Dependencies()
    {
      _machineContainer.Register.Type<Service1DependsOn2>();
      Assert.IsFalse(_machineContainer.CanResolve<Service1DependsOn2>());
    }

    [Test]
    public void CanResolve_DoesButNotUnderInterface_IsTrue()
    {
      _machineContainer.Register.Type<SimpleService1>();
      Assert.IsTrue(_machineContainer.CanResolve<IService1>());
    }

    [Test]
    public void Resolve_By_Interface_No_Dependencies_Resolves_Entry()
    {
      _machineContainer.Register.Type<Service1>();
      Assert.IsNotNull(_machineContainer.Resolve.Object<IService1>());
    }

    [Test]
    public void Resolve_By_Interface_Single_Dependency_Resolves_Entry()
    {
      _machineContainer.Register.Type<Service2DependsOn1>();
      _machineContainer.Register.Type<SimpleService1>();
      Assert.IsNotNull(_machineContainer.Resolve.Object<IService2>());
    }

    [Test]
    [ExpectedException(typeof(PendingDependencyException))]
    public void Resolve_Single_Dependency_Not_There_Throws()
    {
      _machineContainer.Register.Type<Service2DependsOn1>();
      _machineContainer.Resolve.Object<IService2>();
    }

    [Test]
    [ExpectedException(typeof(MissingServiceException))]
    public void Resolve_Named_Instance_That_Is_Not_In_Container_Should_Throw()
    {
      _machineContainer.Resolve.Named("Favorite");
    }

    [Test]
    public void Resolve_Named_Instance_That_Is_In_Container()
    {
      string name = "Favorite";
      _machineContainer.Register.Type<SimpleService1>().Named(name);
      Assert.IsNotNull(_machineContainer.Resolve.Named(name));
    }

    [Test]
    [ExpectedException(typeof(AmbiguousServicesException))]
    public void Resolve_Named_Instance_That_Has_Multiple_Entries_Throws()
    {
      string name = "Favorite";
      _machineContainer.Register.Type<SimpleService1>().Named(name);
      _machineContainer.Register.Type<SimpleService2>().Named(name);
      _machineContainer.Resolve.Named(name);
    }

    [Test]
    public void Resolve_Single_Dependency_There_Resolves_Instance()
    {
      _machineContainer.Register.Type<Service2DependsOn1>();
      _machineContainer.Register.Type<SimpleService1>();
      Assert.IsNotNull(_machineContainer.Resolve.Object<IService2>());
    }

    [Test]
    public void Resolve_Lazily_Single_Dependency_There_Resolves_Instance()
    {
      _machineContainer.Register.Type<Service2DependsOn1>();
      _machineContainer.Register.Type<SimpleService1>();
      Assert.IsNotNull(_machineContainer.Resolve.Object<IService2>());
    }

    [Test]
    [ExpectedException(typeof(AmbiguousServicesException))]
    public void Resolve_Lazily_Multiple_Dependencies_Throws_Ambiguous()
    {
      _machineContainer.Register.Type<Service1DependsOn2>();
      _machineContainer.Register.Type<SimpleService1>();
      _machineContainer.Resolve.Object<IService1>();
    }

    [Test]
    public void Add_Circular_Dependency_Does_Nothing()
    {
      _machineContainer.Register.Type<Service2DependsOn1>();
      _machineContainer.Register.Type<Service1DependsOn2>();
    }

    [Test]
    public void Add_Replaces_The_Old_Service()
    {
      _machineContainer.Register.Type<Service2DependsOn1>();
      _machineContainer.Register.Type<SimpleService2>();
    }

    [Test]
    public void Resolve_Weird_Overlapping_Of_Services_Throws()
    {
      _machineContainer.Register.Type<Service1>();
      _machineContainer.Register.Type<SimpleService1>();
      _machineContainer.Resolve.Object<Service1>();
      _machineContainer.Resolve.Object<SimpleService1>();
    }

    [Test]
    public void Resolve_Weird_Overlapping_Throws()
    {
      _machineContainer.Register.Type<SimpleService2>();
      _machineContainer.Register.Type<Service1>();
      _machineContainer.Register.Type<Service1DependsOn2>();
      _machineContainer.Resolve.Object<Service1>();
      _machineContainer.Resolve.Object<Service1DependsOn2>();
    }

    [Test]
    [ExpectedException(typeof(CircularDependencyException))]
    public void Resolve_Circular_Dependency_Throws()
    {
      _machineContainer.Register.Type<Service2DependsOn1>();
      _machineContainer.Register.Type<Service1DependsOn2>();
      _machineContainer.Resolve.Object<IService2>();
    }

    [Test]
    [ExpectedException(typeof(PendingDependencyException))]
    public void Resolve_Waiting_Dependencies_Throws()
    {
      _machineContainer.Register.Type<Service2DependsOn1>();
      _machineContainer.Resolve.Object<IService2>();
    }

    [Test]
    public void Resolve_Not_Waiting_Dependencies_Works()
    {
      _machineContainer.Register.Type<Service2DependsOn1>();
      _machineContainer.Register.Type<SimpleService1>();
      Assert.IsNotNull(_machineContainer.Resolve.Object<IService2>());
    }

    [Test]
    public void Resolve_Singleton_Yields_Same_Instances()
    {
      _machineContainer.Register.Type<SimpleService1>();
      IService1 service1 = _machineContainer.Resolve.Object<IService1>();
      IService1 service2 = _machineContainer.Resolve.Object<IService1>();
      Assert.AreEqual(service1, service2);
    }

    [Test]
    [ExpectedException(typeof(ServiceContainerException))]
    public void Resolve_And_Change_Entry_Throws()
    {
      _machineContainer.Register.Type<SimpleService1>();
      IService1 service1 = _machineContainer.Resolve.Object<IService1>();
      _machineContainer.Register.Type<IService1>().AsTransient();
    }

    [Test]
    public void Resolve_And_Deactivate_Change_Entry_Works()
    {
      _machineContainer.Register.Type<SimpleService1>();
      IService1 service1 = _machineContainer.Resolve.Object<IService1>();
      _machineContainer.Deactivate(service1);
      _machineContainer.Register.Type<IService1>().AsTransient();
      _machineContainer.Resolve.Object<IService1>();
    }

    [Test]
    public void Resolve_Transient_Yields_Multiple_Instances()
    {
      _machineContainer.Register.Type<SimpleService1>().AsTransient();
      IService1 service1 = _machineContainer.Resolve.Object<IService1>();
      IService1 service2 = _machineContainer.Resolve.Object<IService1>();
      Assert.AreNotEqual(service1, service2);
    }

    [Test]
    public void Resolve_With_Overrides_With_Overrides_Uses_Override()
    {
      _machineContainer.Register.Type<Service1DependsOn2>().AsTransient();
      Assert.IsNotNull(_machineContainer.Resolve.Object<IService1>(new SimpleService2()));
    }

    [Test]
    public void Resolve_With_Parameters()
    {
      _machineContainer.Register.Type<ConfiguredService>().AsTransient();
      Dictionary<string, object> arguments = new Dictionary<string, object>();
      arguments["first"] = "Jacob";
      arguments["last"] = "Lewallen";
      ConfiguredService configured = _machineContainer.Resolve.ObjectWithParameters<ConfiguredService>(arguments);
      Assert.AreEqual("Jacob", configured.First);
      Assert.AreEqual("Lewallen", configured.Last);
    }

    [Test]
    [ExpectedException(typeof(ServiceContainerException))]
    public void Deactivate_Not_Created_By_Container_Throws()
    {
      _machineContainer.Register.Type<SimpleService1>().AsTransient();
      _machineContainer.Deactivate(new SimpleService1());
    }

    [Test]
    public void Deactivate_First_Time_Just_Does_That()
    {
      _machineContainer.Register.Type<SimpleService1>().AsTransient();
      IService1 service1 = _machineContainer.Resolve.Object<IService1>();
      _machineContainer.Deactivate(service1);
    }

    [Test]
    [ExpectedException(typeof(ServiceContainerException))]
    public void Deactivate_Second_Time_Throws()
    {
      _machineContainer.Register.Type<SimpleService1>().AsTransient();
      IService1 service1 = _machineContainer.Resolve.Object<IService1>();
      _machineContainer.Deactivate(service1);
      _machineContainer.Deactivate(service1);
    }

    [Test]
    public void Deactivate_A_Disposable_Calls_Dispose()
    {
      _machineContainer.Register.Type<DisposableService>();
      IDisposableService disposable = _machineContainer.Resolve.Object<IDisposableService>();
      _machineContainer.Deactivate(disposable);
      Assert.IsTrue(disposable.IsDisposed);
    }

    [Test]
    public void Deactivate_A_Singleton_Creates_New_Instance_Afterwards()
    {
      _machineContainer.Register.Type<SimpleService1>();
      IService1 service1 = _machineContainer.Resolve.Object<IService1>();
      _machineContainer.Deactivate(service1);
      IService1 service2 = _machineContainer.Resolve.Object<IService1>();
      Assert.AreNotEqual(service1, service2);
    }

    [Test]
    [ExpectedException(typeof(AmbiguousServicesException))]
    public void Lots_Of_Services_With_AmbigiousDependency_Throws()
    {
      _machineContainer.Register.Type<Service1>();
      _machineContainer.Register.Type<Service2DependsOn1>();
      _machineContainer.Register.Type<SimpleService1>();
      IService2 service2 = _machineContainer.Resolve.Object<IService2>();
    }

    [Test]
    public void Lots_Of_Services_Works()
    {
      ContainerResolver resolve = _machineContainer.Resolve;
      _machineContainer.Register.Type<Service1>();
      _machineContainer.Register.Type<Service2DependsOn1>();
      _machineContainer.Register.Type<SimpleService2>();
      IService1 service1a = _machineContainer.Resolve.Object<Service1>();
      IService1 service1b = _machineContainer.Resolve.Object<IService1>();
      IService2 service2a = _machineContainer.Resolve.Object<Service2DependsOn1>();
      IService2 service2b = _machineContainer.Resolve.Object<SimpleService2>();
      List<IService2> services = new List<IService2>(resolve.All<IService2>());
      Assert.AreEqual(2, services.Count);
    }

    [Test]
    public void Resolve_Generic_Type_Works()
    {
      _machineContainer.Register.Type<StringSomething>();
      ISomething<string> something = _machineContainer.Resolve.Object<ISomething<string>>();
      Assert.IsNotNull(something);
    }

    [Test]
    public void Registering_With_Property_Settings_Will_Use_Them()
    {
      MockPropertySettings propertySettings = new MockPropertySettings();
      _machineContainer.Register.Type<Service1>().Using(propertySettings);
      IService1 service = _machineContainer.Resolve.Object<IService1>();
      Assert.IsTrue(propertySettings.WasApplied);
    }

    [Test]
    public void Registering_With_Dictionary_Property_Settings_Applies_Them_Correctly()
    {
      DictionaryPropertySettings settings = new DictionaryPropertySettings();
      settings["ServiceKey"] = "Jacob";
      _machineContainer.Register.Type<Service1>().Using(settings);
      Service1 service = _machineContainer.Resolve.Object<Service1>();
      Assert.AreEqual("Jacob", service.ServiceKey);
    }
  }
  public class MockPropertySettings : IPropertySettings
  {
    private bool _wasApplied;

    public bool WasApplied
    {
      get { return _wasApplied; }
    }

    public void Apply(object instance)
    {
      _wasApplied = true;
    }
  }
  public interface ISomething<TType>
  {
    void DoSomethingTo(TType value);
  }
  public class StringSomething : ISomething<String>
  {
    public void DoSomethingTo(string value)
    {
    }
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
  public class ConfiguredService
  {
    public string First { get; set; }
    public string Last { get; set; }

    public ConfiguredService(string first, string last)
    {
      First = first;
      Last = last;
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
