using System;
using System.Collections.Generic;

namespace Machine.Core.Services
{
  public interface INamer
  {
    string ToCamelCase(string source);
    string ToLowerCamelCase(string source);
    string ToUnderscoreDelimited(string source);
    string ToDashDelimited(string source);
    string ToDelimited(string source, char delimiter);
    string MakeRandomName();
  }
}
