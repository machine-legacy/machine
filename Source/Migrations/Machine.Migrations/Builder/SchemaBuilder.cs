namespace Machine.Migrations.Builder
{
  using SchemaProviders;

  public class SchemaBuilder
  {
    readonly ISchemaProvider schemaProvider;

    public SchemaBuilder(ISchemaProvider schemaProvider)
    {
      this.schemaProvider = schemaProvider;
    }

    public TableBuilder AddTable(string name, params IColumnBuilder[] columns)
    {
      return new TableBuilder(name, columns).Build(schemaProvider);
    }
  }
}