using System;
using System.Collections.Generic;

using Machine.Container.Services;

namespace Machine.Container.Model
{
  public class ResolvedServiceEntry
  {
    private readonly ServiceEntry _serviceEntry;
    private readonly IActivator _activator;

    public ServiceEntry ServiceEntry
    {
      get { return _serviceEntry; }
    }

    public IActivator Activator
    {
      get { return _activator; }
    }

    public ResolvedServiceEntry(ServiceEntry serviceEntry, IActivator activator)
    {
      _serviceEntry = serviceEntry;
      _activator = activator;
    }

    public override string ToString()
    {
      return String.Format("ResolvedEntry<{0}, {1}>", _serviceEntry, _activator);
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