namespace Machine.Migrations.Services
{
  public interface ISchemaStateManager
  {
    void CheckSchemaInfoTable();
    short[] GetAppliedMigrationVersions(string scope);
    void SetMigrationVersionUnapplied(short version, string scope);
    void SetMigrationVersionApplied(short version, string scope);
  }
}