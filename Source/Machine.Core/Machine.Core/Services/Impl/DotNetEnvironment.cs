using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Machine.Core.Services.Impl
{
  public class DotNetEnvironment : IEnvironment
  {
    #region IEnvironment Members
    public string CurrentDirectory
    {
      get { return Environment.CurrentDirectory; }
    }

    public string CurrentlyExecutingAssemblyPath
    {
      get { return Assembly.GetExecutingAssembly().Location; }
    }

    public string CallingAssemblyPath
    {
      get { return Assembly.GetCallingAssembly().Location; }
    }

    public string ApplicationDomainBaseDirectory
    {
      get { return AppDomain.CurrentDomain.BaseDirectory; }
    }

    public string ApplicationBinaryDirectory
    {
      get
      {
        string directory = ApplicationDomainBaseDirectory;
        if (Directory.Exists(Path.Combine(directory, @"Bin")))
        {
          return Path.Combine(directory, @"Bin");
        }
        return directory;
      }
    }
    #endregion
  }
}
