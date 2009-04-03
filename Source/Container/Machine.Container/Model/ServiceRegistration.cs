using System;
using System.Collections.Generic;

namespace Machine.Container.Model
{
  public class ServiceRegistration
  {
    private readonly Type _serviceType;

    public Type ServiceType
    {
      get { return _serviceType; }
    }

    public ServiceRegistration(Type serviceType)
    {
      _serviceType = serviceType;
    }

    public override string ToString()
    {
      return String.Format("ServiceRegistration<{0}>", _serviceType);
    }
  }
}
