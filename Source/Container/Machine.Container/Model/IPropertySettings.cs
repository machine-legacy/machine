using System;
using System.Collections.Generic;
using System.Reflection;

namespace Machine.Container.Model
{
  public interface IPropertySettings
  {
    void Apply(object instance);
  }
  public abstract class AbstractPropertySettings : IPropertySettings
  {
    #region IPropertySettings Members
    public void Apply(object instance)
    {
      foreach (string propertyName in this.PropertyNames)
      {
        MethodInfo setter = GetPropertySetter(instance, propertyName);
        setter.Invoke(instance, new object[] { GetPropertyValue(propertyName) });
      }
    }
    #endregion

    protected virtual MethodInfo GetPropertySetter(object instance, string propertyName)
    {
      foreach (PropertyInfo propertyInfo in instance.GetType().GetProperties())
      {
        if (propertyInfo.Name == propertyName)
        {
          return propertyInfo.GetSetMethod();
        }
      }
      throw new ServiceContainerException("Unable to set property " + propertyName + " on " + instance);
    }
    protected abstract IEnumerable<string> PropertyNames
    {
      get;
    }

    protected abstract object GetPropertyValue(string name);

  }
  public class DictionaryPropertySettings : AbstractPropertySettings
  {
    private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

    protected override IEnumerable<string> PropertyNames
    {
      get { return _values.Keys; }
    }

    protected override object GetPropertyValue(string name)
    {
      return _values[name];
    }

    public object this[string name]
    {
      get { return _values[name]; }
      set { _values[name] = value; }
    }
  }
}