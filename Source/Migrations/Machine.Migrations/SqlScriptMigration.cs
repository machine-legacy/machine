using Machine.Migrations.Core;
using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;
using Machine.Migrations.Services;

namespace Machine.Migrations
{
  public class SqlScriptMigration : IDatabaseMigration
  {
    readonly log4net.ILog _log;
    readonly string _upScript;
    readonly string _downScript;
    ISchemaProvider _schemaProvider;
    IDatabaseProvider _databaseProvider;
    ICommonTransformations _commonTransformations;
    IConfiguration _configuration;
    IConnectionProvider _connectionProvider;

    public log4net.ILog Log
    {
      get { return _log; }
    }

    public IConfiguration Configuration
    {
      get { return _configuration; }
    }

    public ISchemaProvider Schema
    {
      get { return _schemaProvider; }
    }

    public IDatabaseProvider Database
    {
      get { return _databaseProvider; }
    }

    public ICommonTransformations CommonTransformations
    {
      get { return _commonTransformations; }
    }

    public IConnectionProvider ConnectionProvider
    {
      get { return _connectionProvider; }
    }

    public string UpScript
    {
      get { return _upScript; }
    }

    public string DownScript
    {
      get { return _downScript; }
    }

    public SqlScriptMigration(string upScript, string downScript)
    {
      _log = log4net.LogManager.GetLogger(GetType());
      _upScript = upScript;
      _downScript = downScript;
    }

    public void Initialize(MigrationContext context)
    {
      _configuration = context.Configuration;
      _schemaProvider = context.SchemaProvider;
      _databaseProvider = context.DatabaseProvider;
      _commonTransformations = context.CommonTransformations;
      _connectionProvider = context.ConnectionProvider;
    }

    public void Up()
    {
      if (string.IsNullOrEmpty(UpScript)) return;

      Database.ExecuteNonQuery(UpScript);
    }

    public void Down()
    {
      if (string.IsNullOrEmpty(DownScript)) return;

      Database.ExecuteNonQuery(DownScript);
    }
  }
}