using System;
using System.Collections.Generic;

namespace Machine.Container.Model
{
  public class SupportedFeature
  {
    private readonly string _name;

    protected SupportedFeature(string name)
    {
      _name = name;
    }

    public static readonly SupportedFeature Interceptors = new SupportedFeature("Interception and Proxying");

    public override string ToString()
    {
      return _name;
    }
  }
}
