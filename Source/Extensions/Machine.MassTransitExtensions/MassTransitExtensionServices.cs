using System;
using System.Collections.Generic;

using Machine.Container;
using Machine.Container.Plugins;

using MassTransit.ServiceBus.Internal;
using MassTransit.ServiceBus.Subscriptions;
using MassTransit.ServiceBus.Subscriptions.ServerHandlers;

namespace Machine.MassTransitExtensions
{
  public class MassTransitExtensionServices : IServiceCollection
  {
    #region IServiceCollection Members
    public void RegisterServices(ContainerRegisterer register)
    {
      register.Type<MassTransitController>();
      register.Type<MassTransitUriFactory>();
      register.Type<FollowerRepository>();
      register.Type<EndpointResolver>();
      register.Type<LocalSubscriptionCache>();
      register.Type<HostedServicesController>();
      register.Type<MachineObjectBuilder>();
      register.Type<ServiceBusFactory>();
      register.Type<ServiceBusHubFactory>();
    }
    #endregion
  }
  public class SubscriptionManagerServices : IServiceCollection
  {
    #region IServiceCollection Members
    public void RegisterServices(ContainerRegisterer register)
    {
      register.Type<AddSubscriptionHandler>();
      register.Type<RemoveSubscriptionHandler>();
      register.Type<CancelUpdatesHandler>();
      register.Type<CacheUpdateRequestHandler>();
    }
    #endregion
  }
}