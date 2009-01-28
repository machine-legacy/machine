using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Reflection;

using Machine.Core.Services;
using Microsoft.CSharp;

namespace Machine.Migrations.Services.Impl
{
  public class CSharpMigrationFactory : AbstractMigrationCompilerFactory, IMigrationFactory
  {
    #region Logging
    static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(CSharpMigrationFactory));
    #endregion

    #region Member Data
    readonly IConfiguration _configuration;
    readonly IWorkingDirectoryManager _workingDirectoryManager;
    #endregion

    #region CSharpMigrationFactory()
    public CSharpMigrationFactory(IConfiguration configuration, IWorkingDirectoryManager workingDirectoryManager)
    {
      _configuration = configuration;
      _workingDirectoryManager = workingDirectoryManager;
    }
    #endregion

    #region IMigrationApplicator Members
    public IDatabaseMigration CreateMigration(MigrationReference migrationReference)
    {
      return CreateMigrationInstance(migrationReference);
    }
    #endregion

    protected override Type CompileMigration(MigrationReference migrationReference)
    {
      Dictionary<string, string> providerOptions = new Dictionary<string, string>();
      if (!String.IsNullOrEmpty(_configuration.CompilerVersion))
      {
        providerOptions["CompilerVersion"] = _configuration.CompilerVersion;
      }
      CodeDomProvider provider = new CSharpCodeProvider(providerOptions);
      CompilerParameters parameters = new CompilerParameters();
      parameters.GenerateExecutable = false;
      parameters.OutputAssembly = Path.Combine(_workingDirectoryManager.WorkingDirectory, Path.GetFileNameWithoutExtension(migrationReference.Path) + ".dll");
      parameters.ReferencedAssemblies.Add(typeof(IDatabaseMigration).Assembly.Location);
      parameters.ReferencedAssemblies.Add(typeof(SqlMoney).Assembly.Location);
      parameters.IncludeDebugInformation = true;
      foreach (string reference in _configuration.References)
      {
        parameters.ReferencedAssemblies.Add(reference);
      }
      _log.InfoFormat("Compiling {0}", migrationReference);
      CompilerResults cr = provider.CompileAssemblyFromFile(parameters, migrationReference.Path);
      if (cr.Errors.Count > 0)
      {
        foreach (CompilerError error in cr.Errors)
        {
          _log.ErrorFormat("{0}", error);
        }
        throw new InvalidOperationException();
      }
      if (cr.CompiledAssembly == null)
      {
        throw new InvalidOperationException();
      }
      return MigrationHelpers.LookupMigration(cr.CompiledAssembly, migrationReference);
    }
  }
}