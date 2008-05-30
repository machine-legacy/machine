namespace Machine.Migrations.Builder
{
  using System;
  using System.Collections.Generic;

  using SchemaProviders;

  public class ForeignKeyBuilder : ColumnBuilder<ForeignKeyBuilder>
  {
    readonly string targetTable;
    readonly string targetColName;

    public ForeignKeyBuilder(string name, TableBuilder referencedTable) : base(name)
    {
      IColumnBuilder referencedPK = referencedTable.PrimaryKeyColumn;

      targetTable = referencedTable.Name;
      targetColName = referencedTable.PrimaryKeyColumn.Name;

      base.colType = referencedPK.ColumnType;
      base.size = referencedPK.Size;
    }

    public ForeignKeyBuilder(string name, Type type, string targetTable, string targetColName) : base(name)
    {
      this.targetTable = targetTable;
      this.targetColName = targetColName;
      base.type = type;
    }

    public override Column Build(TableBuilder table, ISchemaProvider schemaProvider, IList<PostProcess> posts)
    {
      Column col = base.Build(table, schemaProvider, posts);

      posts.Add(new PostProcess(
        delegate()
        {
          string fkName = "FK_" +
            SchemaUtils.Normalize(table.Name) + "_" +
              SchemaUtils.Normalize(col.Name) + "_" +
                SchemaUtils.Normalize(targetColName);

          schemaProvider.AddForeignKeyConstraint(
            table.Name, fkName, col.Name,
            targetTable, targetColName);
        }));

      return col;
    }
  }
}