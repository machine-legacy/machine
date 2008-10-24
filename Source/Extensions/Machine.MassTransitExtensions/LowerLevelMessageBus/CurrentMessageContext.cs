using System;
using System.Collections.Generic;

namespace Machine.MassTransitExtensions.LowerLevelMessageBus
{
  public class CurrentMessageContext : IDisposable
  {
    [ThreadStatic]
    static CurrentMessageContext _current;
    readonly TransportMessage _transportMessage;
    IMessage _applicationMessage;

    public static CurrentMessageContext Current
    {
      get { return _current; }
    }

    public TransportMessage TransportMessage
    {
      get { return _transportMessage; }
    }

    public IMessage CurrentApplicationMessage
    {
      get { return _applicationMessage; }
      set { _applicationMessage = value; }
    }

    public CurrentMessageContext(TransportMessage transportMessage)
    {
      _transportMessage = transportMessage;
    }

    public static CurrentMessageContext Open(TransportMessage transportMessage)
    {
      return _current = new CurrentMessageContext(transportMessage);
    }

    #region IDisposable Members
    public void Dispose()
    {
      _current = null;
    }
    #endregion
  }
}