using System;
using System.Collections.Generic;

namespace Machine.Container.Services.Impl
{
  public class Chain<TItem> : IChain<TItem> where TItem : class
  {
    private readonly List<TItem> _items = new List<TItem>();

    public IEnumerable<TItem> ChainItems
    {
      get { return _items; }
    }

    public void AddFirst(TItem item)
    {
      _items.Insert(0, item);
    }

    public void AddAfter(Type type, TItem item)
    {
      _items.Insert(_items.IndexOf(FindByType(type)) + 1, item);
    }

    public void AddBefore(Type type, TItem item)
    {
      _items.Insert(_items.IndexOf(FindByType(type)), item);
    }

    public void AddLast(TItem item)
    {
      _items.Add(item);
    }

    public void Replace(Type type, TItem item)
    {
      int index = _items.IndexOf(FindByType(type));
      _items.Insert(index + 1, item);
      _items.RemoveAt(index);
    }

    public TItem FindChainedItemByType(Type type)
    {
      foreach (TItem resolver in _items)
      {
        if (type.IsInstanceOfType(resolver))
        {
          return resolver;
        }
      }
      return null;
    }

    private TItem FindByType(Type type)
    {
      TItem item = FindChainedItemByType(type);
      if (item != null)
      {
        return item;
      }
      throw new ServiceContainerException("Unable to find item of type: " + type.FullName);
    }
  }
}