using Castle.Core.Interceptor;

using Machine.Container.DynamicProxySupport;
using Machine.Container.Plugins.Disposition;
using Machine.Container.Services.Impl;

using NUnit.Framework;

namespace Machine.Container
{
  [TestFixture]
  [Ignore]
  public class MachineContainerProxyingTests : MachineContainerTestsFixture
  {
    private MachineContainer _machineContainer;

    public override void Setup()
    {
      base.Setup();
      _machineContainer = new MachineContainer();
      _machineContainer.Initialize();
      _machineContainer.AddPlugin(new DisposablePlugin());
      _machineContainer.AddPlugin(new DynamicProxyPlugin());
      _machineContainer.PrepareForServices();
      _machineContainer.Start();
    }

    [Test]
    [ExpectedException(typeof(ServiceContainerException))]
    public void Intercept_When_Not_Installed_Throws()
    {
      _machineContainer = new MachineContainer();
      _machineContainer.Initialize();
      _machineContainer.AddPlugin(new DisposablePlugin());
      _machineContainer.PrepareForServices();
      _machineContainer.Start();
      _machineContainer.Register.Type<SimpleService1>().Intercept<SimpleInterceptor>();
    }

    [Test]
    [ExpectedException(typeof(DynamicProxyException))]
    public void Intercept_When_No_Service_Fails()
    {
      _machineContainer.Register.Type<SimpleService1>().Intercept<SimpleInterceptor>();
      _machineContainer.Resolve.Object<IService1>();
    }

    [Test]
    public void Intercept_Creates_Instance_That_Is_Proxied()
    {
      _machineContainer.Register.Type<SimpleInterceptor>();
      _machineContainer.Register.Type<SimpleService1>().Intercept<SimpleInterceptor>();
      IService1 service1 = _machineContainer.Resolve.Object<IService1>();
      Assert.IsTrue(typeof(IProxyTargetAccessor).IsInstanceOfType(service1));
      service1.SayHello();
    }

    [Test]
    public void Intercept_Call_Method_Calls_Proxy()
    {
      _machineContainer.Register.Type<SimpleInterceptor>();
      _machineContainer.Register.Type<SimpleService1>().Intercept<SimpleInterceptor>();
      IService1 service1 = _machineContainer.Resolve.Object<IService1>();
      service1.SayHello();
      IProxyTargetAccessor proxyTargetAccessor = (IProxyTargetAccessor)service1;
      SimpleInterceptor interceptor = (SimpleInterceptor)proxyTargetAccessor.GetInterceptors()[0];
      Assert.AreEqual(1, interceptor.Invocations.Count);
    }

    [Test]
    public void No_Intercept_Creates_Instance_That_Is_Not_Proxied()
    {
      _machineContainer.Register.Type<SimpleService1>();
      IService1 service1 = _machineContainer.Resolve.Object<IService1>();
      Assert.IsFalse(typeof(IProxyTargetAccessor).IsInstanceOfType(service1));
    }
  }
}