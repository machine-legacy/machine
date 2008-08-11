using System;

namespace Machine.Container.Plugins.ObjectFactories
{
  public static class FactoryHelper
  {
    public static Type CreateFactoryType(Type valueType)
    {
      return typeof(IFactory<>).MakeGenericType(valueType);
    }

    public static bool IsFactoryType(Type type)
    {
      if (type.IsGenericType)
      {
        if (type.GetGenericTypeDefinition() == typeof(IFactory<>))
        {
          return true;
        }
      }
      return false;
    }
  }
}