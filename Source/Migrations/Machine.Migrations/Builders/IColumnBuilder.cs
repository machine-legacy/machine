namespace Machine.Migrations.Builders
{
  using System.Collections.Generic;

  using SchemaProviders;

  public interface IColumnBuilder
  {
    string Name { get; }

    short? Size { get; }

    ColumnType ColumnType { get; }

    Column Build(TableBuilder table, ISchemaProvider schemaBuilder, IList<PostProcess> posts);
  }
}