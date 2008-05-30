using System;
using System.Collections.Generic;

namespace Machine.Testing.FluentFixtures
{
  public class BaseCreator<T> : FixtureContextAware where T : class, new()
  {
    delegate void PopAction();

    T _creation;
    IList<PopAction> _childrenToPop = new List<PopAction>();

    public T Creation
    {
      get { return _creation; }
      private set
      {
        if (Current.Get<T>() != value)
        {
          Current.Push(value);
        }
        _creation = value;
      }
    }

    public BaseCreator(IFixtureContext context) : base(context)
    {
      Creation = new T();
    }

    public BaseCreator(IFixtureContext context, T creation) : base(context)
    {
      Creation = creation;
    }

    public static implicit operator T(BaseCreator<T> creator)
    {
      if (creator._creation == null)
        throw new Exception(String.Format("Creation of {0} is null, it probably shouldn't be.", typeof(T)));
      creator.Current.Pop<T>();
      creator.PopChildren();

      return creator._creation;
    }

    void PopChildren()
    {
      foreach (PopAction block in _childrenToPop)
      {
        block();
      }
    }

    protected void PushChild<TChild>(TChild child)
    {
      Current.Push(child);

      _childrenToPop.Add(()=>Current.Pop<TChild>());
    }
  }
}