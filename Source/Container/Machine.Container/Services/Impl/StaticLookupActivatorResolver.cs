using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services.Impl
{
  public class StaticLookupActivatorResolver : IActivatorResolver
  {
    #region IActivatorResolver Members
    public IActivator ResolveActivator(IResolutionServices services, ServiceEntry entry)
    {
      object value = services.Overrides.LookupOverride(entry);
      if (value == null)
      {
        return null;
      }
      return services.ActivatorFactory.CreateStaticActivator(entry, value);
    }
    #endregion
  }
}