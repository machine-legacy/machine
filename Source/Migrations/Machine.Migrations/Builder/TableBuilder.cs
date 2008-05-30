namespace Machine.Migrations.Builder
{
  using System;
  using System.Collections.Generic;

  using SchemaProviders;

  public class TableBuilder
  {
    readonly string name;
    readonly IColumnBuilder[] columns;
    IColumnBuilder pkColumn;

    public TableBuilder(string name, params IColumnBuilder[] columns)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentNullException("name");
      if (columns == null || columns.Length == 0)
        throw new ArgumentException("Please specify at least one column", "columns");

      this.name = name;
      this.columns = columns;
    }

    public string Name
    {
      get { return name; }
    }

    public IColumnBuilder[] Columns
    {
      get { return columns; }
    }

    public IColumnBuilder PrimaryKeyColumn
    {
      get { return pkColumn; }
    }

    public TableBuilder Build(ISchemaProvider schemaProvider)
    {
      List<Column> cols = new List<Column>();
      List<PostProcess> post = new List<PostProcess>();

      foreach (IColumnBuilder columnBuilder in columns)
      {
        Column col = columnBuilder.Build(this, schemaProvider, post);

        if (col.IsPrimaryKey)
        {
          pkColumn = columnBuilder;
        }

        cols.Add(col);
      }

      schemaProvider.AddTable(name, cols.ToArray());

      foreach (PostProcess process in post)
      {
        process.Action();
      }

      return this;
    }
  }
}