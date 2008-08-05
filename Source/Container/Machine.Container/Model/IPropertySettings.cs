using System;
using System.Collections.Generic;

namespace Machine.Container.Model
{
  public interface IPropertySettings
  {
    void Apply(object instance);
  }
}