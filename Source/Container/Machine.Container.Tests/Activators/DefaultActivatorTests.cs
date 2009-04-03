using System;
using System.Collections.Generic;

using Machine.Container.Activators;
using Machine.Container.Model;
using Machine.Container.Services;
using Machine.Container.Services.Impl;
using Machine.Core;

using NUnit.Framework;

using Rhino.Mocks;

namespace Machine.Container.Activators
{
  [TestFixture]
  public class DefaultActivatorTests : ScaffoldTests<DefaultActivator>
  {
    private ServiceEntry _entry;
    private object _instance;
    private object _parameter1;
    private ResolvedConstructorCandidate _candidate;

    public override void Setup()
    {
      _entry = ServiceEntryHelper.NewEntry();
      _instance = new SimpleService1();
      _parameter1 = new SimpleService1();
      base.Setup();
    }

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
      Activation activation = new Activation(_entry, _instance);
      using (_mocks.Record())
      {
        SetupMocks();
        Expect.Call(Get<IObjectFactory>().CreateObject(_candidate.Candidate, new object[0])).Return(_instance);
      }
      using (_mocks.Playback())
      {
        _target.CanActivate(Get<IResolutionServices>());
        Assert.AreEqual(activation, _target.Activate(Get<IResolutionServices>()));
      }
    }

    [Test]
    public void Create_OneDependency_CallsConstructor()
    {
      ServiceEntry entry = ServiceEntryHelper.NewEntry();
      ResolvedServiceEntry resolvedServiceEntry = new ResolvedServiceEntry(entry, Get<IActivator>(), Get<IObjectInstances>());
      Activation parameterActivation = new Activation(entry, _parameter1);
      Activation activation = new Activation(_entry, _instance);
      ResolvableType resolvableType = new ResolvableType(Get<IServiceGraph>(), Get<IServiceEntryFactory>(), typeof(IService1));
      using (_mocks.Record())
      {
        SetupMocks(typeof(IService1));
        Expect.Call(Get<IResolvableTypeMap>().FindResolvableType(_candidate.Candidate.Dependencies[0])).Return(resolvableType);
        Expect.Call(Get<IServiceEntryResolver>().ResolveEntry(Get<IResolutionServices>(), resolvableType)).Return(resolvedServiceEntry);
        Expect.Call(Get<IActivator>().Activate(Get<IResolutionServices>())).Return(parameterActivation);
        Expect.Call(Get<IObjectFactory>().CreateObject(_candidate.Candidate, new object[] { _parameter1 })).Return(_instance);
      }
      using (_mocks.Playback())
      {
        _target.CanActivate(Get<IResolutionServices>());
        Assert.AreEqual(activation, _target.Activate(Get<IResolutionServices>()));
      }
    }

    protected virtual void SetupMocks(params Type[] dependencies)
    {
      _candidate = new ResolvedConstructorCandidate(CreateCandidate(typeof(SimpleService1), dependencies), new List<ResolvedServiceEntry>());
      SetupResult.For(Get<IResolutionServices>().DependencyGraphTracker).Return(new DependencyGraphTracker());
      SetupResult.For(Get<IResolutionServices>().ActivatorStore).Return(Get<IActivatorStore>());
      SetupResult.For(Get<IResolutionServices>().ResolvableTypeMap).Return(Get<IResolvableTypeMap>());
      SetupResult.For(Get<IServiceDependencyInspector>().SelectConstructor(typeof(SimpleService1))).Return(_candidate.Candidate);
    }

    protected override DefaultActivator Create()
    {
      return new DefaultActivator(Get<IObjectFactory>(), Get<IServiceDependencyInspector>(), Get<IServiceEntryResolver>(), _entry);
    }
  }
}