namespace Machine.Migrations.Builders
{
  public static class Columns
  {
    public static PrimaryKeyBuilder PrimaryKey<T>(string name)
    {
      return new PrimaryKeyBuilder(name, typeof(T));
    }

    public static ForeignKeyBuilder ForeignKey(string name, TableBuilder referencedTable)
    {
      return new ForeignKeyBuilder(name, referencedTable);
    }

    public static ForeignKeyBuilder ForeignKey<T>(string name, string targetTable, string targetColumnName)
    {
      return new ForeignKeyBuilder(name, typeof(T), targetTable, targetColumnName);
    }

    public static SimpleColumnBuilder Simple<T>(string name)
    {
      return new SimpleColumnBuilder(name, typeof(T));
    }

    public static SimpleColumnBuilder Simple(string name, ColumnType colType)
    {
      return new SimpleColumnBuilder(name, colType);
    }

    public static SimpleColumnBuilder Simple(string name, ColumnType colType, short size)
    {
      return new SimpleColumnBuilder(name, colType, size);
    }

    public static SimpleColumnBuilder Simple<T>(string name, short size)
    {
      return new SimpleColumnBuilder(name, typeof(T), size);
    }
  }
}