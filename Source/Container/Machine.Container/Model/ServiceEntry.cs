using System;
using System.Collections.Generic;

namespace Machine.Container.Model
{
  public class ServiceEntry
  {
    private readonly string _key;
    private string _name;
    private Type _serviceType;
    private LifestyleType _lifestyleType;
    private long _numberOfActiveInstances;
    private readonly List<InterceptorApplication> _interceptors = new List<InterceptorApplication>();
    private IPropertySettings _propertySettings;
    private readonly ServiceEntryLock _lock;

    public string Key
    {
      get { return _key; }
    }

    public string Name
    {
      get { return _name; }
      set
      {
        AssertHasNoActiveInstances();
        _name = value;
      }
    }

    public LifestyleType LifestyleType
    {
      get { return _lifestyleType; }
      set
      {
        AssertHasNoActiveInstances();
        _lifestyleType = value;
      }
    }

    public Type ServiceType
    {
      get { return _serviceType; }
      set
      {
        AssertHasNoActiveInstances();
        _serviceType = value;
      }
    }

    public Type ConcreteType
    {
      get
      {
        if (!_serviceType.IsAbstract)
        {
          return _serviceType;
        }
        return null;
      }
    }

    public IPropertySettings PropertySettings
    {
      get { return _propertySettings; }
      set
      {
        AssertHasNoActiveInstances();
        if (_propertySettings != null)
        {
          throw new InvalidOperationException("Overwriting existing PropertySettings for " + this);
        }
        _propertySettings = value;
      }
    }

    public ServiceEntry(Type implementationType, LifestyleType lifestyleType)
      : this(implementationType, lifestyleType, String.Empty)
    {
    }

    public ServiceEntry(Type implementationType, LifestyleType lifestyleType, string key)
    {
      AssertValidTypes(implementationType);
      _serviceType = implementationType;
      _lifestyleType = lifestyleType;
      _key = key;
      _lock = ServiceEntryLockBroker.Singleton.GetLockForEntry(this);
    }

    public override string ToString()
    {
      return String.Format("Entry<{0}, {1}", this.ServiceType, _numberOfActiveInstances);
    }

    public bool IsNamed(string name)
    {
      if (String.IsNullOrEmpty(_name))
      {
        return false;
      }
      return _name.Equals(name);
    }

    public void IncrementActiveInstances()
    {
      _numberOfActiveInstances++;
    }

    public void DecrementActiveInstances()
    {
      if (_numberOfActiveInstances == 0)
      {
        throw new ServiceContainerException("Number of active instances less than 0!");
      }
      _numberOfActiveInstances--;
    }

    public bool CanHaveActivator
    {
      get { return this.LifestyleType != LifestyleType.Override; }
    }

    public void AssertIsAcceptableInstance(object instance)
    {
      if (!_serviceType.IsInstanceOfType(instance))
      {
        throw new ServiceContainerException("Instance is not instance of type: " + _serviceType);
      }
    }

    private static void AssertValidTypes(Type implementationType)
    {
    }

    private void AssertHasNoActiveInstances()
    {
      if (_numberOfActiveInstances > 0)
      {
        throw new ServiceContainerException("You may not do that when there are active instances!");
      }
    }

    public void AddInterceptor(Type interceptorType)
    {
      AssertHasNoActiveInstances();
      InterceptorApplication interceptor = new InterceptorApplication(interceptorType);
      if (!HasInterceptor(interceptor))
      {
        _interceptors.Add(interceptor);
      }
    }

    public bool HasInterceptor(Type interceptorType)
    {
      return HasInterceptor(new InterceptorApplication(interceptorType));
    }

    public bool HasInterceptor(InterceptorApplication interceptor)
    {
      return _interceptors.Contains(interceptor);
    }

    public bool HasPropertySettings()
    {
      return _propertySettings != null;
    }

    public IEnumerable<InterceptorApplication> Interceptors
    {
      get { return _interceptors; }
    }

    public bool HasInterceptorsApplied
    {
      get { return _interceptors.Count > 0; }
    }

    public ServiceEntryLock Lock
    {
      get { return _lock; }
    }
  }
}