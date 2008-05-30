using System;
using System.Collections.Generic;
using System.Threading;

namespace Machine.Core.ValueTypes
{
  public class ValueTypeHelper
  {
    static readonly ObjectEqualityFunctionFactory _objectEqualityFunctionFactory = new ObjectEqualityFunctionFactory();

    static readonly CalculateHashCodeFunctionFactory _calculateHashCodeFunctionFactory =
      new CalculateHashCodeFunctionFactory();

    static readonly ToStringFunctionFactory _toStringFunctionFactory = new ToStringFunctionFactory();
    static readonly Dictionary<Type, CacheEntry> _cache = new Dictionary<Type, CacheEntry>();
    static readonly ReaderWriterLock _lock = new ReaderWriterLock();

    class CacheEntry
    {
      public ObjectEqualityFunction AreEqual;
      public CalculateHashCodeFunction CalculateHashCode;
      public ToStringFunction MakeString;
    }

    /*
    public static bool AreEqual<TType>(TType a, TType b)
    {
      return Lookup(typeof(TType)).AreEqual(a, b);
    }

    public static bool AreEqual<TType>(object a, object b)
    {
      if (a is TType && b is TType)
      {
        return Lookup(typeof(TType)).AreEqual(a, b);
      }
      return false;
    }
    */

    public static bool AreEqual(object a, object b)
    {
      if (a == null) throw new ArgumentNullException("a");
      if (b == null) throw new ArgumentNullException("b");
      if (a.GetType().Equals(b.GetType()))
      {
        return Lookup(a.GetType()).AreEqual(a, b);
      }
      return false;
    }

    /*
    public static Int32 CalculateHashCode<TType>(TType value)
    {
      return Lookup(typeof(TType)).CalculateHashCode(value);
    }
    */

    public static Int32 CalculateHashCode(object value)
    {
      if (value == null) throw new ArgumentNullException("value");
      return Lookup(value.GetType()).CalculateHashCode(value);
    }

    /*
    public static string ToString<TType>(TType a)
    {
      return Lookup(typeof(TType)).MakeString(a);
    }
    */

    public static string ToString(object value)
    {
      if (value == null) throw new ArgumentNullException("value");
      return Lookup(value.GetType()).MakeString(value);
    }

    static CacheEntry Lookup(Type type)
    {
      CacheEntry entry;
      _lock.AcquireReaderLock(Timeout.Infinite);
      if (_cache.ContainsKey(type))
      {
        entry = _cache[type];
      }
      else
      {
        _lock.UpgradeToWriterLock(Timeout.Infinite);
        entry = _cache[type] = new CacheEntry();
        _cache[type].AreEqual = _objectEqualityFunctionFactory.CreateObjectEqualityFunction(type);
        _cache[type].CalculateHashCode = _calculateHashCodeFunctionFactory.CreateCalculateHashCodeFunction(type);
        _cache[type].MakeString = _toStringFunctionFactory.CreateToStringFunction(type);
      }
      _lock.ReleaseLock();
      return entry;
    }
  }
}