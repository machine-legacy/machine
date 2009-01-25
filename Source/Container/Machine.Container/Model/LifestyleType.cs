using System;
using System.Collections.Generic;

namespace Machine.Container.Model
{
  public enum LifestyleType
  {
    Singleton,
    Transient,
    PerWebRequest,
    PerThread,
    Override
  }

  public abstract class LifestyleAttribute : Attribute
  {
    public abstract LifestyleType Lifestyle { get; }
  }
  
  [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
  public class SingletonAttribute : LifestyleAttribute
  {
    public override LifestyleType Lifestyle
    {
      get { return LifestyleType.Singleton; }
    }
  }
  
  [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
  public class TransientAttribute : LifestyleAttribute
  {
    public override LifestyleType Lifestyle
    {
      get { return LifestyleType.Transient; }
    }
  }
  
  [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
  public class PerWebRequestAttribute : LifestyleAttribute
  {
    public override LifestyleType Lifestyle
    {
      get { return LifestyleType.PerWebRequest; }
    }
  }
}
