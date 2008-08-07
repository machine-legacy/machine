using System;
using System.Collections.Generic;

using Machine.Container.Plugins;

using MassTransit.ServiceBus.Internal;
using MassTransit.ServiceBus.Subscriptions;
using MassTransit.SubscriptionStorage;

namespace Machine.MassTransitExtensions
{
  public class MassTransitPlugin : IServiceContainerPlugin
  {
    #region IServiceContainerPlugin Members
    public virtual void Initialize(PluginServices services)
    {
    }

    public virtual void ReadyForServices(PluginServices services)
    {
      services.Container.Register.Type<IMassTransit>().ImplementedBy<MassTransitController>();
      services.Container.Register.Type<FollowerRepository>();
      services.Container.Register.Type<EndpointResolver>();
      services.Container.Register.Type<LocalSubscriptionCache>();
      services.Container.Register.Type<HostedServicesController>();
      services.Container.Register.Type<MachineObjectBuilder>();
      services.Container.Register.Type<NHibernateSubscriptionStorage>();
    }
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
    }
    #endregion
  }
}