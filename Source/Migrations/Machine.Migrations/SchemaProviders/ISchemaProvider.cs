using System;
using System.Collections.Generic;

namespace Machine.Migrations.SchemaProviders
{
  public interface ISchemaProvider
  {
    void AddTable(string table, ICollection<Column> columns);
	void DropTable(string table);
	void RenameTable(string table, string newName);
	void AddColumn(string table, string column, Type type);
    void AddColumn(string table, string column, Type type, bool allowNull);
    void AddColumn(string table, string column, Type type, short size, bool isPrimaryKey, bool allowNull);
    void AddColumn(string table, string column, Type type, short size, bool allowNull);
    void RemoveColumn(string table, string column);
    void RenameColumn(string table, string column, string newName);
	void AddSchema(string name);
	void RemoveSchema(string name);
	bool HasTable(string table);
	bool HasColumn(string table, string column);
	bool HasSchema(string name);
	void ChangeColumn(string table, string column, Type type, short size, bool allowNull);
    string[] Columns(string table);
    string[] Tables();
	void AddForeignKeyConstraint(string table, string name, string column, string foreignTable, string foreignColumn);
	void AddUniqueConstraint(string table, string name, params string[] columns);
	void DropConstraint(string table, string name);
  }
}
