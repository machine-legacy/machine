using System;
using System.Collections.Generic;

using Machine.Core;
using Machine.Core.Services;

using NUnit.Framework;

namespace Machine.Migrations.Services.Impl
{
  [TestFixture]
  public class MigrationFactoryChooserTests : StandardFixture<MigrationFactoryChooser>
  {
    CSharpMigrationFactory _cSharpMigrationFactory;
    BooMigrationFactory _booMigrationFactory;
    SqlScriptMigrationFactory _sqlScriptMigrationFactory;
    IConfiguration _configuration;
    IWorkingDirectoryManager _workingDirectoryManager;
    IFileSystem _fileSystem;

    [Test]
    public void ChooseFactory_IsCSharp_ReturnsFactory()
    {
      Assert.AreEqual(_cSharpMigrationFactory,
        _target.ChooseFactory(new MigrationReference(1, "Migration", "001_migration.cs")));
    }

    [Test]
    public void ChooseFactory_IsBoo_ReturnsFactory()
    {
      Assert.AreEqual(_booMigrationFactory,
        _target.ChooseFactory(new MigrationReference(1, "Migration", "001_migration.boo")));
    }

    [Test]
    public void ChooseFactory_IsSqlScript_ReturnsFactory()
    {
      Assert.AreEqual(_sqlScriptMigrationFactory,
        _target.ChooseFactory(new MigrationReference(1, "Migration", "001_migration.sql")));
    }

    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void ChooseFactory_IsNotCSharpOrBooOrSql_Throws()
    {
      _target.ChooseFactory(new MigrationReference(1, "Migration", "001_migration.vb"));
    }

    public override MigrationFactoryChooser Create()
    {
      _configuration = _mocks.DynamicMock<IConfiguration>();
      _fileSystem = _mocks.DynamicMock<IFileSystem>();
      _workingDirectoryManager = _mocks.DynamicMock<IWorkingDirectoryManager>();
      _cSharpMigrationFactory = new CSharpMigrationFactory(_configuration, _workingDirectoryManager);
      _booMigrationFactory = new BooMigrationFactory(_configuration, _workingDirectoryManager);
      _sqlScriptMigrationFactory = new SqlScriptMigrationFactory(_fileSystem);
      return new MigrationFactoryChooser(_cSharpMigrationFactory, _booMigrationFactory, _sqlScriptMigrationFactory);
    }
  }
}