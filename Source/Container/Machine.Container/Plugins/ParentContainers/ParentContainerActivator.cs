using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Plugins.ParentContainers
{
  public class ParentContainerActivator : IActivator
  {
    private readonly IMachineContainer _container;
    private readonly ServiceEntry _entry;

    public ParentContainerActivator(IMachineContainer container, ServiceEntry entry)
    {
      _container = container;
      _entry = entry;
    }

    #region IActivator Members
    public bool CanActivate(IResolutionServices services)
    {
      return _container.CanResolve(_entry.ServiceType);
    }

    public Activation Activate(IResolutionServices services)
    {
      /* TODO: Pass the overrides down somehow? */
      object instance = _container.Resolve.Object(_entry.ServiceType);
      return new Activation(_entry, instance, false);
    }

    public void Deactivate(IResolutionServices services, object instance)
    {
      _container.Deactivate(instance);
    }
    #endregion
  }
}