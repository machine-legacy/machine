using System;
using System.Reflection;

namespace Machine.Container.ObjectFactories
{
  public class FactoryProxy
  {
    private readonly Type _factoryType;
    private readonly object _target;
    private readonly MethodInfo _create;
    private readonly MethodInfo _deactivate;

    public FactoryProxy(Type factoryType, object target)
    {
      _factoryType = factoryType;
      _target = target;
      _create = _factoryType.GetMethod("Create");
      _deactivate = _factoryType.GetMethod("Deactivate");
    }

    public object Create()
    {
      return _create.Invoke(_target, new object[0]);
    }

    public void Deactivate(object instance)
    {
      _deactivate.Invoke(_target, new object[] { instance});
    }
  }
}