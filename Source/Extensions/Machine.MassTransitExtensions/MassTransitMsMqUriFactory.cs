using System;

namespace Machine.MassTransitExtensions
{
  public class MassTransitMsMqUriFactory : IMassTransitUriFactory
  {
    public Uri CreateUri(string name)
    {
      return CreateUri("localhost", name);
    }

    public Uri CreateUri(string address, string name)
    {
      return new Uri("msmq://" + address + "/" + name);
    }

    public Uri CreateUri(Uri uri)
    {
      return new Uri("msmq://" + uri.Host + uri.AbsolutePath);
    }
  }
}