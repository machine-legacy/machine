using System.Collections.Generic;

using Machine.Migrations.DatabaseProviders;

namespace Machine.Migrations.Services.Impl
{
  public class Migrator : IMigrator
  {
    #region Member Data
    readonly IDatabaseProvider _databaseProvider;
    readonly IMigrationSelector _migrationSelector;
    readonly IMigrationRunner _migrationRunner;
    readonly ISchemaStateManager _schemaStateManager;
    readonly IWorkingDirectoryManager _workingDirectoryManager;
    #endregion

    #region Migrator()
    public Migrator(IMigrationSelector migrationSelector, IMigrationRunner migrationRunner,
      IDatabaseProvider databaseProvider, ISchemaStateManager schemaStateManager,
      IWorkingDirectoryManager workingDirectoryManager)
    {
      _migrationSelector = migrationSelector;
      _workingDirectoryManager = workingDirectoryManager;
      _schemaStateManager = schemaStateManager;
      _databaseProvider = databaseProvider;
      _migrationRunner = migrationRunner;
    }
    #endregion

    #region IMigrator Members
    public void RunMigrator()
    {
      try
      {
        _workingDirectoryManager.Create();
        _databaseProvider.Open();
        _schemaStateManager.CheckSchemaInfoTable();
        ICollection<MigrationStep> steps = _migrationSelector.SelectMigrations();
        if (_migrationRunner.CanMigrate(steps))
        {
          _migrationRunner.Migrate(steps);
        }
      }
      finally
      {
        _databaseProvider.Close();
      }
    }
    #endregion
  }
}