using Machine.Container.Model;

namespace Machine.Container.Services.Impl
{
  public class StaticOverrideLookup : IOverrideLookup
  {
    #region Member Data
    private readonly object[] _objects;
    #endregion

    #region StaticOverrideLookup()
    public StaticOverrideLookup(object[] objects)
    {
      _objects = objects;
    }
    #endregion

    #region IOverrideLookup Members
    public object LookupOverride(ServiceEntry entry)
    {
      foreach (object value in _objects)
      {
        if (entry.ServiceType.IsAssignableFrom(value.GetType()))
        {
          return value;
        }
      }
      return null;
    }
    #endregion
  }
}