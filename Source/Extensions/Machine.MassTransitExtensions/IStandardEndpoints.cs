using System.Collections.Generic;

using MassTransit.ServiceBus;

namespace Machine.MassTransitExtensions
{
  public interface IStandardEndpoints
  {
    IEndpoint DefaultLocalEndpoint
    {
      get;
    }

    IEndpoint SubscriptionManagerEndpoint
    {
      get;
    }
  }
}
