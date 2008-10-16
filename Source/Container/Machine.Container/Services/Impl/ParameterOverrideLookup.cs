using System;

using Machine.Container.Model;

namespace Machine.Container.Services.Impl
{
  public class ParameterOverrideLookup : IOverrideLookup
  {
    private readonly System.Collections.IDictionary _parameters;

    public ParameterOverrideLookup(System.Collections.IDictionary parameters)
    {
      _parameters = parameters;
    }

    #region IOverrideLookup Members
    public object LookupOverride(ServiceEntry entry)
    {
      if (_parameters.Contains(entry.Key))
      {
        return _parameters[entry.Key];
      }
      return null;
    }
    #endregion
  }
}