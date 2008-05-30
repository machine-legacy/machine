using System;

namespace Machine.Migrations
{
  using System.Data.SqlTypes;

  public enum ColumnType
  {
    Undefined,
    Int16,
    Int32,
    Long,
    Binary,
    Char,
    NVarChar,
    Text,
    DateTime,
    Bool,
    Real,
    Money,
    Decimal,
    Image
  }

  public class Column
  {
    private string _name;
    private short _size;
    private ColumnType _columnType;
    private bool _isPrimaryKey;
    private bool _allowNull;
    private bool _isIdentity;
    private bool _isUnique;

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public ColumnType ColumnType
    {
      get { return _columnType; }
      set { _columnType = value; }
    }

    public short Size
    {
      get { return _size; }
      set { _size = value; }
    }

    public bool IsPrimaryKey
    {
      get { return _isPrimaryKey; }
      set { _isPrimaryKey = value; }
    }

    public bool IsIdentity
    {
      get { return _isIdentity; }
      set { _isIdentity = value; }
    }

    public bool IsUnique
    {
      get { return _isUnique; }
      set { _isUnique = value; }
    }

    public bool AllowNull
    {
      get { return _allowNull; }
      set { _allowNull = value; }
    }

    public Column()
    {
    }

    public Column(string name, Type type)
      : this(name, type, false)
    {
    }

    public Column(string name, Type type, bool allowNull)
      : this(name, type, 0, false, allowNull)
    {
      if (type == typeof(Int16))
      {
        _size = 2;
      }
      else if (type == typeof(Int32))
      {
        _size = 4;
      }
      else if (type == typeof(Int64))
      {
        _size = 8;
      }
    }

    public Column(string name, ColumnType type, bool allowNull)
      : this(name, type, 0, false, allowNull)
    {
      if (type == ColumnType.Int16)
      {
        _size = 2;
      }
      else if (type == ColumnType.Int32)
      {
        _size = 4;
      }
      else if (type == ColumnType.Long)
      {
        _size = 8;
      }
    }

    public Column(string name, Type type, short size)
      : this(name, type, size, false)
    {
    }

    public Column(string name, Type type, short size, bool isPrimaryKey)
      : this(name, type, size, isPrimaryKey, false)
    {
    }

    public Column(string name, Type type, short size, bool isPrimaryKey, bool allowNull)
      : this(name, ToColumnType(type), size, isPrimaryKey, allowNull)
    {
    }

    public Column(string name, ColumnType type, short size, bool isPrimaryKey, bool allowNull)
    {
      _name = name;
      _size = size;
      _isPrimaryKey = isPrimaryKey;
      if (_isPrimaryKey)
        _isIdentity = true;
      _allowNull = allowNull;
      _columnType = type;
    }

    private static ColumnType ToColumnType(Type type)
    {
      if (type == typeof(Int16))
      {
        return ColumnType.Int16;
      }
      if (type == typeof(Int32))
      {
        return ColumnType.Int32;
      }
      if (type == typeof(Int64))
      {
        return ColumnType.Long;
      }
      if (type == typeof(byte[]))
      {
        return ColumnType.Binary;
      }
      if (type == typeof(String))
      {
        return ColumnType.NVarChar;
      }
      if (type == typeof(DateTime))
      {
        return ColumnType.DateTime;
      }
      if (type == typeof(bool))
      {
        return ColumnType.Bool;
      }
      if (type == typeof(float) || type == typeof(double))
      {
        return ColumnType.Real;
      }
      if (type == typeof(decimal))
      {
        return ColumnType.Money;
      }
      if (type == typeof(char) || type == typeof(byte))
      {
        return ColumnType.Char;
      }
      if (type == typeof(SqlMoney))
      {
        return ColumnType.Money;
      }
      throw new ArgumentException("Type not supported " + type.FullName, "type");
    }
  }
}
