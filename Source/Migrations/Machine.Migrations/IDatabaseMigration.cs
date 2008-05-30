using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;
using Machine.Migrations.Services;

namespace Machine.Migrations
{
  public interface IDatabaseMigration
  {
    void Initialize(IConfiguration configuration, IDatabaseProvider databaseProvider, ISchemaProvider schemaProvider, ICommonTransformations commonTransformations, IConnectionProvider connectionProvider);
    void Up();
    void Down();
  }
}
