using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Machine.Mocks
{
  [Serializable]
  public class MocksException : Exception
  {
    public MocksException()
    {
    }

    public MocksException(string message) : base(message)
    {
    }

    public MocksException(string message, Exception inner) : base(message, inner)
    {
    }

    protected MocksException(
      SerializationInfo info,
      StreamingContext context) : base(info, context)
    {
    }
  }

  public static class MocksExceptions
  {
    public static MocksException MockInterfaceWithConstructorArguments(Type type)
    {
      return new MocksException(String.Format(Resources.Exception_MockInterfaceWithConstructorArguments, type.Name));
    }
  }
}
