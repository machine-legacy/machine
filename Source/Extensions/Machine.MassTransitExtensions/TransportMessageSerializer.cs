using System;
using System.Collections.Generic;
using System.IO;

namespace Machine.MassTransitExtensions
{
  public class TransportMessageSerializer
  {
    private readonly ITransportMessageBodyFormatter _transportMessageBodyFormatter;

    public TransportMessageSerializer(ITransportMessageBodyFormatter transportMessageBodyFormatter)
    {
      _transportMessageBodyFormatter = transportMessageBodyFormatter;
    }

    public TransportMessage Serialize<T>(IMessageBus bus, params T[] messages) where T : IMessage
    {
      using (MemoryStream stream = new MemoryStream())
      {
        IMessage[] nonGeneric = new IMessage[messages.Length];
        Array.Copy(messages, nonGeneric, nonGeneric.Length);
        _transportMessageBodyFormatter.Serialize(nonGeneric, stream);
        return new TransportMessage(bus.Address, stream.ToArray());
      }
    }

    public ICollection<IMessage> Deserialize(TransportMessage transportMessage)
    {
      using (MemoryStream stream = new MemoryStream(transportMessage.Body))
      {
        List<IMessage> messages = new List<IMessage>();
        messages.AddRange(_transportMessageBodyFormatter.Deserialize(stream));
        return messages;
      }
    }
  }
}
