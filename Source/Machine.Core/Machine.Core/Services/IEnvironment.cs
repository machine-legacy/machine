using System;
using System.Collections.Generic;

namespace Machine.Core.Services
{
  public interface IEnvironment
  {
    string CurrentDirectory
    {
      get;
    }

    string CurrentlyExecutingAssemblyPath
    {
      get;
    }

    string CallingAssemblyPath
    {
      get;
    }

    string ApplicationDomainBaseDirectory
    {
      get;
    }

    string ApplicationBinaryDirectory
    {
      get;
    }
  }
}
