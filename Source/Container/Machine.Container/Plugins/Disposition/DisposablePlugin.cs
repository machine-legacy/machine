using System;
using System.Collections.Generic;
using System.Threading;

using Machine.Container.Model;
using Machine.Container.Services;
using Machine.Core.Utility;

namespace Machine.Container.Plugins.Disposition
{
  public class DisposablePlugin : AbstractServiceContainerListener, IServiceContainerPlugin
  {
    private readonly List<IDisposable> _disposables = new List<IDisposable>();
    private readonly ReaderWriterLock _lock = new ReaderWriterLock();

    public virtual void Initialize(PluginServices services)
    {
      services.Container.AddListener(this);
    }

    public virtual void ReadyForServices(PluginServices services)
    {
    }

    public override void InitializeListener(IMachineContainer container)
    {
    }

    public override void OnActivation(ResolvedServiceEntry entry, Activation activation)
    {
      IDisposable disposable = activation.Instance as IDisposable;
      if (disposable == null) return;
      using (RWLock.AsReader(_lock))
      {
        if (RWLock.UpgradeToWriterIf(_lock, () => { return !_disposables.Contains(disposable); }))
        {
          _disposables.Add(disposable);
        }
      }
    }

    public override void OnDeactivation(ResolvedServiceEntry entry, Deactivation deactivation)
    {
      IDisposable disposable = deactivation.Instance as IDisposable;
      if (disposable == null) return;
      using (RWLock.AsWriter(_lock))
      {
        if (_disposables.Contains(disposable))
        {
          DisposeOf(disposable);
        }
      }
    }

    public override void Dispose()
    {
      using (RWLock.AsWriter(_lock))
      {
        _disposables.Reverse();
        while (_disposables.Count > 0)
        {
          DisposeOf(_disposables[0]);
        }
      }
    }

    private void DisposeOf(IDisposable disposable)
    {
      disposable.Dispose();
      _disposables.Remove(disposable);
    }
  }
}
