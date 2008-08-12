using System;

using Castle.MonoRail.Framework;
using Castle.MonoRail.Framework.Services;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.CastleMonoRail
{
  public class MachineControllerTree : IControllerTree
  {
    private readonly IControllerTree _defaultTree;
    private readonly IMachineContainer _container;
    private bool _initialized;

    public MachineControllerTree(IMachineContainer container)
    {
      _container = container;
      _defaultTree = new DefaultControllerTree();
    }

    #region IControllerTree Members
    public event EventHandler<ControllerAddedEventArgs> ControllerAdded
    {
      add { _defaultTree.ControllerAdded += value; }
      remove { _defaultTree.ControllerAdded -= value; }
    }

    public void AddController(string areaName, string controllerName, Type controller)
    {
      _defaultTree.AddController(areaName, controllerName, controller);
    }

    public Type GetController(string areaName, string controllerName)
    {
      if (!_initialized)
      {
        foreach (ServiceRegistration registration in _container.RegisteredServices)
        {
          if (IsController(registration.ServiceType))
          {
            AddController(null, GetControllerName(registration.ServiceType), registration.ServiceType);
          }
        }
        _initialized = true;
      }
      return _defaultTree.GetController(areaName, controllerName);
    }
    #endregion

    protected virtual bool IsController(Type type)
    {
      return typeof(IController).IsAssignableFrom(type);
    }

    protected virtual string GetControllerName(Type type)
    {
      string name = type.Name;
      if (name.EndsWith("Controller"))
      {
        name = name.Substring(0, name.Length - "Controller".Length);
      }
      return name;
    }
  }
}