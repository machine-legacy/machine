using System;
using System.Collections;
using System.Collections.Generic;

namespace Machine.Testing.FluentFixtures
{
  public class CurrentService
  {
    readonly Dictionary<Type, Stack> _currentMap = new Dictionary<Type, Stack>();

    public void Push<T>(T current)
    {
      GetStack<T>().Push(current);
    }

    public T Pop<T>()
    {
      return (T)GetStack<T>().Pop();
    }

    public T Get<T>() where T : class
    {
      if (GetStack<T>().Count == 0)
      {
        return null;
      }

      return (T)GetStack<T>().Peek();
    }

    Stack GetStack<T>()
    {
      if (!_currentMap.ContainsKey(typeof(T)))
      {
        _currentMap[typeof(T)] = new Stack();
      }

      return _currentMap[typeof(T)];
    }
  }
}