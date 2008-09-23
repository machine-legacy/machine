using System;

using Machine.Container.Services;
using Machine.Core.Utility;
using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;
using Machine.Migrations.Services;
using Machine.Migrations.Services.Impl;
using Machine.MsBuildExtensions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Machine.Migrations
{
  public class MigratorTask : Task, IConfiguration
  {
    string _migrationsDirectory;
    string _scope;
    string _connectionString;
    short _desiredVersion;
    int _commandTimeout = 30;
    bool _diagnostics;
    string[] _references;
    string _compilerVersion;

    public MigratorTask()
    {
      _migrationsDirectory = Environment.CurrentDirectory;
    }

    public virtual IMigratorContainerFactory CreateContainerFactory()
    {
      return new MigratorContainerFactory();
    }

    public override bool Execute()
    {
      log4net.Config.BasicConfigurator.Configure(new Log4NetMsBuildAppender(this.Log, new log4net.Layout.PatternLayout("%-5p %x %m")));
      LoggingHelper.Disable("Machine.Container");
      IMigratorContainerFactory migratorContainerFactory = CreateContainerFactory();
      using (Machine.Core.LoggingUtilities.Log4NetNdc.Push(String.Empty))
      {
        IHighLevelContainer container = migratorContainerFactory.CreateAndPopulateContainer(this);
        container.Resolve.Object<IMigrator>().RunMigrator();
      }
      return true;
    }

    #region IConfiguration Members
    [Required]
    public string ConnectionString
    {
      get { return _connectionString; }
      set { _connectionString = value; }
    }

    public string Scope
    {
      get { return _scope; }
      set { _scope = value; }
    }

    public string MigrationsDirectory
    {
      get { return _migrationsDirectory; }
      set { _migrationsDirectory = value; }
    }

    public short DesiredVersion
    {
      get { return _desiredVersion; }
      set { _desiredVersion = value; }
    }

    public string CompilerVersion
    {
      get { return _compilerVersion; }
      set { _compilerVersion = value; }
    }

    public bool ShowDiagnostics
    {
      get { return _diagnostics; }
      set { _diagnostics = value; }
    }

    public string[] References
    {
      get
      {
        if (_references == null)
        {
          return new string[0];
        }
        return _references;
      }
      set { _references = value; }
    }

    public int CommandTimeout
    {
      get { return _commandTimeout; }
      set { _commandTimeout = value; }
    }

    public void SetCommandTimeout(int commandTimeout)
    {
      this.CommandTimeout = commandTimeout;
    }

    public virtual Type ConnectionProviderType
    {
      get { return typeof(SqlServerConnectionProvider); }
    }

    public virtual Type TransactionProviderType
    {
      get { return typeof(TransactionProvider); }
    }

    public virtual Type SchemaProviderType
    {
      get { return typeof(SqlServerSchemaProvider); }
    }

    public virtual Type DatabaseProviderType
    {
      get { return typeof(SqlServerDatabaseProvider); }
    }
    #endregion
  }
}