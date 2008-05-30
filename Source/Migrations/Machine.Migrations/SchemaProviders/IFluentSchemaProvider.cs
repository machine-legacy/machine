using System;

using Machine.Migrations.Builders;

namespace Machine.Migrations.SchemaProviders
{
  public interface IFluentSchemaProvider
  {
    TableInfo CreateTable(string tableName, Action<ITableBuilder> buildAction);
    void AlterTable(string tableName, Action<ITableMutator> alterAction);
  }
}