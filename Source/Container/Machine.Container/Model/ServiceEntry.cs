using System;
using System.Collections.Generic;

using Machine.Container.Services;

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
        _serviceType = value;
      }
    }

    public Type ImplementationType
    {
      get { return _implementationType; }
      set
      {
        AssertHasNoActiveInstances();
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
      _numberOfActiveInstances++;
    }

    public void DecrementActiveInstances()
    {
      _numberOfActiveInstances--;
    }

    private void AssertHasNoActiveInstances()
    {
      if (_numberOfActiveInstances > 0)
      {
        throw new ServiceContainerException("You may not do that when there are active instances!");
      }
    }
  }
}