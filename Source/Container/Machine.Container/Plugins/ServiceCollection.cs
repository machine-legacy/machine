using System;
using System.Collections.Generic;

namespace Machine.Container.Plugins
{
  public class ServiceCollection : IServiceCollection
  {
    #region IServiceCollection Members
    public void RegisterServices(ContainerRegisterer register)
    {
      foreach (Type type in TransientServices())
      {
        register.Type(type).AsTransient();
      }
      foreach (Type type in SingletonServices())
      {
        register.Type(type).AsSingleton();
      }
      foreach (Type type in AllServices())
      {
        register.Type(type);
      }
    }
    #endregion

    protected virtual IEnumerable<Type> TransientServices()
    {
      return new List<Type>();
    }

    protected virtual IEnumerable<Type> SingletonServices()
    {
      return new List<Type>();
    }

    protected virtual IEnumerable<Type> AllServices()
    {
      return new List<Type>();
    }
  }
}