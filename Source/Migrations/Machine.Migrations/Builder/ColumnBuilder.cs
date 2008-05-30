namespace Machine.Migrations.Builder
{
	using System;
	using System.Collections.Generic;
	using SchemaProviders;

	public abstract class ColumnBuilder<T> : IColumnBuilder
		where T : ColumnBuilder<T>
	{
		protected string name;
		protected Type type;
		protected ColumnType? colType;
		protected short? size;
		protected bool? nullable;
		protected bool? identity;
		protected bool? unique;

		protected ColumnBuilder(string name)
		{
			this.name = name;
		}

		protected ColumnBuilder(string name, Type columnType)
		{
			this.name = name;
			this.type = columnType;
		}

		protected ColumnBuilder(string name, Type columnType, short? size)
		{
			this.name = name;
			this.size = size;
			this.type = columnType;
		}

		protected ColumnBuilder(string name, ColumnType columnType)
		{
			this.name = name;
			this.colType = columnType;
		}

		protected ColumnBuilder(string name, ColumnType columnType, short? size)
		{
			this.name = name;
			this.size = size;
			this.colType = columnType;
		}

		public string Name
		{
			get { return name; }
		}

		public ColumnType ColumnType
		{
			get { return colType.Value; }
		}

		public short? Size
		{
			get { return size; }
		}

		public T Identity()
		{
			identity = true;
			return (T) this;
		}

		public T Nullable()
		{
			nullable = true;
			return (T) this;
		}

		public T MakeUnique()
		{
			unique = true;
			return (T) this;
		}

		public virtual Column Build(TableBuilder table, ISchemaProvider schemaProvider, IList<PostProcess> posts)
		{
			Column col;

			if (colType.HasValue)
			{
				col = new Column(name, colType.Value, false);
			}
			else
			{
				col = new Column(name, type);
			}

			if (colType.HasValue)
			{
				col.ColumnType = colType.Value;
			}
			if (size.HasValue)
			{
				col.Size = size.Value;
			}
			if (nullable.HasValue)
			{
				col.AllowNull = nullable.Value;
			}
			if (identity.HasValue)
			{
				col.IsIdentity = identity.Value;
			}
			if (unique.HasValue)
			{
				col.IsUnique = unique.Value;
			}

			colType = col.ColumnType;

			return col;
		}
	}

	public delegate void ProcessAction();

	public class PostProcess
	{
		private readonly ProcessAction action;

		public PostProcess(ProcessAction action)
		{
			this.action = action;
		}

		public ProcessAction Action
		{
			get { return action; }
		}
	}
}
