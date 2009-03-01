using System;
using System.Collections.Generic;

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
      activation.AssertIsFullyActivated();
      if (_trackingPolicy.Remember(entry, activation) == TrackingStatus.New)
      {
        _listenerInvoker.OnActivation(entry, activation);
        entry.IncrementActiveInstances();
        _log.Info("Remembering: " + entry + " - " + activation);
      }
    }

    public void Deactivate(IResolutionServices services, object instance)
    {
      Deactivation deactivation = new Deactivation(instance);
      RememberedActivation rememberedActivation = _trackingPolicy.RetrieveAndForget(instance);
      if (rememberedActivation != null)
      {
        _listenerInvoker.OnDeactivation(rememberedActivation.ResolvedEntry, deactivation);
        rememberedActivation.Deactivate(services);
      }
    }

    public void DeactivateAll(IResolutionServices services)
    {
      foreach (RememberedActivation rememberedActivation in _trackingPolicy.RetrieveAndForgetAll())
      {
        Deactivation deactivation = rememberedActivation.ToDeactivation();
        _listenerInvoker.OnDeactivation(rememberedActivation.ResolvedEntry, deactivation);
        rememberedActivation.Deactivate(services);
      }
    }
  }
}