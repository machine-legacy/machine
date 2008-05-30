namespace Machine.Migrations.Builders
{
  public static class SchemaUtils
  {
    public static string Normalize(string content)
    {
      return content.ToUpper().
        Replace(".", "_").
        Replace("[", "").
        Replace("]", "").
        Replace("`", "");
    }
  }
}