using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace Machine.Core.Utility
{
  public class XmlSerializationHelper
  {
    public static string Serialize<T>(T obj)
    {
      XmlSerializer<T> serializer = new XmlSerializer<T>();
      return serializer.Serialize(obj);
    }

    public static T DeserializeString<T>(string value)
    {
      XmlSerializer<T> serializer = new XmlSerializer<T>();
      return serializer.DeserializeString(value);
    }
  }
  public class XmlSerializer<T>
  {
    private static readonly XmlSerializer _serializer = new XmlSerializer(typeof(T));

    public string Serialize(T obj)
    {
      StringWriter writer = new StringWriter();
      _serializer.Serialize(writer, obj);
      return writer.ToString();
    }

    public T DeserializeString(string value)
    {
      return (T)_serializer.Deserialize(new StringReader(value));
    }
  }
}