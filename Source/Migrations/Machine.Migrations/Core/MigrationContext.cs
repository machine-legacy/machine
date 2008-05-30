using System;
using System.Collections.Generic;
using System.Text;

using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.Services;
using Machine.Migrations.SchemaProviders;

namespace Machine.Migrations.Core
{
  public class MigrationContext
  {
    readonly IConfiguration _configuration;
    readonly IDatabaseProvider _databaseProvider;
    readonly ICommonTransformations _commonTransformations;
    readonly IConnectionProvider _connectionProvider;
    readonly ISchemaProvider _schemaProvider;

    public IConfiguration Configuration
    {
      get { return _configuration; }
    }

    public IDatabaseProvider DatabaseProvider
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

    public ISchemaProvider SchemaProvider
    {
      get { return _schemaProvider; }
    }

    public MigrationContext(IConfiguration configuration, IDatabaseProvider databaseProvider, ISchemaProvider schemaProvider,
      ICommonTransformations commonTransformations, IConnectionProvider connectionProvider)
    {
      _configuration = configuration;
      _databaseProvider = databaseProvider;
      _commonTransformations = commonTransformations;
      _connectionProvider = connectionProvider;
      _schemaProvider = schemaProvider;
    }
  }
}