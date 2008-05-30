using System;
using System.Collections.Generic;

namespace Machine.Core.Utility
{
  public static class Enumerate
  {
    public static IEnumerable<TType> OfType<TType>(System.Collections.IEnumerable enumerable)
    {
      foreach (object value in enumerable)
      {
        if (value is TType)
        {
          yield return (TType)value;
        }
      }
    }
  }
}
