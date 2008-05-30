using System;
using System.Collections.Generic;
using System.Text;

using Machine.Migrations.Builders;

namespace Machine.Migrations.SchemaProviders
{
  public class FluentSchemaProvider : IFluentSchemaProvider
  {
    readonly ISchemaProvider _originalSchemaProvider;

    public FluentSchemaProvider(ISchemaProvider schemaProvider)
    {
      _originalSchemaProvider = schemaProvider;
    }

    public TableInfo CreateTable(string tableName, Action<ITableBuilder> buildAction)
    {
      var builder = new TableBuilder(tableName);
      buildAction(builder);

      var tableInfo = builder.Build(_originalSchemaProvider);

      return tableInfo;
    }

    public void AlterTable(string tableName, Action<ITableMutator> alterAction)
    {
    }
  }
}
