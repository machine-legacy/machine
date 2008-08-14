using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Policy;

namespace Machine.Core.Services.Impl
{
  public class DotNetDotNet : IDotNet
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(DotNetDotNet));
    private readonly IObjectActivator _objectActivator;
    private readonly IFileSystem _fileSystem;

    public DotNetDotNet(IObjectActivator objectActivator, IFileSystem fileSystem)
    {
      _objectActivator = objectActivator;
      _fileSystem = fileSystem;
      AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += OnReflectionOnlyAssemblyResolve;
    }

    #region IDotNet Members
    public IDomain CreateDomain(string name)
    {
      Evidence evidence = AppDomain.CurrentDomain.Evidence;
      AppDomainSetup domainSetup = new AppDomainSetup();
      domainSetup.ApplicationBase = String.Empty;
      domainSetup.PrivateBinPath = String.Empty;
      domainSetup.ApplicationName = "MyDomain";
      return new DotNetDomain(AppDomain.CreateDomain(name, evidence, domainSetup));
    }

    public IAssembly LoadAssembly(string name)
    {
      if (_fileSystem.IsFile(name))
      {
        return new DotNetAssembly(Assembly.LoadFrom(name));
      }
      return new DotNetAssembly(Assembly.Load(name));
    }

    public IAssembly LoadAssemblyForReflectionOnly(string name)
    {
      return new DotNetAssembly(Assembly.ReflectionOnlyLoadFrom(name));
    }

    public IObjectActivator Activator
    {
      get { return _objectActivator; }
    }

    public IDomain ThisDomain
    {
      get { return new DotNetDomain(AppDomain.CurrentDomain); }
    }
    #endregion

    private static Assembly OnReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
    {
      _log.Info("ReflectionOnlyResolve: " + args.Name);
      Assembly assembly = Assembly.ReflectionOnlyLoad(args.Name);
      if (assembly == null)
      {
        throw new YouFoundABugException("ReflectionOnlyLoad failed: " + args.Name);
      }
      return assembly;
    }
  }
  public class DotNetDomain : IDomain
  {
    private readonly AppDomain _domain;

    public DotNetDomain(AppDomain domain)
    {
      _domain = domain;
    }

    #region IDomain Members
    public IAssembly Load(string assemblyString)
    {
      return new DotNetAssembly(_domain.Load(assemblyString));
    }

    public TType CreateAndUnwrap<TType>()
    {
      return CreateAndUnwrap<TType>(typeof(TType));
    }

    public TType CreateAndUnwrap<TType>(Type type)
    {
      return CreateAndUnwrap<TType>(type.Assembly.FullName, type.FullName);
    }

    public TType CreateAndUnwrap<TType>(string assembly, string type)
    {
      if (assembly == null) throw new ArgumentNullException("assembly");
      if (type == null) throw new ArgumentNullException("type");
      return (TType)_domain.CreateInstanceAndUnwrap(assembly, type);
    }

    public IEnumerable<IAssembly> GetAssemblies()
    {
      foreach (Assembly assembly in _domain.GetAssemblies())
      {
        yield return new DotNetAssembly(assembly);
      }
    }

    public void Unload()
    {
      AppDomain.Unload(_domain);
    }
    #endregion
  }
  public class DotNetAssembly : IAssembly
  {
    private readonly Assembly _assembly;

    public DotNetAssembly(Assembly assembly)
    {
      _assembly = assembly;
    }

    #region IAssembly Members
    public string Name
    {
      get { return _assembly.FullName; }
    }

    public Type[] GetExportedTypes()
    {
      return _assembly.GetExportedTypes();
    }
    #endregion
  }
}
