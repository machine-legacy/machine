using System;

namespace Machine.Container.Plugins.ObjectFactories
{
  public static class FactoryHelper
  {
    public static Type CreateFactoryType(Type valueType)
    {
      return typeof(IFactory<>).MakeGenericType(valueType);
    }
  }
}