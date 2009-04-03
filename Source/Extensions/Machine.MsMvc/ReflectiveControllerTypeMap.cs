using System;
using System.Collections.Generic;
using System.Web.Mvc;

using Machine.Container;
using Machine.Container.Model;

namespace Machine.MsMvc
{
  public class ReflectiveControllerTypeMap : IControllerTypeMap
  {
    private readonly IMachineContainer _container;
    private Dictionary<string, Type> _map;

    public ReflectiveControllerTypeMap(IMachineContainer container)
    {
      _container = container;
    }

    #region IControllerTypeMap Members
    public Type LookupControllerType(string controllerName)
    {
      if (_map == null)
      {
        _map = CreateMap();
      }
      controllerName = controllerName.ToLowerInvariant();
      if (!_map.ContainsKey(controllerName ))
      {
        throw new KeyNotFoundException("No controller named: " + controllerName);
      }
      return _map[controllerName];
    }
    #endregion

    private Dictionary<string, Type> CreateMap()
    {
      Dictionary<string, Type> map = new Dictionary<string, Type>();
      foreach (ServiceRegistration registration in _container.RegisteredServices)
      {
        if (IsController(registration.ServiceType))
        {
          var name = GetControllerName(registration.ServiceType);

          if (map.ContainsKey(name))
          {
            throw new Exception(String.Format(
@"Cannot add controller {0} because a similarly named controller {1} already exists.
We currently only look the class name in a case insensitive manner at this time.", registration.ServiceType, map[name]));
          }
          map[name] = registration.ServiceType;
        }
      }
      return map;
    }

    protected virtual bool IsController(Type type)
    {
      return typeof(IController).IsAssignableFrom(type);
    }

    protected virtual string GetControllerName(Type type)
    {
      string name = type.Name;
      if (name.EndsWith("Controller"))
      {
        name = name.Substring(0, name.Length - "Controller".Length).ToLowerInvariant();
      }
      return name;
    }
  }
}
