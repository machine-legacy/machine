using System;
using System.Collections.Generic;

namespace Machine.Core.Utility
{
  public class ArrayHelpers
  {
    public static bool AreArraysEqual(Array a1, Array a2)
    {
      if (a1 == null && a2 == null) return true;
      if (a1 == null || a2 == null) return false;
      if (a1.Length != a2.Length) return false;
      for (var i = 0; i < a1.Length; ++i)
      {
        if (!Equals(a1.GetValue(i), a2.GetValue(i)))
          return false;
      }
      return true;
    }

    public static Int32 GetHashCode(Array array)
    {
      if (array == null) return 0;
      Int32 code = 0;
      for (var i = 0; i < array.Length; ++i)
      {
        object value = array.GetValue(i);
        if (value != null)
        {
          code ^= value.GetHashCode();
        }
      }
      return code;
    }
  }
}
