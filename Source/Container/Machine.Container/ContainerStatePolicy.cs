using System;

namespace Machine.Container
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

    public void AssertCanResolve()
    {
      AssertState("Resolve Service", ContainerState.Started);
    }

    public void AssertCanRelease()
    {
      AssertState("Release Service", ContainerState.Started);
    }

    public void AssertCanAddServices()
    {
      AssertState("Register Service", ContainerState.ServiceRegistration, ContainerState.Started);
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
  }
}