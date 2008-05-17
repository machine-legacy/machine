using System;

namespace Machine.Mocks.Generation
{
  public interface IMockGenerationStrategy
  {
    bool CanGenerateMock(Type type);
    object GenerateMock(Type type, object[] constructorArguments);
  }
}