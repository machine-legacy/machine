using System;

using Machine.Container.Model;

namespace Machine.Container.Services.Impl
{
  public class RootActivatorFactoryChain : Chain<IActivatorFactory>, IRootActivatorFactory
  {
    #region IActivatorFactory Members
    public IActivator CreateStaticActivator(ServiceEntry entry, object instance)
    {
      foreach (IActivatorFactory resolver in this.ChainItems)
      {
        IActivator activator = resolver.CreateStaticActivator(entry, instance);
        if (activator != null)
        {
          return activator;
        }
      }
      throw new ServiceContainerException("Unable to create static Activator for: " + entry);
    }

    public IActivator CreateDefaultActivator(ServiceEntry entry)
    {
      foreach (IActivatorFactory resolver in this.ChainItems)
      {
        IActivator activator = resolver.CreateDefaultActivator(entry);
        if (activator != null)
        {
          return activator;
        }
      }
      throw new ServiceContainerException("Unable to create default Activator for: " + entry);
    }
    #endregion
  }
}
