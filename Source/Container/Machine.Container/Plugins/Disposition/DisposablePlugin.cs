using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Plugins.Disposition
{
  public class DisposablePlugin : AbstractServiceContainerListener, IServiceContainerPlugin
  {
    private readonly List<IDisposable> _disposables = new List<IDisposable>();

    #region IServiceContainerListener Members
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
      /* Get Reader Lock */
      if (!_disposables.Contains(disposable))
      {
        /* Get Writer Lock */
        _disposables.Add(disposable);
      }
    }

    public override void OnDeactivation(ResolvedServiceEntry entry, Deactivation deactivation)
    {
      IDisposable disposable = deactivation.Instance as IDisposable;
      if (disposable == null) return;
      /* Get Writer Lock */
      disposable.Dispose();
      _disposables.Remove(disposable);
    }
    #endregion

    #region IDisposable Members
    public override void Dispose()
    {
      /* Get Writer Lock */
      _disposables.Reverse();
      while (_disposables.Count > 0)
      {
        _disposables[0].Dispose();
        _disposables.RemoveAt(0);
      }
    }
    #endregion
  }
}
