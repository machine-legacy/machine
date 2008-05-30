using System;
using System.Collections.Generic;

using Machine.Core;
using Machine.Migrations.Core;
using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;
using Machine.Migrations.Services;

using NUnit.Framework;

namespace Machine.Migrations
{
  [TestFixture]
  public class SimpleMigrationTests : StandardFixture<ConcreteSimpleMigration>
  {
    IDatabaseProvider _databaseProvider;
    ISchemaProvider _schemaProvider;
    IConfiguration _configuration;
    ICommonTransformations _commonTransformations;
    IConnectionProvider _connectionProvider;

    public override ConcreteSimpleMigration Create()
    {
      _configuration = _mocks.DynamicMock<IConfiguration>();
      _databaseProvider = _mocks.DynamicMock<IDatabaseProvider>();
      _schemaProvider = _mocks.DynamicMock<ISchemaProvider>();
      _commonTransformations = _mocks.DynamicMock<ICommonTransformations>();
      _connectionProvider = _mocks.DynamicMock<IConnectionProvider>();
      return new ConcreteSimpleMigration();
    }

    [Test]
    public void Initialize_Always_SetsServices()
    {
      _target.Initialize(new MigrationContext(
        _configuration, _databaseProvider, _schemaProvider, _commonTransformations, _connectionProvider));
      Assert.AreEqual(_databaseProvider, _target.Database);
      Assert.AreEqual(_schemaProvider, _target.Schema);
    }
  }

  public class ConcreteSimpleMigration : SimpleMigration
  {
    public override void Up()
    {
    }

    public override void Down()
    {
    }
  }
}