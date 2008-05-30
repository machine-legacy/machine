namespace Machine.Migrations.Builders
{
  public interface IColumnBuilder
  {
    IColumnBuilder Identity();
    IColumnBuilder Nullable();
    IColumnBuilder Unique();
  }
}