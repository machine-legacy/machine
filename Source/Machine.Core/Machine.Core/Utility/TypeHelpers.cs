using System;
using System.Collections.Generic;

namespace Machine.Core.Utility
{
  public static class TypeHelpers
  {
    public static bool IsNullableType(this Type type)
    {
      return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
    }
  }
}
