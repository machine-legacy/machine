using System;

namespace Machine.MassTransitExtensions
{
  public interface IMassTransitUriFactory
  {
    Uri CreateUri(string name);
    Uri CreateUri(string address, string name);
    Uri CreateUri(Uri uri);
  }
}