using System;

namespace Machine.Container.Services
{
  public interface IChain<TItem> where TItem : class
  {
    void AddFirst(TItem item);
    void AddAfter(Type type, TItem item);
    void AddBefore(Type type, TItem item);
    void AddLast(TItem item);
    void Replace(Type type, TItem item);
    TItem FindChainedItemByType(Type type);
  }
}