namespace Machine.Migrations.Services
{
  public interface IMigrationFactory
  {
    IDatabaseMigration CreateMigration(MigrationReference migrationReference);
  }
}
