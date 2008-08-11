using System;
using System.Collections.Generic;

namespace Machine.Container.Plugins.ObjectFactories
{
  public interface IFactory<TType>
  {
    TType Create();
    void Deactivate(TType instance);
  }
}
