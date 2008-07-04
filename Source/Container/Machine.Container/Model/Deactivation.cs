using System;
using System.Collections.Generic;

namespace Machine.Container.Model
{
  public class Deactivation
  {
    private readonly object _instance;

    public object Instance
    {
      get { return _instance; }
    }

    public Deactivation(object instance)
    {
      _instance = instance;
    }
  }
}