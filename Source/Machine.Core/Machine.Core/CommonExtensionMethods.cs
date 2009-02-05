using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Core
{
  public static class CommonExtensionMethods
  {
    public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
      foreach (var item in enumerable)
      {
        action(item);
      }
    }

    public static bool ElementsEqualInOrder<T>(this IEnumerable<T> left, IEnumerable<T> right)
    {
      var rightEnumerator = right.GetEnumerator();

      foreach (var element in left)
      {
        if (!rightEnumerator.MoveNext()) return false;
        
        if (!element.Equals(rightEnumerator.Current)) return false;
      }

      if (rightEnumerator.MoveNext()) return false;

      return true;
    }
  }
}