using System;

namespace Machine.MassTransitExtensions
{
  [Serializable]
  public class TransportMessage
  {
    private readonly EndpointName _returnAddress;
    private readonly byte[] _body;

    public EndpointName ReturnAddress
    {
      get { return _returnAddress; }
    }

    public byte[] Body
    {
      get { return _body; }
    }

    protected TransportMessage()
    {
    }

    public TransportMessage(EndpointName returnAddress, byte[] body)
    {
      _returnAddress = returnAddress;
      _body = body;
    }

    public override string ToString()
    {
      return "TransportMessage from " + _returnAddress + " with " + _body.Length + "bytes";
    }
  }
}