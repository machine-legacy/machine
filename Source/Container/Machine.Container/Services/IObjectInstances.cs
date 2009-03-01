using Machine.Container.Model;

namespace Machine.Container.Services
{
  public interface IObjectInstances
  {
    void Remember(ResolvedServiceEntry entry, Activation activation);
    void Deactivate(IResolutionServices services, object instance);
    void DeactivateAll(IResolutionServices services);
  }
}