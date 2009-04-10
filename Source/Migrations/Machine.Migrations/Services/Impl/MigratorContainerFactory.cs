using Machine.Container;
using Machine.Core.Services;
using Machine.Core.Services.Impl;
using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;

namespace Machine.Migrations.Services.Impl
{
  public class MigratorContainerFactory : IMigratorContainerFactory
  {
    #region IMigratorContainerFactory Members
    public virtual IMachineContainer CreateAndPopulateContainer(IConfiguration configuration)
    {
      IMachineContainer container = CreateContainer();
      container.Initialize();
      container.PrepareForServices();
      container.Register.Type(configuration.ConnectionProviderType);
      container.Register.Type(configuration.TransactionProviderType);
      container.Register.Type(configuration.DatabaseProviderType);
      container.Register.Type(configuration.SchemaProviderType);
      container.Register.Type<FileSystem>();
      container.Register.Type<Namer>();
      container.Register.Type<SchemaStateManager>();
      container.Register.Type<MigrationFinder>();
      container.Register.Type<MigrationSelector>();
      container.Register.Type<MigrationRunner>();
      container.Register.Type<MigrationInitializer>();
      container.Register.Type<Migrator>();
      container.Register.Type<MigrationFactoryChooser>();
      container.Register.Type<VersionStateFactory>();
      container.Register.Type<WorkingDirectoryManager>();
      container.Register.Type<CommonTransformations>();
      container.Register.Type<IConfiguration>().Is(configuration);
      container.Register.Type<CSharpMigrationFactory>();
      container.Register.Type<BooMigrationFactory>();
      container.Register.Type<SqlScriptMigrationFactory>();
      container.Start();
      return container;
    }
    #endregion

    public virtual IMachineContainer CreateContainer()
    {
      return new MachineContainer();
    }
  }
}