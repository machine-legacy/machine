using System;

using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;

namespace Machine.Migrations.Services.Impl
{
  public class SchemaStateManager : ISchemaStateManager
  {
    #region Logging
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(SchemaStateManager));
    #endregion

    #region Member Data
    private readonly string TableName = "schema_info";
    private readonly string IdColumnName = "id";
    // private readonly string ApplicationDateColumnName = "applied_at";
    private readonly string VersionColumnName = "version";
    private readonly string ScopeColumnName = "scope";
    private readonly IDatabaseProvider _databaseProvider;
    private readonly ISchemaProvider _schemaProvider;
    #endregion

    #region SchemaStateManager()
    public SchemaStateManager(IDatabaseProvider databaseProvider, ISchemaProvider schemaProvider)
    {
      _databaseProvider = databaseProvider;
      _schemaProvider = schemaProvider;
    }
    #endregion

    #region ISchemaStateManager Members
    public void CheckSchemaInfoTable()
    {
      if (_schemaProvider.HasTable(TableName))
      {
        if (!_schemaProvider.HasColumn(TableName, ScopeColumnName))
        {
          _log.InfoFormat("Adding {0} column to {1}...", ScopeColumnName, TableName);
          _schemaProvider.AddColumn(TableName, ScopeColumnName, typeof(string), 25, false, true);
        }

        return;
      }

      _log.InfoFormat("Creating {0}...", TableName);

      Column[] columns = new Column[] {
        new Column(IdColumnName, typeof(Int32), 4, true),
        new Column(VersionColumnName, typeof(Int32), 4, false),
        new Column(ScopeColumnName, typeof(string), 25, false, true)
      };
      _schemaProvider.AddTable(TableName, columns);
    }

    public short[] GetAppliedMigrationVersions(string scope)
    {
      if (string.IsNullOrEmpty(scope))
      {
        return _databaseProvider.ExecuteScalarArray<Int16>("SELECT CAST({1} AS SMALLINT) FROM {0} WHERE {2} IS NULL ORDER BY {1}",
          TableName, VersionColumnName, ScopeColumnName);
      }
      else
      {
        return _databaseProvider.ExecuteScalarArray<Int16>("SELECT CAST({1} AS SMALLINT) FROM {0} WHERE {2} = '{3}' ORDER BY {1}",
          TableName, VersionColumnName, ScopeColumnName, scope);
      }
    }

    public void SetMigrationVersionUnapplied(short version, string scope)
    {
      if (string.IsNullOrEmpty(scope))
      {
        _databaseProvider.ExecuteNonQuery("DELETE FROM {0} WHERE {1} = {2} AND {3} IS NULL",
          TableName, VersionColumnName, version, ScopeColumnName);
      }
      else
      {
        _databaseProvider.ExecuteNonQuery("DELETE FROM {0} WHERE {1} = {2} AND {3} = '{4}'",
          TableName, VersionColumnName, version, ScopeColumnName, scope);
      }
    }

    public void SetMigrationVersionApplied(short version, string scope)
    {
      if (string.IsNullOrEmpty(scope))
      {
        _databaseProvider.ExecuteNonQuery("INSERT INTO {0} ({1}, {2}) VALUES ({3}, NULL)",
          TableName, VersionColumnName, ScopeColumnName, version);
      }
      else
      {
        _databaseProvider.ExecuteNonQuery("INSERT INTO {0} ({1}, {2}) VALUES ({3}, '{4}')",
          TableName, VersionColumnName, ScopeColumnName, version, scope);
      }
    }
    #endregion
  }
}
