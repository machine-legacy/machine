namespace Machine.Migrations.Builders
{
  public interface ITableBuilder
  {
    IColumnBuilder AddPrimaryKey<T>(string columnName);
    IColumnBuilder AddColumn<T>(string columnName);
    IColumnBuilder AddColumn<T>(string columnName, short size);
    IColumnBuilder AddForeignKey(string columnName, TableInfo table);
  }
}