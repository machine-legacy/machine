using System;
using System.Collections.Generic;

namespace Machine.Core.Services
{
  public interface IDotNet
  {
    IDomain CreateDomain(string name);
    IAssembly LoadAssembly(string name);
    IAssembly LoadAssemblyForReflectionOnly(string name);
    IObjectActivator Activator
    {
      get;
    }
    IDomain ThisDomain
    {
      get;
    }
  }
  public interface IDomain
  {
    IAssembly Load(string assemblyString);
    TType CreateAndUnwrap<TType>();
    TType CreateAndUnwrap<TType>(Type type);
    TType CreateAndUnwrap<TType>(string assembly, string type);
    IEnumerable<IAssembly> GetAssemblies();
    void Unload();
  }
  public interface IAssembly
  {
    string Name
    {
      get;
    }
    Type[] GetExportedTypes();
  }
}
