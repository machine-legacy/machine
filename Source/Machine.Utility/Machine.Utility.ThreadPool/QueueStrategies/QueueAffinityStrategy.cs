using System;
using System.Collections.Generic;
using System.Threading;

using Machine.Core.Services;
using Machine.Core.Utility;

namespace Machine.Utility.ThreadPool.QueueStrategies
{
  public interface IHasQueuingAffinity<TKey>
  {
    TKey QueueAffinityKey
    {
      get;
    }
  }
  public class QueueAffinityStrategy<TType, TKey> : QueuePerWorkerStrategy where TType: IHasQueuingAffinity<TKey>
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(QueuePerWorkerStrategy));
    private readonly Dictionary<TKey, QueueOfRunnables> _keyToQueue = new Dictionary<TKey, QueueOfRunnables>();
    private readonly Dictionary<QueueOfRunnables, List<TKey>> _queueToKeys = new Dictionary<QueueOfRunnables, List<TKey>>();

    protected override void RetireQueueUnderLock(QueueOfRunnables queue)
    {
      _log.Info("Clearing Queue State");
      if (!_queueToKeys.ContainsKey(queue))
      {
        return;
      }
      foreach (TKey key in _queueToKeys[queue])
      {
        System.Diagnostics.Debug.Assert(_keyToQueue[key] == queue);
        _keyToQueue.Remove(key);
      }
      _queueToKeys.Remove(queue);
    }

    protected override QueueOfRunnables SelectQueue(List<QueueOfRunnables> queues, IRunnable runnable, ReaderWriterLock lok)
    {
      ConsumingRunnable<TType> consumerRunnable = (ConsumingRunnable<TType>)runnable;
      TKey key = consumerRunnable.Value.QueueAffinityKey;
      if (RWLock.UpgradeToWriterIf(lok, delegate() { return !_keyToQueue.ContainsKey(key); }))
      {
        QueueOfRunnables queue = base.SelectQueue(queues, runnable, lok);
        ResetQueueStateIfPossible(queue);
        _keyToQueue[key] = queue;
        if (!_queueToKeys.ContainsKey(queue))
        {
          _queueToKeys[queue] = new List<TKey>();
        }
        _queueToKeys[queue].Add(key);
      }
      return _keyToQueue[key];
    }

    protected virtual void ResetQueueStateIfPossible(QueueOfRunnables queue)
    {
      if (queue.IsEmpty)
      {
        RetireQueueUnderLock(queue);
      }
    }
  }
}
