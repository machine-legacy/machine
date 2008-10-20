using System;
using System.Collections.Generic;
using System.Reflection;

namespace Machine.MassTransitExtensions
{
  public class MessageEndpointLookup : IMessageEndpointLookup 
  {
    private readonly Dictionary<Type, List<EndpointName>> _map = new Dictionary<Type, List<EndpointName>>();
    private readonly List<EndpointName> _catchAlls = new List<EndpointName>();

    public ICollection<EndpointName> LookupEndpointFor(Type messageType)
    {
      List<EndpointName> destinations = new List<EndpointName>(_catchAlls);
      if (_map.ContainsKey(messageType))
      {
        destinations.AddRange(_map[messageType]);
      }
      if (destinations.Count == 0)
      {
        throw new InvalidOperationException("No endpoints for: " + messageType);
      }
      return destinations;
    }

    public void SendMessageTypeTo(Type messageType, EndpointName destination)
    {
      if (!_map.ContainsKey(messageType))
      {
        _map[messageType] = new List<EndpointName>();
      }
      _map[messageType].Add(destination);
    }

    public void SendMessageTypeTo<T>(EndpointName destination)
    {
      SendMessageTypeTo(typeof(T), destination);
    }

    public void SendAllFromAssemblyTo<T>(Assembly assembly, EndpointName destination)
    {
      foreach (Type type in assembly.GetTypes())
      {
        if (typeof(T).IsAssignableFrom(type))
        {
          SendMessageTypeTo(type, destination);
        }
      }
    }
    
    public void SendAllTo(EndpointName destination)
    {
      _catchAlls.Add(destination);
    }
  }
}