using Machine.Container.Model;

namespace Machine.Container.Services
{
  public interface IObjectInstances
  {
    void Remember(ResolvedServiceEntry entry, Activation activation);
    void Release(IResolutionServices services, object instance);
  }
}