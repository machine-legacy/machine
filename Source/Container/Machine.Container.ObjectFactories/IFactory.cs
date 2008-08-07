using System;
using System.Collections.Generic;

namespace Machine.Container.ObjectFactories
{
  public interface IFactory<TType>
  {
    TType Create();
    void Deactivate(TType instance);
  }
}
