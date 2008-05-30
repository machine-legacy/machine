using System;
using System.Collections.Generic;

using Machine.Core;
using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;

using NUnit.Framework;

namespace Machine.Migrations.Services.Impl
{
  [TestFixture]
  public class MigrationInitializerTests : StandardFixture<MigrationInitializer>
  {
    IDatabaseProvider _databaseProvider;
    ISchemaProvider _schemaProvider;
    IDatabaseMigration _migration;
    ICommonTransformations _commonTransformations;
    IConfiguration _configuration;
    IConnectionProvider _connectionProvider;

    public override MigrationInitializer Create()
    {
      _databaseProvider = _mocks.StrictMock<IDatabaseProvider>();
      _schemaProvider = _mocks.StrictMock<ISchemaProvider>();
      _migration = _mocks.StrictMock<IDatabaseMigration>();
      _configuration = _mocks.StrictMock<IConfiguration>();
      _commonTransformations = _mocks.StrictMock<ICommonTransformations>();
      _connectionProvider = _mocks.StrictMock<IConnectionProvider>();

      return new MigrationInitializer(_configuration, _databaseProvider, _schemaProvider, _commonTransformations,
        _connectionProvider);
    }

    [Test]
    public void InitializeMigration_Always_CallsInitialize()
    {
      using (_mocks.Record())
      {
        _migration.Initialize(_configuration, _databaseProvider, _schemaProvider, _commonTransformations,
          _connectionProvider);
      }
      _target.InitializeMigration(_migration);
      _mocks.VerifyAll();
    }
  }
}