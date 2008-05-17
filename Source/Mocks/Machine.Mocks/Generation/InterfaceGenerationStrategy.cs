using System;
using Machine.Mocks.Generation;

namespace Machine.Mocks.Generation
{
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
}