using System;

namespace Machine.MassTransitExtensions
{
  public class MassTransitActiveMqUriFactory : IMassTransitUriFactory
  {
    public Uri CreateUri(string name)
    {
      return CreateUri("127.0.0.1", name);
    }

    public Uri CreateUri(string address, string name)
    {
      return new Uri("activemq://" + address + ":61616/" + name);
    }

    public Uri CreateUri(Uri uri)
    {
      return new Uri("activemq://" + uri.Host + ":61616" + uri.AbsolutePath);
    }
  }
}