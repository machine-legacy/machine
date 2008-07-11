using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Machine.Core
{
  [Serializable]
  [CoverageExclude]
  public class YouFoundABugException : Exception
  {
    public YouFoundABugException()
    {
    }

    public YouFoundABugException(string message)
     : base(message)
    {
    }

    public YouFoundABugException(string message, Exception innerException)
     : base(message, innerException)
    {
    }

    #if !SILVERLIGHT
    protected YouFoundABugException(SerializationInfo info, StreamingContext context)
     : base(info, context)
    {
    }
    #endif
  }
}
