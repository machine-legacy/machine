using System;
using System.Collections.Generic;

namespace Machine.Container.Model
{
  public class InterceptorApplication
  {
    private readonly Type _interceptorType;

    public InterceptorApplication(Type interceptorType)
    {
      _interceptorType = interceptorType;
    }

    public Type InterceptorType
    {
      get { return _interceptorType; }
    }

    public override bool Equals(object obj)
    {
      InterceptorApplication interceptor = obj as InterceptorApplication;
      if (interceptor != null)
      {
        return interceptor.InterceptorType.Equals(this.InterceptorType);
      }
      return false;
    }

    public override Int32 GetHashCode()
    {
      return this.InterceptorType.GetHashCode();
    }
  }
}