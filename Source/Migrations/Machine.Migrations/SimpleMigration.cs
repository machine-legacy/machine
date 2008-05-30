using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;
using Machine.Migrations.Services;

namespace Machine.Migrations
{
  public abstract class SimpleMigration : IDatabaseMigration
  {
    #region Member Data
    readonly log4net.ILog _log;
    ISchemaProvider _schemaProvider;
    IDatabaseProvider _databaseProvider;
    ICommonTransformations _commonTransformations;
    IConfiguration _configuration;
    IConnectionProvider _connectionProvider;
    #endregion

    #region Properties
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
    #endregion

    #region SimpleMigration()
    protected SimpleMigration()
    {
      _log = log4net.LogManager.GetLogger(GetType());
    }
    #endregion

    #region IDatabaseMigration Members
    public virtual void Initialize(IConfiguration configuration, IDatabaseProvider databaseProvider,
      ISchemaProvider schemaProvider, ICommonTransformations commonTransformations,
      IConnectionProvider connectionProvider)
    {
      _configuration = configuration;
      _schemaProvider = schemaProvider;
      _databaseProvider = databaseProvider;
      _commonTransformations = commonTransformations;
      _connectionProvider = connectionProvider;
    }

    public void SetCommandTimeout(int timeout)
    {
      _configuration.SetCommandTimeout(timeout);
    }

    public abstract void Up();

    public abstract void Down();
    #endregion
  }
}