using System;

namespace Machine.Container.Model
{
  public class ServiceEntry
  {
    #region Member Data
    private Type _serviceType;
    private Type _implementationType;
    private LifestyleType _lifestyleType;
    private long _numberOfActiveInstances;
    #endregion

    #region Properties
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
        AssertValidTypes(value, _implementationType);
        _serviceType = value;
      }
    }

    public Type ImplementationType
    {
      get { return _implementationType; }
      set
      {
        AssertHasNoActiveInstances();
        AssertValidTypes(_serviceType, value);
        _implementationType = value;
      }
    }

    public Type ConcreteType
    {
      get
      {
        if (_implementationType != null && !_implementationType.IsAbstract)
        {
          return _implementationType;
        }
        if (!_serviceType.IsAbstract)
        {
          return _serviceType;
        }
        return null;
      }
    }
    #endregion

    #region ServiceEntry()
    public ServiceEntry(Type serviceType, Type implementationType, LifestyleType lifestyleType)
    {
      AssertValidTypes(serviceType, implementationType);
      _serviceType = serviceType;
      _implementationType = implementationType;
      _lifestyleType = lifestyleType;
    }
    #endregion

    #region Methods
    public override string ToString()
    {
      return String.Format("Entry<{0}, {1}, {2}>", this.ServiceType, this.ImplementationType, _numberOfActiveInstances);
    }
    #endregion

    public void IncrementActiveInstances()
    {
      // Protected by ObjectInstances lock
      _numberOfActiveInstances++;
    }

    public void DecrementActiveInstances()
    {
      // Protected by ObjectInstances lock
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

    private static void AssertValidTypes(Type serviceType, Type implementationType)
    {
      if (!serviceType.IsAssignableFrom(implementationType))
      {
        throw new ServiceContainerException("Service Type should be assignable from Implementation Type!");
      }
    }

    private void AssertHasNoActiveInstances()
    {
      if (_numberOfActiveInstances > 0)
      {
        throw new ServiceContainerException("You may not do that when there are active instances!");
      }
    }

    public ServiceEntryLock Lock
    {
      get { return ServiceEntryLockBroker.Singleton.GetLockForEntry(this); }
    }
  }
}