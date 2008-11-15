using System;
using System.Text.RegularExpressions;

using Machine.Core.Services;

namespace Machine.Migrations.Services.Impl
{
    public class SqlScriptMigrationFactory : IMigrationFactory
    {
      readonly IFileSystem _fileSystem;

      public SqlScriptMigrationFactory(IFileSystem fileSystem)
      {
        _fileSystem = fileSystem;
      }

      private readonly Regex _regexBothUpAndDown = new Regex(@"^/\* MIGRATE UP \*/((.|\r|\n)*)/\* MIGRATE DOWN \*/((.|\r|\n)*)$");
      private readonly Regex _regexUp = new Regex(@"^/\* MIGRATE UP \*/((.|\r|\n)*)$");
      private readonly Regex _regexDown = new Regex(@"^/\* MIGRATE DOWN \*/((.|\r|\n)*)$");

      public IDatabaseMigration CreateMigration(MigrationReference migrationReference)
      {
        var migrationFileContent = _ReadMigrationFile(migrationReference);
        if (migrationFileContent == "") return GetEmptySqlScriptMigration();

        return new SqlScriptMigration(ReadUpScriptPartFrom(migrationFileContent),
                                      ReadDownScriptPartFrom(migrationFileContent));
      }

      private string _ReadMigrationFile(MigrationReference migrationReference)
      {
        return _fileSystem.ReadAllText(migrationReference.Path);
      }
      
      private static IDatabaseMigration GetEmptySqlScriptMigration()
      {
        return new SqlScriptMigration(string.Empty, string.Empty);
      }
      
      private string ReadUpScriptPartFrom(string readScript)
      {
        var upAndDownScriptMatch = MatchUpAndDownRegexTo(readScript);
        if (upAndDownScriptMatch.Success) return upAndDownScriptMatch.Groups[1].Value;

        var upOnlyScriptMatch = MatchUpOnlyRegexTo(readScript);
        if (upOnlyScriptMatch.Success) return upOnlyScriptMatch.Groups[1].Value;

        var downOnlyScriptMatch = MatchDownOnlyRegexTo(readScript);
        if (downOnlyScriptMatch.Success) return String.Empty;

        return readScript;
      }
      
      private string ReadDownScriptPartFrom(string readScript)
      {
        var upAndDownScriptMatch = MatchUpAndDownRegexTo(readScript);
        if (upAndDownScriptMatch.Success) return upAndDownScriptMatch.Groups[3].Value;

        var downOnlyScriptMatch = MatchDownOnlyRegexTo(readScript);
        if (downOnlyScriptMatch.Success) return downOnlyScriptMatch.Groups[1].Value;

        var upOnlyScriptMatch = MatchUpOnlyRegexTo(readScript);
        if (upOnlyScriptMatch.Success) return String.Empty;

        return String.Empty;
      }
      
      private Match MatchUpAndDownRegexTo(string readScript)
      {
        return _regexBothUpAndDown.Match(readScript);
      }
      
      private Match MatchUpOnlyRegexTo(string readScript)
      {
        return _regexUp.Match(readScript);
      }
      
      private Match MatchDownOnlyRegexTo(string readScript)
      {
        return _regexDown.Match(readScript);
      }
    }
}