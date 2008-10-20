using System;
using System.Collections.Generic;
using System.Reflection;

namespace Machine.MassTransitExtensions
{
  public interface IMessageEndpointLookup
  {
    ICollection<EndpointName> LookupEndpointFor(Type messageType);
    void SendMessageTypeTo(Type messageType, EndpointName destination);
    void SendMessageTypeTo<T>(EndpointName destination);
    void SendAllFromAssemblyTo<T>(Assembly assembly, EndpointName destination);
    void SendAllTo(EndpointName destination);
  }
}