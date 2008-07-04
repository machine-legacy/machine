using System;
using System.Collections.Generic;
using System.Threading;

using Machine.Container.Model;
using Machine.Container.Plugins;

namespace Machine.Container.Services.Impl
{
  public class ObjectInstances : IObjectInstances
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ObjectInstances));

    private readonly IListenerInvoker _listenerInvoker;
    private readonly IInstanceTrackingPolicy _trackingPolicy;

    public ObjectInstances(IListenerInvoker listenerInvoker, IInstanceTrackingPolicy trackingPolicy)
    {
      _listenerInvoker = listenerInvoker;
      _trackingPolicy = trackingPolicy;
    }

    public void Remember(ResolvedServiceEntry entry, Activation activation)
    {
      if (_trackingPolicy.Remember(entry, activation) == TrackingStatus.New)
      {
        _listenerInvoker.InstanceCreated(entry, activation);
        entry.IncrementActiveInstances();
        _log.Info("Remembering: " + entry + " - " + activation);
      }
    }

    public void Release(IResolutionServices services, object instance)
    {
      Deactivation deactivation = new Deactivation(instance);
      ResolvedServiceEntry resolvedEntry = _trackingPolicy.RetrieveAndForget(instance);
      _listenerInvoker.InstanceReleased(resolvedEntry, deactivation);
      resolvedEntry.DecrementActiveInstances();
      resolvedEntry.Release(services, instance);
    }
  }
}