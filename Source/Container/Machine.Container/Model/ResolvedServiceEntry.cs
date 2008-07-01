using System;
using System.Collections.Generic;

using Machine.Container.Services;
using Machine.Container.Services.Impl;

namespace Machine.Container.Model
{
  public class ResolvedServiceEntry
  {
    private readonly ServiceEntry _entry;
    private readonly IActivator _activator;
    private readonly IObjectInstances _objectInstances;

    protected ServiceEntry ServiceEntry
    {
      get { return _entry; }
    }

    protected IActivator Activator
    {
      get { return _activator; }
    }

    public ResolvedServiceEntry(ServiceEntry entry, IActivator activator, IObjectInstances objectInstances)
    {
      _entry = entry;
      _objectInstances = objectInstances;
      _activator = activator;
    }

    public object Activate(IResolutionServices services)
    {
      object instance = _activator.Activate(services);
      _objectInstances.Remember(this, instance);
      return instance;
    }

    public void Release(IResolutionServices services, object instance)
    {
      _activator.Release(services, instance);
      _objectInstances.Release(services, instance);
    }

    public void IncrementActiveInstances()
    {
      _entry.IncrementActiveInstances();
    }

    public void DecrementActiveInstances()
    {
      _entry.DecrementActiveInstances();
    }

    public override string ToString()
    {
      return String.Format("ResolvedEntry<{0}, {1}>", _entry, _activator);
    }

    public override bool Equals(object obj)
    {
      ResolvedServiceEntry other = obj as ResolvedServiceEntry;
      if (other != null)
      {
        return other.ServiceEntry.Equals(this.ServiceEntry) && other.Activator.Equals(this.Activator);
      }
      return false;
    }

    public override Int32 GetHashCode()
    {
      return this.ServiceEntry.GetHashCode() ^ this.Activator.GetHashCode();
    }
  }
}