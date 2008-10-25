using System;
using System.Collections.Generic;

using Machine.Container.Services;
using Machine.MassTransitExtensions.LowerLevelMessageBus;

namespace Machine.MassTransitExtensions
{
  public class MessageBusManager : IMessageBusManager, IDisposable
  {
    private readonly IMachineContainer _container;
    private readonly IMessageBusFactory _messageBusFactory;
    private IMessageBus _bus;

    public MessageBusManager(IMessageBusFactory messageBusFactory, IMachineContainer container)
    {
      _messageBusFactory = messageBusFactory;
      _container = container;
    }

    #region IMessageBusManager Members
    public IMessageBus UseSingleBus(EndpointName local)
    {
      _bus = _messageBusFactory.CreateMessageBus(local);
      _container.Register.Type<IMessageBus>().Is(_bus);
      return _bus;
    }
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
      if (_bus != null)
      {
        _bus.Dispose();
      }
    }
    #endregion
  }
}
