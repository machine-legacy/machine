using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Core.ValueTypes
{
  public class Message1
  {
  }

  public class Message2 : ClassTypeAsValueType
  {
    string _name;

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public Message2(string name)
    {
      _name = name;
    }
  }

  public class Message3
  {
    string _name;
    short _size;

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public short Size
    {
      get { return _size; }
      set { _size = value; }
    }

    public Message3(string name, short size)
    {
      _name = name;
      _size = size;
    }
  }

  public class Message4
  {
    string _name;
    short _size;
    Message2 _message2;

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public short Size
    {
      get { return _size; }
      set { _size = value; }
    }

    public Message4(string name, short size, Message2 message2)
    {
      _name = name;
      _message2 = message2;
      _size = size;
    }
  }

  public enum YesNoMaybe
  {
    Yes = 0,
    No,
    Maybe
  }

  public class TypeWithABunchOfTypes
  {
    readonly string _aString;
    readonly long _aLong;
    readonly short _aShort;
    readonly bool _aBool;
    readonly int _aInt;
    readonly YesNoMaybe _aEnum;
    readonly DateTime _aDateTime;

    public TypeWithABunchOfTypes(bool aBool, int aInt, long aLong, short aShort, string aString, YesNoMaybe aEnum,
      DateTime aDateTime)
    {
      _aBool = aBool;
      _aDateTime = aDateTime;
      _aInt = aInt;
      _aLong = aLong;
      _aShort = aShort;
      _aString = aString;
      _aEnum = aEnum;
    }
  }

  public class TypeWithOnlyEnum
  {
    readonly YesNoMaybe _maybe;

    public TypeWithOnlyEnum(YesNoMaybe maybe)
    {
      _maybe = maybe;
    }
  }

  public class MessageWithArray
  {
    readonly string[] _array;

    public MessageWithArray(params string[] array)
    {
      _array = array;
    }
  }

}