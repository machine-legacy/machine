using System;

namespace Machine.Mocks.Implementation.Generation
{
  public interface IMockGenerationStrategy
  {
    bool CanGenerateMock(Type type);
    object GenerateMock(Type type, object[] constructorArguments);
  }
}