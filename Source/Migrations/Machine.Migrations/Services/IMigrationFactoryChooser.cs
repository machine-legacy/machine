
namespace Machine.Migrations.Services
{
  public interface IMigrationFactoryChooser
  {
    IMigrationFactory ChooseFactory(MigrationReference migrationReference);
  }
}
