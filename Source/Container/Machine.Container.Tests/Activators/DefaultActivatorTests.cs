using System;
using System.Collections.Generic;

using Machine.Container.Activators;
using Machine.Container.Model;
using Machine.Container.Services;
using Machine.Container.Services.Impl;

using NUnit.Framework;

using Rhino.Mocks;

namespace Machine.Container.Activators
{
  [TestFixture]
  public class DefaultActivatorTests : ScaffoldTests<DefaultActivator>
  {
    #region Member Data
    private ServiceEntry _entry;
    private object _instance;
    private object _parameter1;
    private ResolvedConstructorCandidate _candidate;
    #endregion

    #region Test Setup and Teardown Methods
    public override void Setup()
    {
      _entry = ServiceEntryHelper.NewEntry();
      _instance = new object();
      _parameter1 = new object();
      base.Setup();
    }
    #endregion

    #region Test Methods
    [Test]
    [ExpectedException(typeof(YouFoundABugException))]
    public void Create_NotResolved_Throws()
    {
      using (_mocks.Record())
      {
      }
      using (_mocks.Playback())
      {
        _target.Activate(Get<IResolutionServices>());
      }
    }

    [Test]
    public void Create_NoDependencies_CallsConstructor()
    {
      using (_mocks.Record())
      {
        SetupMocks();
        Expect.Call(Get<IObjectFactory>().CreateObject(_candidate.Candidate, new object[0])).Return(_instance);
      }
      using (_mocks.Playback())
      {
        _target.CanActivate(Get<IResolutionServices>());
        Assert.AreEqual(_instance, _target.Activate(Get<IResolutionServices>()));
      }
    }

    [Test]
    public void Create_OneDependency_CallsConstructor()
    {
      ResolvedServiceEntry resolvedServiceEntry = new ResolvedServiceEntry(ServiceEntryHelper.NewEntry(), Get<IActivator>(), Get<IObjectInstances>());
      using (_mocks.Record())
      {
        SetupMocks(typeof(IService1));
        Expect.Call(Get<IServiceEntryResolver>().ResolveEntry(Get<IResolutionServices>(), typeof(IService1), true)).Return(resolvedServiceEntry);
        Expect.Call(Get<IActivator>().Activate(Get<IResolutionServices>())).Return(_parameter1);
        Expect.Call(Get<IObjectFactory>().CreateObject(_candidate.Candidate, new object[] {_parameter1})).Return(_instance);
      }
      using (_mocks.Playback())
      {
        _target.CanActivate(Get<IResolutionServices>());
        Assert.AreEqual(_instance, _target.Activate(Get<IResolutionServices>()));
      }
    }
    #endregion

    #region Methods
    protected virtual void SetupMocks(params Type[] dependencies)
    {
      _candidate = new ResolvedConstructorCandidate(CreateCandidate(typeof(Service1DependsOn2), dependencies), new List<ResolvedServiceEntry>());
      SetupResult.For(Get<IResolutionServices>().DependencyGraphTracker).Return(new DependencyGraphTracker());
      SetupResult.For(Get<IResolutionServices>().ActivatorStore).Return(Get<IActivatorStore>());
      SetupResult.For(Get<IServiceDependencyInspector>().SelectConstructor(typeof(Service1DependsOn2))).Return(_candidate.Candidate);
    }

    protected override DefaultActivator Create()
    {
      return new DefaultActivator(Get<IObjectFactory>(), Get<IServiceDependencyInspector>(), Get<IServiceEntryResolver>(), _entry);
    }
    #endregion
  }
}