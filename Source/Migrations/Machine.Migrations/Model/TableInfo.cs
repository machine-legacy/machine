using System;
using System.Collections.Generic;

namespace Machine.Migrations
{
  public class TableInfo
  {
    public string Name
    {
      get; private set;
    }

    public string PrimaryKeyName
    {
      get; private set;
    }

    public ColumnType? PrimaryKeyType
    {
      get; private set;
    }

    public short? PrimaryKeySize
    {
      get; private set;
    }

    public TableInfo(string name, string primaryKeyName, ColumnType? primaryKeyType, short? primaryKeySize)
    {
      Name = name;
      PrimaryKeyName = primaryKeyName;
      PrimaryKeyType = primaryKeyType;
      PrimaryKeySize = primaryKeySize;
    }
  }
}