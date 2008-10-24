using System;
using System.Collections.Generic;

using Machine.Container;
using Machine.Container.Plugins;
using Machine.MassTransitExtensions.InterfacesAsMessages;
using Machine.MassTransitExtensions.LowerLevelMessageBus;

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
  public class LowerLevelMessageBusServices : IServiceCollection
  {
    #region IServiceCollection Members
    public void RegisterServices(ContainerRegisterer register)
    {
      register.Type<MessageEndpointLookup>();
      register.Type<MessageInterfaceTransportFormatter>();
      register.Type<TransportMessageBodySerializer>();
      register.Type<MessageInterfaceImplementations>();
      register.Type<MessageFactory>();
      register.Type<MessageDispatcher>();
      register.Type<MessageBusFactory>();
    }
    #endregion
  }
}