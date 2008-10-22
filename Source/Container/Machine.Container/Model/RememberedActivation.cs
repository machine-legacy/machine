using System;
using System.Collections.Generic;

using Machine.Container.Services;

namespace Machine.Container.Model
{
  public class RememberedActivation
  {
    public ResolvedServiceEntry ResolvedEntry
    {
      get; private set;
    }

    protected Activation Activation
    {
      get; private set;
    }

    public RememberedActivation(ResolvedServiceEntry resolvedEntry, Activation activation)
    {
      this.ResolvedEntry = resolvedEntry;
      this.Activation = activation;
    }

    public void Deactivate(IResolutionServices services)
    {
      this.Activation.Deactivate(services);
    }

    public override bool Equals(object obj)
    {
      RememberedActivation other = obj as RememberedActivation;
      if (other != null)
      {
        return other.ResolvedEntry.Equals(this.ResolvedEntry) && other.Activation.Equals(this.Activation);
      }
      return false;
    }

    public override Int32 GetHashCode()
    {
      return this.ResolvedEntry.GetHashCode() ^ this.Activation.GetHashCode();
    }
  }
}