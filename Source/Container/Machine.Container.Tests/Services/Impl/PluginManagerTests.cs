using System;
using System.Collections.Generic;
using Machine.Container.Plugins;
using NUnit.Framework;

namespace Machine.Container.Services.Impl
{
  [TestFixture]
  public class PluginManager_NotInitialized : ScaffoldTests<PluginManager>
  {
    [Test]
    public void AddPlugin_Always_Just_Adds_It()
    {
      _target.AddPlugin(_mocks.DynamicMock<IServiceContainerPlugin>());
      _target.AddPlugin(_mocks.DynamicMock<IServiceContainerPlugin>());
    }

    [Test]
    public void Initialize_With_Plugin_Initializes_It()
    {
      IServiceContainerPlugin plugin = _mocks.DynamicMock<IServiceContainerPlugin>();
      _target.AddPlugin(plugin);
      plugin.Initialize(Get<IHighLevelContainer>());

      _mocks.ReplayAll();
      _target.Initialize();

      _mocks.Verify(plugin);
    }

    protected override PluginManager Create()
    {
      return new PluginManager(Get<IHighLevelContainer>());
    }
  }

  [TestFixture]
  public class PluginManager_Initialized : ScaffoldTests<PluginManager>
  {
    [Test]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddPlugin_Throws()
    {
      _target.Initialize();
      _target.AddPlugin(_mocks.DynamicMock<IServiceContainerPlugin>());
    }
  }
}
