using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Machine.Mocks.Properties;

namespace Machine.Mocks.Exceptions
{
  [Serializable]
  public class MockVerificationException : Exception
  {
    //
    // For guidelines regarding the creation of new exception types, see
    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
    // and
    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
    //

    public MockVerificationException()
    {
    }

    public MockVerificationException(string message) : base(message)
    {
    }

    public MockVerificationException(string message, Exception inner) : base(message, inner)
    {
    }

    protected MockVerificationException(
      SerializationInfo info,
      StreamingContext context) : base(info, context)
    {
    }
  }

  [Serializable]
  public class MockUsageException : Exception
  {
    public MockUsageException()
    {
    }

    public MockUsageException(string message) : base(message)
    {
    }

    public MockUsageException(string message, Exception inner) : base(message, inner)
    {
    }

    protected MockUsageException(
      SerializationInfo info,
      StreamingContext context) : base(info, context)
    {
    }
  }

  public static class MocksExceptions
  {
    public static MockUsageException MockInterfaceWithConstructorArguments(Type type)
    {
      return new MockUsageException(String.Format(Resources.Exception_MockInterfaceWithConstructorArguments, type.Name));
    }
  }
}