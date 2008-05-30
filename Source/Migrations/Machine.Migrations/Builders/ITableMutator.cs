using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Migrations.Builders
{
  public interface ITableMutator : ITableBuilder
  {
    void DropColumn(string columnName);
    void RenameColumn(string oldName, string newName);
  }
}
