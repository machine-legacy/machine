using System;
using System.Collections.Generic;

namespace Machine.Container.Model
{
  public class Activation
  {
    private readonly ServiceEntry _entry;
    private readonly object _instance;
    private readonly bool _isBrandNew;
    private readonly Int32 _ownerThread;

    public ServiceEntry Entry
    {
      get { return _entry; }
    }

    public object Instance
    {
      get { return _instance; }
    }

    public bool IsBrandNew
    {
      get { return _isBrandNew; }
    }

    public Activation(ServiceEntry entry, object instance)
     : this(entry, instance, false)
    {
    }

    public Activation(Activation activation)
     : this(activation.Entry, activation.Instance, false)
    {
    }

    public Activation(ServiceEntry entry, object instance, bool isBrandNew)
    {
      _entry = entry;
      _instance = instance;
      _isBrandNew = isBrandNew;
      _entry.AssertIsAcceptableInstance(_instance);
      _ownerThread = System.Threading.Thread.CurrentThread.ManagedThreadId;
    }

    public override bool Equals(object obj)
    {
      Activation activation = obj as Activation;
      if (activation != null)
      {
        return this.Entry.Equals(activation.Entry) && this.Instance.Equals(activation.Instance);
      }
      return false;
    }

    public override Int32 GetHashCode()
    {
      return this.Entry.GetHashCode() ^ this.Instance.GetHashCode();
    }
  }
}
