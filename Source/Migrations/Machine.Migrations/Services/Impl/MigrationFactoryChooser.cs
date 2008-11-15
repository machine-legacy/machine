using System;
using System.IO;

namespace Machine.Migrations.Services.Impl
{
  public class MigrationFactoryChooser : IMigrationFactoryChooser
  {
    #region Member Data
    readonly CSharpMigrationFactory _cSharpMigrationFactory;
    readonly BooMigrationFactory _booMigrationFactory;
    readonly SqlScriptMigrationFactory _sqlScriptMigrationFactory;
    #endregion

    #region MigrationApplicatorChooser()
    public MigrationFactoryChooser(CSharpMigrationFactory cSharpMigrationFactory,
      BooMigrationFactory booMigrationFactory, SqlScriptMigrationFactory sqlScriptMigrationFactory)
    {
      _cSharpMigrationFactory = cSharpMigrationFactory;
      _sqlScriptMigrationFactory = sqlScriptMigrationFactory;
      _booMigrationFactory = booMigrationFactory;
    }
    #endregion

    #region IMigrationApplicatorChooser Members
    public IMigrationFactory ChooseFactory(MigrationReference migrationReference)
    {
      string extension = Path.GetExtension(migrationReference.Path);
      if (extension.Equals(".cs"))
      {
        return _cSharpMigrationFactory;
      }
      if (extension.Equals(".boo"))
      {
        return _booMigrationFactory;
      }
      if (extension.Equals(".sql"))
      {
        return _sqlScriptMigrationFactory;
      }
      throw new ArgumentException(migrationReference.Path);
    }
    #endregion
  }
}