using System;
using System.Collections.Generic;
using System.Reflection;

using log4net.Appender;
using Machine.Testing.AutoMocking;
using Machine.Container.Model;

using NUnit.Framework;
using Rhino.Mocks;

namespace Machine.Container
{
  public class MachineContainerTestsFixture
  {
    protected static bool _loggingInitialized;
    protected MockRepository _mocks;
    protected AutoMockingContainer _container;

    [SetUp]
    public virtual void Setup()
    {
      if (!_loggingInitialized)
      {
        OutputDebugStringAppender appender = new OutputDebugStringAppender();
        appender.Layout = new log4net.Layout.PatternLayout("%-5p %c{1} %m");
        log4net.Config.BasicConfigurator.Configure(appender);
        _loggingInitialized = true;
      }
      _mocks = new MockRepository();
      _container = new AutoMockingContainer(_mocks);
      _container.Initialize();
      _container.PrepareForServices();
      _container.Start();
    }

    public T Create<T>()
    {
      return _container.Resolve.New<T>();
    }

    public T Get<T>() where T : class
    {
      return _container.Get<T>();
    }

    protected static ConstructorCandidate CreateCandidate(Type type, params Type[] parameterTypes)
    {
      ConstructorInfo ctor = type.GetConstructor(parameterTypes);
      ConstructorCandidate constructorCandidate = new ConstructorCandidate(ctor);
      foreach (Type parameterType in parameterTypes)
      {
        constructorCandidate.AddParameterDependency(new ServiceDependency(parameterType, DependencyType.Constructor, parameterType.Name));
      }
      return constructorCandidate;
    }
  }
}
