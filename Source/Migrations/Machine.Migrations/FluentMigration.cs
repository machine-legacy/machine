using System;
using System.Collections.Generic;
using System.Text;

using Machine.Migrations.Builders;
using Machine.Migrations.SchemaProviders;

namespace Machine.Migrations
{
  public abstract class FluentMigration : SimpleMigration
  {
    IFluentSchemaProvider _fluentSchema;

    public new IFluentSchemaProvider Schema
    {
      get { return _fluentSchema; }
    }

    public new ISchemaProvider SimpleSchema
    {
      get { return base.Schema; }
    }

    public override void Initialize(Machine.Migrations.Core.MigrationContext context)
    {
      base.Initialize(context);

      _fluentSchema = new FluentSchemaProvider(base.Schema);
    }
  }
}
