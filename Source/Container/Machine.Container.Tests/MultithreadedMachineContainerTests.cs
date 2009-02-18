using System;
using System.Collections.Generic;
using System.Threading;

using Machine.Container.Model;
using Machine.Container.Plugins;
using Machine.Container.Plugins.Disposition;
using Machine.Container.Services;
using Machine.Core.Utility;

using NUnit.Framework;

namespace Machine.Container
{
  public class Creation
  {
    private readonly ResolvedServiceEntry _entry;
    private readonly Activation _activation;

    public ResolvedServiceEntry Entry
    {
      get { return _entry; }
    }

    public object Instance
    {
      get { return _activation.Instance; }
    }

    public Creation(ResolvedServiceEntry entry, Activation activation)
    {
      _entry = entry;
      _activation = activation;
    }
  }
  public class ServiceCreations
  {
    private readonly List<Creation> _creations = new List<Creation>();

    public void Clear()
    {
      _creations.Clear();
    }

    public void Add(ResolvedServiceEntry entry, Activation activation)
    {
      _creations.Add(new Creation(entry, activation));
    }

    public IDictionary<Type, List<Creation>> GroupByType()
    {
      Dictionary<Type, List<Creation>> grouped = new Dictionary<Type, List<Creation>>();
      foreach (Creation creation in _creations)
      {
        Type type = creation.Instance.GetType();
        if (!grouped.ContainsKey(type))
        {
          grouped[type] = new List<Creation>();
        }
        grouped[type].Add(creation);
      }
      return grouped;
    }
  }
  [TestFixture]
  public class MultithreadedMachineContainerTests : IServiceContainerListener
  {
    #region Member Data
    private MachineContainer _machineContainer;
    private readonly List<Thread> _threads = new List<Thread>();
    private readonly ServiceCreations _creations = new ServiceCreations();
    #endregion

    #region Test Setup and Teardown Methods
    [SetUp]
    public virtual void Setup()
    {
      _threads.Clear();
      _machineContainer = new MachineContainer();
      _machineContainer.Initialize();
      _machineContainer.AddListener(this);
      _machineContainer.AddPlugin(new DisposablePlugin());
      _machineContainer.PrepareForServices();
      _machineContainer.Start();
      log4net.Appender.OutputDebugStringAppender appender = new log4net.Appender.OutputDebugStringAppender();
      appender.Layout = new log4net.Layout.PatternLayout("%-5p %c{1} %m");
      log4net.Config.BasicConfigurator.Configure(appender);
    }
    #endregion

    [Test]
    [Ignore]
    public void Multiple_Threads_Resolving()
    {
      ReaderWriterLockStatistics statistics = new ReaderWriterLockStatistics();

      _machineContainer.Register.Type<Service1DependsOn2>().AsTransient();
      _machineContainer.Register.Type<SimpleService2>().AsSingleton();
      ThreadStart start = delegate()
      {
        for (int i = 0; i < 20; ++i)
        {
          Service1DependsOn2 service = _machineContainer.Resolve.Object<Service1DependsOn2>();
          _machineContainer.Deactivate(service);
        }
        PerThreadUsages.CopyThreadUsagesToMainCollection(statistics);
      };
      for (int i = 0; i < 30; ++i)
      {
        AddThread(start);
      }
      JoinAllThreads();
      PerThreadUsages.CopyThreadUsagesToMainCollection(statistics);

      ReaderWriterLockStatistics.Report report = statistics.CreateReport();
      Console.WriteLine(report.ToAscii());

      IDictionary<Type, List<Creation>> grouped = _creations.GroupByType();
      Assert.AreEqual(1, grouped[typeof(SimpleService2)].Count);
      Assert.AreEqual(600, grouped[typeof(Service1DependsOn2)].Count);
    }

    [TearDown]
    public void Teardown()
    {
      JoinAllThreads();
    }

    private void JoinAllThreads()
    {
      while (_threads.Count > 0)
      {
        _threads[0].Join();
        _threads.RemoveAt(0);
      }
    }

    private void AddThread(ThreadStart start)
    {
      Thread thread = new Thread(start);
      thread.Start();
      _threads.Add(thread);
    }

    #region IServiceContainerListener Members
    public void InitializeListener(IMachineContainer container)
    {
    }

    public void PreparedForServices()
    {
    }

    public void OnRegistration(ServiceEntry entry)
    {
    }

    public void Started()
    {
      _creations.Clear();
    }

    public void OnActivation(ResolvedServiceEntry entry, Activation activation)
    {
      _creations.Add(entry, activation);
    }

    public void OnDeactivation(ResolvedServiceEntry entry, Deactivation deactivation)
    {
    }
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
    }
    #endregion
  }
}