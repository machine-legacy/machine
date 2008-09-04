using Machine.Container;
using Machine.Container.Services;
using Machine.Core.Services;
using Machine.Core.Services.Impl;
using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;

namespace Machine.Migrations.Services.Impl
{
  public class MigratorContainerFactory : IMigratorContainerFactory
  {
    #region IMigratorContainerFactory Members
    public virtual IHighLevelContainer CreateAndPopulateContainer(IConfiguration configuration)
    {
      IHighLevelContainer container = CreateContainer();
      container.Initialize();
      container.PrepareForServices();
      container.Add<IConnectionProvider>(configuration.ConnectionProviderType);
      container.Add<ITransactionProvider>(configuration.TransactionProviderType);
      container.Add<IDatabaseProvider>(configuration.DatabaseProviderType);
      container.Add<ISchemaProvider>(configuration.SchemaProviderType);
      container.Add<IFileSystem, FileSystem>();
      container.Add<INamer, Namer>();
      container.Add<ISchemaStateManager, SchemaStateManager>();
      container.Add<IMigrationFinder, MigrationFinder>();
      container.Add<IMigrationSelector, MigrationSelector>();
      container.Add<IMigrationRunner, MigrationRunner>();
      container.Add<IMigrationInitializer, MigrationInitializer>();
      container.Add<IMigrator, Migrator>();
      container.Add<IMigrationFactoryChooser, MigrationFactoryChooser>();
      container.Add<IVersionStateFactory, VersionStateFactory>();
      container.Add<IWorkingDirectoryManager, WorkingDirectoryManager>();
      container.Add<ICommonTransformations, CommonTransformations>();
      container.Add<IConfiguration>(configuration);
      container.Add<CSharpMigrationFactory>();
      container.Add<BooMigrationFactory>();
      container.Start();
      return container;
    }
    #endregion

    public virtual IHighLevelContainer CreateContainer()
    {
      return new MachineContainer();
    }
  }
}