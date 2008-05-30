using System;

using Machine.Migrations.DatabaseProviders;

namespace Machine.Migrations.SchemaProviders
{
  public class SqlServerCeSchemaProvider : SqlServerSchemaProvider
  {
    #region SqlServerCeSchemaProvider()
    public SqlServerCeSchemaProvider(IDatabaseProvider databaseProvider)
     : base(databaseProvider)
    {
    }
    #endregion

    #region ISchemaProvider Members
    public override string ColumnToConstraintsSql(string tableName, Column column)
    {
      if (column.IsPrimaryKey)
      {
        return String.Format("CONSTRAINT PK_{0} PRIMARY KEY (\"{1}\")", tableName, column.Name);
      }
      return null;
    }

	public override string ToMsSqlType(ColumnType type, int size)
    {
      if (type == ColumnType.Binary)
      {
        return "IMAGE";
      }
	  
	  if (type == ColumnType.Text || type == ColumnType.NVarChar)
      {
        if (size == 0)
        {
          return "NTEXT";
        }
        return String.Format("NVARCHAR({0})", size);
      }

	  return base.ToMsSqlType(type, size);
    }
    #endregion
  }
}
