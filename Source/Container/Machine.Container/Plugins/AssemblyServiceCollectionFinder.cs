using System;
using System.Collections.Generic;
using System.Reflection;

namespace Machine.Container.Plugins
{
  public class AssemblyServiceCollectionFinder
  {
    readonly List<Assembly> _assemblies = new List<Assembly>();
    readonly string _rootPath;

    public AssemblyServiceCollectionFinder(string rootPath)
    {
      _rootPath = rootPath;
    }

    public AssemblyServiceCollectionFinder()
    {
      _rootPath = System.IO.Path.GetDirectoryName(typeof(AssemblyServiceCollectionFinder).Assembly.Location);
    }

    public void AddAssembly(string assemblyName)
    {
      string path = System.IO.Path.Combine(_rootPath, assemblyName);
      Assembly assembly = Assembly.LoadFrom(path);
      _assemblies.Add(assembly);
    }

    public IEnumerable<T> Create<T>()
    {
      foreach (Type type in EnumerateTypesOf<T>())
      {
        yield return (T)Activator.CreateInstance(type);
      }
    }

    private IEnumerable<Type> EnumerateTypesOf<T>()
    {
      foreach (Assembly assembly in _assemblies)
      {
        foreach (Type type in assembly.GetExportedTypes())
        {
          if (typeof (T).IsAssignableFrom(type))
          {
            yield return type;
          }
        }
      }
    }
  }

  public class ImportedAssemblyServices : IServiceCollection
  {
    static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ImportedAssemblyServices));
    readonly AssemblyServiceCollectionFinder _finder;
    readonly List<string> _assemblies = new List<string>();

    public ImportedAssemblyServices(AssemblyServiceCollectionFinder finder, params string[] assemblies)
    {
      _finder = finder;
      _assemblies.AddRange(assemblies);
    }

    public ImportedAssemblyServices(params string[] assemblies)
      : this(new AssemblyServiceCollectionFinder(), assemblies)
    {
    }

    public void AddAssembly(string assembly)
    {
      _assemblies.Add(assembly);
    }

    public void RegisterServices(ContainerRegisterer register)
    {
      foreach (string path in _assemblies)
      {
        _finder.AddAssembly(path);
      }

      foreach (IServiceCollection collection in _finder.Create<IServiceCollection>())
      {
        _log.Info("Importing " + collection);
        collection.RegisterServices(register);
      }
    }
  }
}