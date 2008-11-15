using System;
using System.Text;
using Machine.Core;
using Machine.Core.Services;
using NUnit.Framework;
using Rhino.Mocks;

namespace Machine.Migrations.Services.Impl
{
  [TestFixture]
  public class SqlScriptMigrationFactoryTests : StandardFixture<SqlScriptMigrationFactory>
  {
    IFileSystem _fileSystem;
    string _migrationPath;
    MigrationReference _migrationReference;
    IDatabaseMigration resultingMigration;

    public override SqlScriptMigrationFactory Create()
    {
      _fileSystem = _mocks.DynamicMock<IFileSystem>();
      return new SqlScriptMigrationFactory(_fileSystem);
    }

    public string ReadScriptWithoutMarkers
    {
      get
      {
        var builder = new StringBuilder();
        builder.AppendFormat("some{0}", Environment.NewLine);
        builder.AppendFormat("multiline{0}", Environment.NewLine);
        builder.AppendFormat("script{0}", Environment.NewLine);
        builder.AppendFormat("without{0}", Environment.NewLine);
        builder.AppendFormat("markers{0}", Environment.NewLine);

        return builder.ToString();
      }
    }
    
    public string UpScript
    {
      get
      {
        var builder = new StringBuilder();
        builder.AppendFormat("{0}", Environment.NewLine);
        builder.AppendFormat("some{0}", Environment.NewLine);
        builder.AppendFormat("multiline{0}", Environment.NewLine);
        builder.AppendFormat("script{0}", Environment.NewLine);
        builder.AppendFormat("with{0}", Environment.NewLine);
        builder.AppendFormat("UP{0}", Environment.NewLine);
        builder.AppendFormat("marker{0}", Environment.NewLine);
        builder.AppendFormat("{0}", Environment.NewLine);

        return builder.ToString();
      }
    }
    
    public string DownScript
    {
      get
      {
        var builder = new StringBuilder();
        builder.AppendFormat("{0}", Environment.NewLine);
        builder.AppendFormat("some{0}", Environment.NewLine);
        builder.AppendFormat("multiline{0}", Environment.NewLine);
        builder.AppendFormat("script{0}", Environment.NewLine);
        builder.AppendFormat("with{0}", Environment.NewLine);
        builder.AppendFormat("DOWN{0}", Environment.NewLine);
        builder.AppendFormat("marker{0}", Environment.NewLine);
        builder.AppendFormat("{0}", Environment.NewLine);

        return builder.ToString();
      }
    }
    
    public string ReadScriptWithBothUpAndDownMarkers
    {
      get
      {
        var builder = new StringBuilder();
        builder.AppendFormat("/* MIGRATE UP */");
        builder.AppendFormat("{0}", UpScript);
        builder.AppendFormat("/* MIGRATE DOWN */");
        builder.AppendFormat("{0}", DownScript);

        return builder.ToString();
      }
    }
    
    public string ReadScriptWithUpMarkerOnly
    {
      get
      {
        var builder = new StringBuilder();
        builder.AppendFormat("/* MIGRATE UP */");
        builder.AppendFormat("{0}", UpScript);

        return builder.ToString();
      }
    }
    
    public string ReadScriptWithDownMarkerOnly
    {
      get
      {
        var builder = new StringBuilder();
        builder.AppendFormat("/* MIGRATE DOWN */");
        builder.AppendFormat("{0}", DownScript);

        return builder.ToString();
      }
    }

    public override void Setup()
    {
      base.Setup();
      _migrationPath = @"C:\Migration.sql";
      _migrationReference = new MigrationReference(1, "", _migrationPath);
    }

    [Test]
    public void CreateMigration_when_called_will_read_the_contents_of_the_file_from_MigrationReference_Path()
    {
      using (_mocks.Record())
      {
        Expect.Call(_fileSystem.ReadAllText(_migrationPath)).Repeat.Once().Return("");
      }
      using (_mocks.Playback())
      {
        _target.CreateMigration(_migrationReference);
      }

      _mocks.VerifyAll();
    }

