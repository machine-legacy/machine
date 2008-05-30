using System;
using System.Collections.Generic;

using Machine.Migrations.SchemaProviders;

namespace Machine.Migrations.Builders
{
  public class TableBuilder : ITableBuilder
  {
    readonly string _name;
    readonly List<IColumnBuilder> _columns = new List<IColumnBuilder>();
    ColumnBuilder _pkColumn;

    public string Name
    {
      get { return _name; }
    }

    protected internal TableBuilder(string name)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentNullException("name");

      _name = name;
    }

    protected internal TableInfo Build(ISchemaProvider schemaProvider)
    {
      List<Column> cols = new List<Column>();
      List<PostProcess> post = new List<PostProcess>();

      foreach (ColumnBuilder columnBuilder in _columns)
      {
        Column col = columnBuilder.Build(this, schemaProvider, post);

        if (col.IsPrimaryKey)
        {
          _pkColumn = columnBuilder;
        }

        cols.Add(col);
      }

      schemaProvider.AddTable(_name, cols.ToArray());

      foreach (PostProcess process in post)
      {
        process.Action();
      }

      return new TableInfo(_name, _pkColumn.Name, _pkColumn.ColumnType, _pkColumn.Size);
    }

    public IColumnBuilder AddPrimaryKey<T>(string columnName)
    {
      if (_pkColumn != null) throw new InvalidOperationException("You can only add one primary key");

      var builder = new PrimaryKeyBuilder(columnName, typeof(T));
      _columns.Add(builder);
      _pkColumn = builder;

      return builder;
    }

    public IColumnBuilder AddColumn<T>(string columnName)
    {
      var builder = new SimpleColumnBuilder(columnName, typeof(T));
      _columns.Add(builder);

      return builder;
    }

    public IColumnBuilder AddColumn<T>(string columnName, short size)
    {
      var builder = new SimpleColumnBuilder(columnName, typeof(T), size);
      _columns.Add(builder);

      return builder;
    }

    public IColumnBuilder AddForeignKey(string columnName, TableInfo table)
    {
      var builder = new ForeignKeyBuilder(columnName, table);
      _columns.Add(builder);

      return builder;
    }
  }
}