using System;
using System.Collections.Generic;

namespace Machine.MassTransitExtensions
{
  public class EndpointName
  {
    private readonly string _address;
    private readonly string _name;

    public string Address
    {
      get { return _address; }
    }

    public string Name
    {
      get { return _name; }
    }

    protected EndpointName()
    {
      _name = null;
    }

    protected EndpointName(string address, string name)
    {
      if (String.IsNullOrEmpty(address)) throw new ArgumentNullException("address");
      if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
      _address = address;
      _name = name;
    }

    public override bool Equals(object obj)
    {
      EndpointName other = obj as EndpointName;
      if (other != null)
      {
        return other.Address.Equals(this.Address) && other.Name.Equals(this.Name);
      }
      return false;
    }

    public override Int32 GetHashCode()
    {
      return _address.GetHashCode() ^ _name.GetHashCode();
    }

    public static readonly EndpointName Null = new EndpointName();

    public static EndpointName ForRemoteQueue(string address, string queue)
    {
      return new EndpointName(address, queue);
    }

    public static EndpointName ForLocalQueue(string queue)
    {
      return ForRemoteQueue("localhost", queue);
    }
  }
}