    [Test]
    public void CreateMigration_reading_empty_file_will_return_a_SqlScriptMigration_with_empty_Up_and_Down_scripts()
    {
      using (_mocks.Record())
      {
        SetupResult.For(_fileSystem.ReadAllText(_migrationPath)).Repeat.Once().Return("");
      }
      using (_mocks.Playback())
      {
        resultingMigration = _target.CreateMigration(_migrationReference);
      }

      var sqlScriptMigration = resultingMigration as SqlScriptMigration;
      if (sqlScriptMigration == null) Assert.Fail("Returned migration should be SqlScriptMigration!");

      Assert.That(sqlScriptMigration.UpScript == "");
      Assert.That(sqlScriptMigration.DownScript == "");

      _mocks.VerifyAll();
    }

    [Test]
    public void CreateMigration_reading_a_file_without_markers_will_initialize_SqlScriptMigration_with_Up_script_only()
    {
      using (_mocks.Record())
      {
        SetupResult.For(_fileSystem.ReadAllText(_migrationPath)).Repeat.Once().Return(ReadScriptWithoutMarkers);
      }
      using (_mocks.Playback())
      {
        resultingMigration = _target.CreateMigration(_migrationReference);
      }

      var sqlScriptMigration = resultingMigration as SqlScriptMigration;

      Assert.That(sqlScriptMigration.UpScript == ReadScriptWithoutMarkers);
      Assert.That(sqlScriptMigration.DownScript == "");

      _mocks.VerifyAll();
    }

    [Test]
    public void CreateMigration_reading_a_file_with_both_Up_and_Down_markers_will_initialize_SqlScriptMigration_with_both_Up_and_Down_scripts()
    {
      using (_mocks.Record())
      {
        SetupResult.For(_fileSystem.ReadAllText(_migrationPath)).Repeat.Once().Return(ReadScriptWithBothUpAndDownMarkers);
      }
      using (_mocks.Playback())
      {
        resultingMigration = _target.CreateMigration(_migrationReference);
      }

      var sqlScriptMigration = resultingMigration as SqlScriptMigration;

      Assert.That(sqlScriptMigration.UpScript == UpScript);
      Assert.That(sqlScriptMigration.DownScript == DownScript);

      _mocks.VerifyAll();
    }

    [Test]
    public void CreateMigration_reading_a_file_with_Up_marker_only_will_initialize_SqlScriptMigration_with_Up_scripts_only()
    {
      using (_mocks.Record())
      {
        SetupResult.For(_fileSystem.ReadAllText(_migrationPath)).Repeat.Once().Return(ReadScriptWithUpMarkerOnly);
      }
      using (_mocks.Playback())
      {
        resultingMigration = _target.CreateMigration(_migrationReference);
      }

      var sqlScriptMigration = resultingMigration as SqlScriptMigration;

      Assert.That(sqlScriptMigration.UpScript == UpScript);
      Assert.That(sqlScriptMigration.DownScript == "");

      _mocks.VerifyAll();
    }

    [Test]
    public void CreateMigration_reading_a_file_with_Down_marker_only_will_initialize_SqlScriptMigration_with_Down_scripts_only()
    {
      using (_mocks.Record())
      {
        SetupResult.For(_fileSystem.ReadAllText(_migrationPath)).Repeat.Once().Return(ReadScriptWithDownMarkerOnly);
      }
      using (_mocks.Playback())
      {
        resultingMigration = _target.CreateMigration(_migrationReference);
      }

      var sqlScriptMigration = resultingMigration as SqlScriptMigration;

      Assert.That(sqlScriptMigration.UpScript == "");
      Assert.That(sqlScriptMigration.DownScript == DownScript);

      _mocks.VerifyAll();
    }
  }
}