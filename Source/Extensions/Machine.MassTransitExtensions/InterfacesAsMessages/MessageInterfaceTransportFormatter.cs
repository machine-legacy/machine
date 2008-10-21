using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace Machine.MassTransitExtensions.InterfacesAsMessages
{
  public class MessageInterfaceTransportFormatter : ITransportMessageBodyFormatter
  {
    private readonly MessageInterfaceImplementations _messageInterfaceImplementor;

    public MessageInterfaceTransportFormatter(MessageInterfaceImplementations messageInterfaceImplementor)
    {
      _messageInterfaceImplementor = messageInterfaceImplementor;
    }

    #region ITransportMessageBodyFormatter Members
    public void Serialize(IMessage[] messages, Stream stream)
    {
      using (StreamWriter writer = new StreamWriter(stream))
      {
        InterfaceMessageJsonConverter converter = new InterfaceMessageJsonConverter(_messageInterfaceImplementor, false);
        JsonSerializer serializer = new JsonSerializer();
        serializer.Converters.Add(converter);
        serializer.Serialize(writer, messages);
      }
    }

    public IMessage[] Deserialize(Stream stream)
    {
      using (StreamReader reader = new StreamReader(stream))
      {
        InterfaceMessageJsonConverter converter = new InterfaceMessageJsonConverter(_messageInterfaceImplementor, true);
        JsonSerializer serializer = new JsonSerializer();
        serializer.Converters.Add(converter);
        return (IMessage[])serializer.Deserialize(new JsonReader(reader), typeof(IMessage[]));
      }
    }
    #endregion
  }

  public class InterfaceMessageJsonConverter : JsonConverter
  {
    private static readonly string MessageTypePropertyName = "MessageType";
    private static readonly string MessageBodyPropertyName = "MessageBody";
    private readonly MessageInterfaceImplementations _messageInterfaceImplementor;
    private readonly bool _ignoreClassesBecauseWeAreReading;
    private bool _skipNext;

    public InterfaceMessageJsonConverter(MessageInterfaceImplementations messageInterfaceImplementor, bool reading)
    {
      _messageInterfaceImplementor = messageInterfaceImplementor;
      _ignoreClassesBecauseWeAreReading = reading;
    }

    public override bool CanConvert(Type objectType)
    {
      if (_skipNext)
      {
        return _skipNext = false;
      }
      if (typeof(IMessage).IsAssignableFrom(objectType))
      {
        return !_ignoreClassesBecauseWeAreReading || objectType.IsInterface;
      }
      return false;
    }

    public override object ReadJson(JsonReader reader, Type objectType)
    {
      if (reader.TokenType == JsonToken.Null)
      {
        return null;
      }
      reader.Read();
      System.Diagnostics.Debug.Assert(reader.Value.Equals(MessageTypePropertyName));
      
      reader.Read();
      string messageTypeName = reader.Value.ToString();
      
      reader.Read();
      System.Diagnostics.Debug.Assert(reader.Value.Equals(MessageBodyPropertyName));
     
      // Allow interfaces and non-interfaces...
      Type deserializeAs = Type.GetType(messageTypeName);
      if (deserializeAs.IsInterface)
      {
        deserializeAs = _messageInterfaceImplementor.GetClassFor(deserializeAs);
      }
      JsonSerializer serializer = new JsonSerializer();
      serializer.Converters.Add(this);
      object value = serializer.Deserialize(reader, deserializeAs);
      reader.Read();
      return value;
    }

    public override void WriteJson(JsonWriter writer, object value)
    {
      Type objectType = value.GetType();
      Type messageType = objectType;
      // Allow interfaces and non-interfaces...
      if (_messageInterfaceImplementor.IsClassOrInterface(objectType))
      {
        messageType = _messageInterfaceImplementor.GetInterfaceFor(value.GetType());
      }
      writer.WriteStartObject();
      writer.WritePropertyName(MessageTypePropertyName);
      writer.WriteValue(messageType.FullName);
      writer.WritePropertyName(MessageBodyPropertyName);
      JsonSerializer serializer = new JsonSerializer();
      serializer.Converters.Add(this);
      _skipNext = true;
      serializer.Serialize(writer, value);
      writer.WriteEndObject();
    }
  }

  public class InterfaceFormatterTests
  {
    public interface IUserCreated : IMessage
    {
      Guid UserId { get; set; }
      DateTime CreatedAt { get; set; }
    }

    public interface IHasChildren : IMessage
    {
      IUserCreated First { get; set; }
      IUserCreated Second { get; set; }
    }
    public void Test()
    {
      MessageInterfaceImplementations messageInterfaceImplementor = new MessageInterfaceImplementations();
      messageInterfaceImplementor.GenerateImplementationsOf(typeof(IUserCreated), typeof(IHasChildren));
      IMessageFactory factory = new MessageFactory(messageInterfaceImplementor);
      MemoryStream stream = new MemoryStream();
      MessageInterfaceTransportFormatter formatter = new MessageInterfaceTransportFormatter(messageInterfaceImplementor);
      List<IMessage> messages = new List<IMessage>();
      IUserCreated created = factory.Create<IUserCreated>();
      created.UserId = Guid.NewGuid();
      created.CreatedAt = DateTime.UtcNow;
      messages.Add(created);
      IHasChildren hasChildren = factory.Create<IHasChildren>();
      hasChildren.First = factory.Create<IUserCreated>();
      hasChildren.First.UserId = Guid.NewGuid();
      hasChildren.Second = factory.Create<IUserCreated>();
      hasChildren.Second.UserId = Guid.NewGuid();
      messages.Add(hasChildren);
      formatter.Serialize(messages.ToArray(), stream);
      string text = System.Text.Encoding.Default.GetString(stream.ToArray());
      Console.WriteLine(text);
      IMessage[] received = formatter.Deserialize(new MemoryStream(stream.ToArray()));
      foreach (IMessage msg in received)
      {
        Console.WriteLine("Received: " + msg);
      }
    }
  }
}