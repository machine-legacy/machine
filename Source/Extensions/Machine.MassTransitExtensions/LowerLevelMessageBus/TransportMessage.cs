using System;

namespace Machine.MassTransitExtensions.LowerLevelMessageBus
{
  [Serializable]
  public class TransportMessage
  {
    private readonly EndpointName _returnAddress;
    private readonly Guid _id;
    private readonly Guid _correlationId;
    private readonly byte[] _body;

    public Guid Id
    {
      get { return _id; }
    }

    public Guid CorrelationId
    {
      get { return _correlationId; }
    }

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

    public TransportMessage(EndpointName returnAddress, Guid correlationId, byte[] body)
    {
      _id = Guid.NewGuid();
      _correlationId = correlationId;
      _returnAddress = returnAddress;
      _body = body;
    }

    public override string ToString()
    {
      return "TransportMessage from " + _returnAddress + " with " + _body.Length + "bytes";
    }
  }
}