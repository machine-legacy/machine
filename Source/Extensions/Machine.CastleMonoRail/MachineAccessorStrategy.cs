using System;

using Castle.Core;
using Castle.MonoRail.Framework;

using Machine.Container;

namespace Machine.CastleMonoRail
{
  public class MachineAccessorStrategy : ServiceProviderLocator.IAccessorStrategy
  {
    private readonly IServiceProviderEx _serviceProvider;

    class ServiceProviderEx : IServiceProviderEx
    {
      private readonly IMachineContainer _container;

      public ServiceProviderEx(IMachineContainer container)
      {
        _container = container;
      }

      #region IServiceProvider Members
      public object GetService(Type serviceType)
      {
        if (!_container.CanResolve(serviceType))
        {
          return null;
        }
        return _container.Resolve.Object(serviceType);
      }
      #endregion

      #region IServiceProviderEx Members
      public T GetService<T>() where T : class
      {
        return (T)GetService(typeof(T));
      }
      #endregion
    }

    public MachineAccessorStrategy(IMachineContainer container)
    {
      _serviceProvider = new ServiceProviderEx(container);
    }

    #region IAccessorStrategy Members
    public IServiceProviderEx LocateProvider()
    {
      return _serviceProvider;
    }
    #endregion
  }
}