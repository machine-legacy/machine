using Machine.Core;
using Machine.Migrations.Core;
using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;
using Machine.Migrations.Services;

using Rhino.Mocks;
using NUnit.Framework;

namespace Machine.Migrations
{
  [TestFixture]
  public class SqlScriptMigrationTests : StandardFixture<SqlScriptMigration>
  {
    IDatabaseProvider _dbProvider;
    MigrationContext _mockedMigrationContext;

    public override SqlScriptMigration Create()
    {
      return new SqlScriptMigration("", "");
    }

    public override void Setup()
    {
      base.Setup();
      _dbProvider = _mocks.DynamicMock<IDatabaseProvider>();
      _mockedMigrationContext = new MigrationContext(_mocks.Stub<IConfiguration>(), _dbProvider, _mocks.Stub<ISchemaProvider>(), _mocks.Stub<ICommonTransformations>(), _mocks.Stub<IConnectionProvider>());
    }

    [Test]
    public void Initialize_Always_SetsServices()
    {
      _target.Initialize(_mockedMigrationContext);

      Assert.AreEqual(_mockedMigrationContext.DatabaseProvider, _target.Database);
      Assert.AreEqual(_mockedMigrationContext.SchemaProvider, _target.Schema);
    }

    [Test]
    public void Up_does_not_execute_a_query_when_Up_script_is_not_provided()
    {
      _target = new SqlScriptMigration("", "");
      _target.Initialize(_mockedMigrationContext);

      using (_mocks.Record())
      {
        Expect.Call(_dbProvider.ExecuteNonQuery("")).IgnoreArguments().Repeat.Never();
      }
      using (_mocks.Playback())
      {
        _target.Up();
      }

      _mocks.VerifyAll();
    }

    [Test]
    public void Up_executes_a_query_when_Up_script_is_provided()
    {
      var script = "some script";

      _target = new SqlScriptMigration(script, "");
      _target.Initialize(_mockedMigrationContext);

      using (_mocks.Record())
      {
        Expect.Call(_dbProvider.ExecuteNonQuery(script)).Repeat.Once().Return(false);
      }
      using (_mocks.Playback())
      {
        _target.Up();
      }

      _mocks.VerifyAll();
    }

    [Test]
    public void Down_does_not_execute_a_query_when_Down_script_is_not_provided()
    {
      _target = new SqlScriptMigration("", "");
      _target.Initialize(_mockedMigrationContext);

      using (_mocks.Record())
      {
        Expect.Call(_dbProvider.ExecuteNonQuery("")).IgnoreArguments().Repeat.Never();
      }
      using (_mocks.Playback())
      {
        _target.Down();
      }

      _mocks.VerifyAll();
    }

    [Test]
    public void Down_executes_a_query_when_Down_script_is_provided()
    {
      var script = "some script";

      _target = new SqlScriptMigration("", script);
      _target.Initialize(_mockedMigrationContext);

      using (_mocks.Record())
      {
        Expect.Call(_dbProvider.ExecuteNonQuery(script)).Repeat.Once().Return(false);
      }
      using (_mocks.Playback())
      {
        _target.Down();
      }

      _mocks.VerifyAll();
    }
  }
}