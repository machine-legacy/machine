using System;

namespace Machine.Mocks.Generation
{
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
}