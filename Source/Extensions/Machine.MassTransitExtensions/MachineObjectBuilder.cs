using System;
using System.Collections;
using System.Collections.Generic;

using Machine.Container.Services;

using MassTransit.ServiceBus;

namespace Machine.MassTransitExtensions
{
  public class MachineObjectBuilder : IObjectBuilder
  {
    private readonly IMachineContainer _container;

    public MachineObjectBuilder(IMachineContainer container)
    {
      _container = container;
    }

    #region IObjectBuilder Members
    public T Build<T>(IDictionary arguments)
    {
      return _container.Resolve.Object<T>();
    }

    public T Build<T>(Type component) where T : class
    {
      return _container.Resolve.Object<T>();
    }

    public T Build<T>() where T : class
    {
      return _container.Resolve.Object<T>();
    }

    public object Build(Type objectType)
    {
      return _container.Resolve.Object(objectType);
    }

    public void Register<T>() where T : class
    {
      _container.Register.Type<T>().AsTransient();
    }

    public void Release<T>(T obj)
    {
      _container.Deactivate(obj);
    }

    public T GetInstance<T>(IDictionary arguments)
    {
      return _container.Resolve.ObjectWithParameters<T>(arguments);
    }
    #endregion

    #region IServiceLocator Members
    public IEnumerable<TService> GetAllInstances<TService>()
    {
      return _container.Resolve.All<TService>();
    }

    public IEnumerable<object> GetAllInstances(Type serviceType)
    {
      return _container.Resolve.All(serviceType);
    }

    public TService GetInstance<TService>(string key)
    {
      return _container.Resolve.Object<TService>();
    }

    public TService GetInstance<TService>()
    {
      return _container.Resolve.Object<TService>();
    }

    public object GetInstance(Type serviceType, string key)
    {
      return _container.Resolve.Object(serviceType);
    }

    public object GetInstance(Type serviceType)
    {
      return _container.Resolve.Object(serviceType);
    }
    #endregion

    #region IServiceProvider Members
    public object GetService(Type serviceType)
    {
      return _container.Resolve.Object(serviceType);
    }
    #endregion
  }
}