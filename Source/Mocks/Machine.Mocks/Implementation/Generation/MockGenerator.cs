using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Mocks.Implementation.Generation;

namespace Machine.Mocks.Implementation.Generation
{
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
}