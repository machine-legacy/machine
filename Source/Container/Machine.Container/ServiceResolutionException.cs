using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Machine.Container
{
  [Serializable]
  [CoverageExclude]
  public class ServiceContainerException : Exception
  {
    public ServiceContainerException()
    {
    }

    public ServiceContainerException(string message) : base(message)
    {
    }

    public ServiceContainerException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #if !SILVERLIGHT
    protected ServiceContainerException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    #endif
  }

  [Serializable]
  [CoverageExclude]
  public class ServiceResolutionException : ServiceContainerException
  {
    public ServiceResolutionException()
    {
    }

    public ServiceResolutionException(string message) : base(message)
    {
    }

    public ServiceResolutionException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #if !SILVERLIGHT
    protected ServiceResolutionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    #endif
  }

  [Serializable]
  [CoverageExclude]
  public class YouFoundABugException : ServiceContainerException
  {
    public YouFoundABugException()
    {
    }

    public YouFoundABugException(string message) : base(message)
    {
    }

    public YouFoundABugException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #if !SILVERLIGHT
    protected YouFoundABugException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    #endif
  }

  [Serializable]
  [CoverageExclude]
  public class CircularDependencyException : ServiceContainerException
  {
    public CircularDependencyException()
    {
    }

    public CircularDependencyException(string message) : base(message)
    {
    }

    public CircularDependencyException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #if !SILVERLIGHT
    protected CircularDependencyException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    #endif
  }

  [Serializable]
  [CoverageExclude]
  public class PendingDependencyException : ServiceContainerException
  {
    public PendingDependencyException()
    {
    }

    public PendingDependencyException(string message) : base(message)
    {
    }

    public PendingDependencyException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #if !SILVERLIGHT
    protected PendingDependencyException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    #endif
  }

  [Serializable]
  [CoverageExclude]
  public class MissingServiceException : ServiceContainerException
  {
    public MissingServiceException()
    {
    }

    public MissingServiceException(string message) : base(message)
    {
    }

    public MissingServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #if !SILVERLIGHT
    protected MissingServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    #endif
  }

  [Serializable]
  [CoverageExclude]
  public class AmbiguousServicesException : ServiceContainerException
  {
    public AmbiguousServicesException()
    {
    }

    public AmbiguousServicesException(string message) : base(message)
    {
    }

    public AmbiguousServicesException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #if !SILVERLIGHT
    protected AmbiguousServicesException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    #endif
  }
}