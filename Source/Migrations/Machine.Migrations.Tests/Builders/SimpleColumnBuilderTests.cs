using System;
using Machine.Migrations.Builder;
using NUnit.Framework;

namespace Machine.Migrations.Builders
{
	[TestFixture]
	public class SimpleColumnBuilderTests
	{
		[Test]
		public void Constructor_SetsCorrectColumnTypeBasedOnSystemType()
		{
			Column col = new SimpleColumnBuilder("Name", typeof(string)).Build(null, null, null);
			Assert.AreEqual(ColumnType.NVarChar, col.ColumnType);
			
			col = new SimpleColumnBuilder("Age", typeof(int)).Build(null, null, null);
			Assert.AreEqual(ColumnType.Int32, col.ColumnType);

			col = new SimpleColumnBuilder("DoB", typeof(DateTime)).Build(null, null, null);
			Assert.AreEqual(ColumnType.DateTime, col.ColumnType);

			col = new SimpleColumnBuilder("Price", typeof(decimal)).Build(null, null, null);
			Assert.AreEqual(ColumnType.Money, col.ColumnType);
		}

		[Test]
		public void Constructor_SetsNameCorrectly()
		{
			Column col = new SimpleColumnBuilder("Name", typeof(string)).Build(null, null, null);
			Assert.AreEqual("Name", col.Name);

			col = new SimpleColumnBuilder("Age", typeof(int)).Build(null, null, null);
			Assert.AreEqual("Age", col.Name);
		}

		[Test]
		public void Constructor_SetsSizeCorrectly()
		{
			Column col = new SimpleColumnBuilder("Name", typeof(string), 10).Build(null, null, null);
			Assert.AreEqual(10, col.Size);

			col = new SimpleColumnBuilder("Name", typeof(string), 30).Build(null, null, null);
			Assert.AreEqual(30, col.Size);
		}
	}
}
using System;
using Machine.Migrations.Builder;
using NUnit.Framework;

namespace Machine.Migrations.Builders
{
	[TestFixture]
	public class SimpleColumnBuilderTests
	{
		[Test]
		public void Constructor_SetsCorrectColumnTypeBasedOnSystemType()
		{
			Column col = new SimpleColumnBuilder("Name", typeof(string)).Build(null, null, null);
			Assert.AreEqual(ColumnType.NVarChar, col.ColumnType);
			
			col = new SimpleColumnBuilder("Age", typeof(int)).Build(null, null, null);
			Assert.AreEqual(ColumnType.Int32, col.ColumnType);

			col = new SimpleColumnBuilder("DoB", typeof(DateTime)).Build(null, null, null);
			Assert.AreEqual(ColumnType.DateTime, col.ColumnType);

			col = new SimpleColumnBuilder("Price", typeof(decimal)).Build(null, null, null);
			Assert.AreEqual(ColumnType.Money, col.ColumnType);
		}

		[Test]
		public void Constructor_SetsNameCorrectly()
		{
			Column col = new SimpleColumnBuilder("Name", typeof(string)).Build(null, null, null);
			Assert.AreEqual("Name", col.Name);

			col = new SimpleColumnBuilder("Age", typeof(int)).Build(null, null, null);
			Assert.AreEqual("Age", col.Name);
		}

		[Test]
		public void Constructor_SetsSizeCorrectly()
		{
			Column col = new SimpleColumnBuilder("Name", typeof(string), 10).Build(null, null, null);
			Assert.AreEqual(10, col.Size);

			col = new SimpleColumnBuilder("Name", typeof(string), 30).Build(null, null, null);
			Assert.AreEqual(30, col.Size);
		}
	}
}
