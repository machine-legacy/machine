using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Plugins.Disposition
{
  public class DisposablePlugin : AbstractServiceContainerListener, IServiceContainerPlugin
  {
    private readonly List<IDisposable> _disposables = new List<IDisposable>();
    private bool _initialized;

    #region IServiceContainerListener Members
    public override void Initialize(IMachineContainer container)
    {
      if (_initialized) return;
      container.AddListener(this);
      _initialized = true;
    }

    public override void InstanceCreated(ResolvedServiceEntry entry, Activation activation)
    {
      IDisposable disposable = activation.Instance as IDisposable;
      if (disposable == null) return;
      if (!_disposables.Contains(disposable))
      {
        _disposables.Add(disposable);
      }
    }

    public override void InstanceReleased(ResolvedServiceEntry entry, Deactivation deactivation)
    {
      IDisposable disposable = deactivation.Instance as IDisposable;
      if (disposable == null) return;
      disposable.Dispose();
      _disposables.Remove(disposable);
    }
    #endregion

    #region IDisposable Members
    public override void Dispose()
    {
      while (_disposables.Count > 0)
      {
        _disposables[0].Dispose();
        _disposables.RemoveAt(0);
      }
    }
    #endregion
  }
}
