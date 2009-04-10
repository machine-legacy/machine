using Machine.Container;

namespace Machine.Migrations.Services
{
  public interface IMigratorContainerFactory
  {
    IMachineContainer CreateAndPopulateContainer(IConfiguration configuration);
  }
}