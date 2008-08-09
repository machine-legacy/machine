using Machine.Container.Services;
using Machine.Container.Services.Impl;

namespace Machine.Container.Plugins.ParentContainers
{
  public class ParentContainer : IServiceContainerPlugin
  {
    private readonly IMachineContainer _container;

    public ParentContainer(IMachineContainer container)
    {
      _container = container;
    }

    #region IServiceContainerPlugin Members
    public void Initialize(PluginServices services)
    {
      services.Resolver.AddBefore(typeof(ThrowsPendingActivatorResolver), new ParentContainerActivatorResolver(_container));
    }

    public void ReadyForServices(PluginServices services)
    {
    }
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
    }
    #endregion
  }
}