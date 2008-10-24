using System;
using System.Collections.Generic;

using Machine.Container.Services;
using Machine.MassTransitExtensions.LowerLevelMessageBus;

namespace Machine.MassTransitExtensions
{
  public class MessageBusManager : IMessageBusManager
  {
    private readonly IMachineContainer _container;
    private readonly IMessageBusFactory _messageBusFactory;

    public MessageBusManager(IMessageBusFactory messageBusFactory, IMachineContainer container)
    {
      _messageBusFactory = messageBusFactory;
      _container = container;
    }

    #region IMessageBusManager Members
    public void UseSingleBus(EndpointName local)
    {
      IMessageBus bus = _messageBusFactory.CreateMessageBus(local);
      _container.Register.Type<IMessageBus>().Is(bus);
    }
    #endregion
  }
}
