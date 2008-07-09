using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services
{
  public interface IActivatorResolver
  {
    IActivator ResolveActivator(IResolutionServices services, ServiceEntry entry);
  }

  public interface IRootActivatorResolver : IActivatorResolver, IChain<IActivatorResolver>
  {
  }
}
