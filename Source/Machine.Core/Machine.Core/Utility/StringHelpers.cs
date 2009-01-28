using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Core.Utility
{
  public static class StringHelpers
  {
    public static string ToUnderscoreDelimited(string source)
    {
      return ToDelimited(source, '_');
    }

    public static string ToDelimited(string source, char delimiter)
    {
      StringBuilder sb = new StringBuilder();
      bool previousWasDelimiter = true;
      foreach (char c in source)
      {
        if (Char.IsUpper(c) && !previousWasDelimiter)
        {
          sb.Append(delimiter);
        }
        sb.Append(c);
        previousWasDelimiter = (c == delimiter);
      }
      return sb.ToString().ToLower();
    }
  }
}
