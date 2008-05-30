using System;
using System.Collections.Generic;

using Machine.Core;
using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;

using NUnit.Framework;
using Rhino.Mocks;

namespace Machine.Migrations.Services.Impl
{
  [TestFixture]
  public class SchemaStateManagerTests : StandardFixture<SchemaStateManager>
  {
    private IDatabaseProvider _databaseProvider;
    private ISchemaProvider _schemaProvider;

    public override SchemaStateManager Create()
    {
      _databaseProvider = _mocks.StrictMock<IDatabaseProvider>();
      _schemaProvider = _mocks.DynamicMock<ISchemaProvider>();
      return new SchemaStateManager(_databaseProvider, _schemaProvider);
    }

    [Test]
    public void GetAppliedMigrationVersions_Always_JustDoesSelectAndReturnsArray()
    {
      using (_mocks.Record())
      {
        SetupResult.For(_databaseProvider.ExecuteScalarArray<Int16>(
          "SELECT CAST({1} AS SMALLINT) FROM {0} WHERE {2} IS NULL ORDER BY {1}",
          "schema_info", "version", "scope")).Return(new Int16[] { 1, 2, 3 });
      }
      Assert.AreEqual(new Int16[] { 1, 2, 3 }, _target.GetAppliedMigrationVersions(null));
      _mocks.VerifyAll();
    }

    [Test]
    public void GetAppliedMigrationVersions_WithScope_Always_SelectAndReturnsArray()
    {
      using (_mocks.Record())
      {
        SetupResult.For(_databaseProvider.ExecuteScalarArray<Int16>(
          "SELECT CAST({1} AS SMALLINT) FROM {0} WHERE {2} = '{3}' ORDER BY {1}",
          "schema_info", "version", "scope", "core")).Return(new Int16[] { 1, 2, 3 });
      }
      Assert.AreEqual(new Int16[] { 1, 2, 3 }, _target.GetAppliedMigrationVersions("core"));
      _mocks.VerifyAll();
    }

    [Test]
    public void CheckSchemaInfoTable_DoesHaveTable_DoesNothing()
    {
      using (_mocks.Record())
      {
        SetupResult.For(_schemaProvider.HasTable("schema_info")).Return(true);
      }
      _target.CheckSchemaInfoTable();
      _mocks.VerifyAll();
    }

    [Test]
    public void CheckSchemaInfoTable_DoesNotHaveTable_CreatesTable()
    {
      using (_mocks.Record())
      {
        SetupResult.For(_schemaProvider.HasTable("schema_info")).Return(false);
        _schemaProvider.AddTable("schema_info", null);
        LastCall.IgnoreArguments();
      }
      _target.CheckSchemaInfoTable();
      _mocks.VerifyAll();
    }

    [Test]
    public void SetMigrationVersionUnapplied_Always_NukesRow()
    {
      short version = 1;
      using (_mocks.Record())
      {
        SetupResult.For(_databaseProvider.ExecuteNonQuery(
          "DELETE FROM {0} WHERE {1} = {2} AND {3} IS NULL",
          "schema_info", "version", version, "scope")).Return(true);
      }
      _target.SetMigrationVersionUnapplied(version, null);
      _mocks.VerifyAll();
    }

    [Test]
    public void SetMigrationVersionUnapplied_WithScope_Always_NukesRow()
    {
      short version = 1;
      using (_mocks.Record())
      {
        SetupResult.For(_databaseProvider.ExecuteNonQuery(
          "DELETE FROM {0} WHERE {1} = {2} AND {3} = '{4}'",
          "schema_info", "version", version, "scope", "core")).Return(true);
      }
      _target.SetMigrationVersionUnapplied(version, "core");
      _mocks.VerifyAll();
    }

    [Test]
    public void SetMigrationVersionApplied_Always_AddsRow()
    {
      short version = 2;
      using (_mocks.Record())
      {
        SetupResult.For(_databaseProvider.ExecuteNonQuery(
      "INSERT INTO {0} ({1}, {2}) VALUES ({3}, NULL)",
      "schema_info", "version", "scope", version)).Return(true);
      }
      _target.SetMigrationVersionApplied(version, null);
      _mocks.VerifyAll();
    }

    [Test]
    public void SetMigrationVersionApplied_WithScope_Always_AddsRow()
    {
      short version = 2;
      using (_mocks.Record())
      {
        SetupResult.For(_databaseProvider.ExecuteNonQuery(
      "INSERT INTO {0} ({1}, {2}) VALUES ({3}, '{4}')",
      "schema_info", "version", "scope", version, "core")).Return(true);
      }
      _target.SetMigrationVersionApplied(version, "core");
      _mocks.VerifyAll();
    }
  }
}

