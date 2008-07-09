using System;
using System.Collections.Generic;

namespace Machine.Container.Model
{
  public class ContainerStatePolicy
  {
    public enum ContainerState
    {
      Unknown,
      PluginRegistration,
      ServiceRegistration,
      Started
    }

    private readonly List<SupportedFeature> _supportedFeatures = new List<SupportedFeature>();
    private ContainerState _state = ContainerState.Unknown;

    private void AssertState(string attempting, params ContainerState[] states)
    {
      if (Array.IndexOf(states, _state) == -1)
      {
        throw new ServiceContainerException(String.Format("You may not do that when the container is in the {0} state!\n{1}", _state, attempting));
      }
    }

    public void AssertCanAddPlugins()
    {
      AssertState("Add Plugin", ContainerState.PluginRegistration);
    }

    public void AssertCanAddListeners()
    {
      AssertState("Add Listener", ContainerState.PluginRegistration);
    }

    public void AssertCanActivate()
    {
      AssertState("Activate", ContainerState.ServiceRegistration, ContainerState.Started);
    }

    public void AssertCanDeactivate()
    {
      AssertState("Deactivate", ContainerState.Started);
    }

    public void AssertCanAddServices()
    {
      AssertState("Register", ContainerState.ServiceRegistration, ContainerState.Started);
    }

    public void AssertIsInitialized()
    {
      AssertState("Is Initialized", ContainerState.ServiceRegistration, ContainerState.Started);
    }

    public void Initialize()
    {
      AssertState("Initialize", ContainerState.Unknown);
      _state = ContainerState.PluginRegistration;
    }

    public void PrepareForServices()
    {
      AssertState("Prepare For Service Registration", ContainerState.PluginRegistration);
      _state = ContainerState.ServiceRegistration;
    }

    public void Start()
    {
      AssertState("Starting Container", ContainerState.ServiceRegistration);
      _state = ContainerState.Started;
    }

    public void AddSupportedFeature(SupportedFeature feature)
    {
      lock (_supportedFeatures)
      {
        _supportedFeatures.Add(feature);
      }
    }

    public void AssertSupports(SupportedFeature feature)
    {
      if (!_supportedFeatures.Contains(feature))
      {
        throw new ServiceContainerException(String.Format("Support for the feature {0} is missing!", feature));
      }
    }
  }
}