using System;
using System.Collections.Generic;

using Castle.Core;

namespace Machine.WindsorExtensions
{
  [AttributeUsage(AttributeTargets.Class)]
  public class ProvidesServiceAttribute : LifestyleAttribute
  {
    readonly Type _serviceType;

    public Type ServiceType
    {
      get { return _serviceType; }
    }

    public ProvidesServiceAttribute(LifestyleType type, Type serviceType)
      : base(type)
    {
      _serviceType = serviceType;
    }
  }
}