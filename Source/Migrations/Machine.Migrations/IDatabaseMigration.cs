using Machine.Migrations.Core;
using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;
using Machine.Migrations.Services;

namespace Machine.Migrations
{
  public interface IDatabaseMigration
  {
    void Initialize(MigrationContext context);

    void Up();
    void Down();
  }
}