using System;
using System.Collections.Generic;

namespace Machine.Container.Model
{
  public class ServiceDependency : IEquatable<ServiceDependency>
  {
    private readonly Type _dependencyType;
    private readonly DependencyType _type;
    private readonly string _key;

    public Type DependencyType
    {
      get { return _dependencyType; }
    }

    public string Key
    {
      get { return _key; }
    }

    public ServiceDependency(Type dependencyType, DependencyType type, string key)
    {
      _dependencyType = dependencyType;
      _type = type;
      _key = key;
    }

    public bool Equals(ServiceDependency serviceDependency)
    {
      if (serviceDependency == null) return false;
      if (!Equals(_dependencyType, serviceDependency._dependencyType)) return false;
      if (!Equals(_type, serviceDependency._type)) return false;
      if (!Equals(_key, serviceDependency._key)) return false;
      return true;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(this, obj)) return true;
      return Equals(obj as ServiceDependency);
    }

    public override Int32 GetHashCode()
    {
      return _dependencyType.GetHashCode() ^ _type.GetHashCode() ^ _key.GetHashCode();
    }
  }
}