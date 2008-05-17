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
    private IDatabaseProvider _databaseProvider;
    private ISchemaProvider _schemaProvider;
    private IDatabaseMigration _migration;
    private ICommonTransformations _commonTransformations;
    private IConfiguration _configuration;

    public override MigrationInitializer Create()
    {
      _databaseProvider = _mocks.StrictMock<IDatabaseProvider>();
      _schemaProvider = _mocks.StrictMock<ISchemaProvider>();
      _migration = _mocks.StrictMock<IDatabaseMigration>();
      _configuration = _mocks.StrictMock<IConfiguration>();
      _commonTransformations = _mocks.StrictMock<ICommonTransformations>();
      return new MigrationInitializer(_configuration, _databaseProvider, _schemaProvider, _commonTransformations);
    }

    [Test]
    public void InitializeMigration_Always_CallsInitialize()
    {
      using (_mocks.Record())
      {
        _migration.Initialize(_configuration, _databaseProvider, _schemaProvider, _commonTransformations);
      }
      _target.InitializeMigration(_migration);
      _mocks.VerifyAll();
    }
  }
}