using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

    public static IEnumerable<TType> AndChange<TType>(IEnumerable<TType> original)
    {
      return new ReadOnlyCollection<TType>(new List<TType>(original));
    }
  }
}
