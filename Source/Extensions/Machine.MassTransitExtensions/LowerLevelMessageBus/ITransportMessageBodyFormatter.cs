using System;
using System.IO;

namespace Machine.MassTransitExtensions.LowerLevelMessageBus
{
  public interface ITransportMessageBodyFormatter
  {
    void Serialize(IMessage[] messages, Stream stream);
    IMessage[] Deserialize(Stream stream);
  }
}