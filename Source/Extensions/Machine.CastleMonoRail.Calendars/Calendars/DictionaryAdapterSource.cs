using Castle.Components.DictionaryAdapter;

namespace Machine.CastleMonoRail.Calendars
{
  public static class DictionaryAdapterSource
  {
    private static readonly IDictionaryAdapterFactory _dictionaryAdapterFactory = new DictionaryAdapterFactory();
    public static IDictionaryAdapterFactory DictionaryAdapterFactory
    {
      get { return _dictionaryAdapterFactory; }
    }
  }
}