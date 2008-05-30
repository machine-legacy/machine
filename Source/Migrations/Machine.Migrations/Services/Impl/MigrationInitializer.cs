
using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;

namespace Machine.Migrations.Services.Impl
{
  public class MigrationInitializer : IMigrationInitializer
  {
    #region Member Data
    private readonly IConfiguration _configuration;
    private readonly IDatabaseProvider _databaseProvider;
    private readonly ISchemaProvider _schemaProvider;
    private readonly ICommonTransformations _commonTransformations;
  	private readonly IConnectionProvider _connectionProvider;

  	#endregion

    #region MigrationInitializer()
    public MigrationInitializer(IConfiguration configuration, IDatabaseProvider databaseProvider, ISchemaProvider schemaProvider, ICommonTransformations commonTransformations, IConnectionProvider connectionProvider)
    {
      _configuration = configuration;
      _commonTransformations = commonTransformations;
      _databaseProvider = databaseProvider;
      _schemaProvider = schemaProvider;
    	_connectionProvider = connectionProvider;
    }
    #endregion

    #region IMigrationInitializer Members
    public void InitializeMigration(IDatabaseMigration migration)
    {
		migration.Initialize(_configuration, _databaseProvider, _schemaProvider, _commonTransformations, _connectionProvider);
    }
    #endregion
  }
}
