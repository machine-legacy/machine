using System;
using System.Collections.Generic;
using System.Web;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Lifestyles
{
  public class WebRequestLifestyle : TransientLifestyle
  {
    private readonly AspDotNet _aspDotNet;

    public WebRequestLifestyle(IActivatorFactory activatorFactory, ServiceEntry serviceEntry)
      : base(activatorFactory, serviceEntry)
    {
      _aspDotNet = new AspDotNet();
    }

    public override Activation Activate(IResolutionServices services)
    {
      string key = MakeKey();
      Activation activation = _aspDotNet.Get<Activation>(key);
      if (activation == null)
      {
        activation = base.Activate(services);
        Activation notBrandNew = new Activation(activation);
        _aspDotNet.Set(key, notBrandNew);
      }
      return activation;
    }

    public override void Deactivate(IResolutionServices services, object instance)
    {
      _aspDotNet.Remove(MakeKey());
      base.Deactivate(services, instance);
    }

    private string MakeKey()
    {
      return "WebRequestLifestyle-" + this.Entry.GetHashCode();
    }
  }
  public class AspDotNet
  {
    private static HttpContext Current
    {
      get
      {
        HttpContext current = HttpContext.Current;
        if (current == null)
        {
          throw new InvalidOperationException("Attempting to use WebRequestLifestyle outside of ASP.NET");
        }
        return current;
      }
    }
    
    public T Get<T>(string key)
    {
      return (T)Current.Items[key];
    }

    public void Set<T>(string key, T value)
    {
      Current.Items[key] = value;
    }

    public void Remove(string key)
    {
      Current.Items.Remove(key);
    }
  }
}
