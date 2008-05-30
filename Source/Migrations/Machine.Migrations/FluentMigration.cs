using System;
using System.Collections.Generic;
using System.Text;

using Machine.Migrations.Builders;

namespace Machine.Migrations
{
  public abstract class FluentMigration : SimpleMigration
  {
    public TableInfo CreateTable(string tableName, Action<ITableBuilder> buildAction)
    {
      var builder = new TableBuilder(tableName);
      buildAction(builder);

      var tableInfo = builder.Build(this.Schema);

      return tableInfo;
    }
  }
}
