using System;
using System.Collections.Generic;

namespace Machine.Utility.ThreadPool
{
  public interface IConsumer<TType>
  {
    void Consume(TType value);
  }
}