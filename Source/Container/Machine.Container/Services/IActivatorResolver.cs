using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services
{
  public interface IActivatorResolver
  {
    IActivator ResolveActivator(IResolutionServices services, ServiceEntry entry);
  }
  public interface IRootActivatorResolver : IActivatorResolver
  {
    void AddFirst(IActivatorResolver resolver);
    void AddAfter(Type type, IActivatorResolver resolver);
    void AddBefore(Type type, IActivatorResolver resolver);
    void AddLast(IActivatorResolver resolver);
  }
}
