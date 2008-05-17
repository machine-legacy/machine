using System;
using System.Collections.Generic;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using System.Linq;

namespace Machine.Mocks
{
  public class Mock
  {
    static readonly MockGenerator _mockGenerator = new MockGenerator();

    public static T Of<T>() where T : class 
    {
      return Of<T>(new object[] {});
    }

    public static T Of<T>(params object[] constructorArguments) where T : class
    {
      var mock = new Mock<T>(_mockGenerator, constructorArguments);

      return mock.Object;
    }
  }

  public class Mock<T>
  {
    T _object;

    public T Object
    { 
      get { return _object; }
    }

    public Mock(MockGenerator mockGenerator, object[] constructorArguments)
    {
      _object = (T)mockGenerator.GenerateMock(typeof(T), constructorArguments);
    }
  }

  public class MockGenerator
  {
    readonly List<IMockGenerationStrategy> _generationStrategies;

    public MockGenerator()
    {
      _generationStrategies = new List<IMockGenerationStrategy>();
      var proxyGenerators = new ProxyGenerators();

      _generationStrategies.Add(new InterfaceGenerationStrategy(proxyGenerators));
      _generationStrategies.Add(new ClassGenerationStrategy(proxyGenerators));
    }

    public object GenerateMock(Type type, object[] constructorArguments)
    {
      var generationStrategy = FindGenerationStrategy(type);

      return generationStrategy.GenerateMock(type, constructorArguments);
    }

    IMockGenerationStrategy FindGenerationStrategy(Type type)
    {
      return _generationStrategies.Where(x=>x.CanGenerateMock(type)).FirstOrDefault();
    }
  }

  public interface IMockGenerationStrategy
  {
    bool CanGenerateMock(Type type);
    object GenerateMock(Type type, object[] constructorArguments);
  }

  public class InterfaceGenerationStrategy : IMockGenerationStrategy
  {
    readonly ProxyGenerators _proxyGenerators;

    public InterfaceGenerationStrategy(ProxyGenerators proxyGenerators)
    {
      _proxyGenerators = proxyGenerators;
    }

    public bool CanGenerateMock(Type type)
    {
      return type.IsInterface;
    }

    public object GenerateMock(Type type, object[] constructorArguments)
    {
      if (constructorArguments.Length > 0)
      {
        throw MocksExceptions.MockInterfaceWithConstructorArguments(type);
      }
      var generator = _proxyGenerators.GetGenerator(type);

      return generator.CreateInterfaceProxyWithoutTarget(type, new MockInterceptor());
    }
  }

  public class ClassGenerationStrategy : IMockGenerationStrategy
  {
    readonly ProxyGenerators _proxyGenerators;

    public ClassGenerationStrategy(ProxyGenerators proxyGenerators)
    {
      _proxyGenerators = proxyGenerators;
    }

    public bool CanGenerateMock(Type type)
    {
      return type.IsClass;
    }

    public object GenerateMock(Type type, object[] constructorArguments)
    {
      var generator = _proxyGenerators.GetGenerator(type);

      return generator.CreateClassProxy(type, new MockInterceptor());
    }
  }

  public class ProxyGenerators
  {
    Dictionary<Type, ProxyGenerator> _generatorMap;


    public ProxyGenerators()
    {
      _generatorMap = new Dictionary<Type, ProxyGenerator>();
    }

    public ProxyGenerator GetGenerator(Type type)
    {
      if (!_generatorMap.ContainsKey(type))
      {
        _generatorMap[type] = new ProxyGenerator();
      }

      return _generatorMap[type];
    }
  }

  public class MockInterceptor : IInterceptor
  {
    public void Intercept(IInvocation invocation)
    {
      //invocation.Proceed();
    }
  }
}