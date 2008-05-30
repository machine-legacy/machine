using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Migrations.Builders
{
  public class TableMutator : ITableMutator
  {
    public IColumnBuilder AddPrimaryKey<T>(string columnName)
    {
      throw new NotImplementedException();
    }

    public IColumnBuilder AddColumn<T>(string columnName)
    {
      throw new NotImplementedException();
    }

    public IColumnBuilder AddColumn<T>(string columnName, short size)
    {
      throw new NotImplementedException();
    }

    public IColumnBuilder AddForeignKey(string columnName, TableInfo table)
    {
      throw new NotImplementedException();
    }

    public void DropColumn(string columnName)
    {
      throw new NotImplementedException();
    }

    public void RenameColumn(string oldName, string newName)
    {
      throw new NotImplementedException();
    }
  }
}
