using Machine.Migrations.Builder;
using NUnit.Framework;
using Rhino.Mocks;

namespace Machine.Migrations.Builders
{
	[TestFixture]
	public class SchemaUtilsTests
	{
		[Test]
		public void Normalize_WontChangeValidIdentifier()
		{
			Assert.AreEqual("NAME", SchemaUtils.Normalize("Name"));
		}

		[Test]
		public void Normalize_RemoveBrackets()
		{
			Assert.AreEqual("USER", SchemaUtils.Normalize("[user]"));
		}

		[Test]
		public void Normalize_RemoveMark()
		{
			Assert.AreEqual("KEY", SchemaUtils.Normalize("`Key`"));
		}
	}
}
using Machine.Migrations.Builder;
using NUnit.Framework;
using Rhino.Mocks;

namespace Machine.Migrations.Builders
{
	[TestFixture]
	public class SchemaUtilsTests
	{
		[Test]
		public void Normalize_WontChangeValidIdentifier()
		{
			Assert.AreEqual("NAME", SchemaUtils.Normalize("Name"));
		}

		[Test]
		public void Normalize_RemoveBrackets()
		{
			Assert.AreEqual("USER", SchemaUtils.Normalize("[user]"));
		}

		[Test]
		public void Normalize_RemoveMark()
		{
			Assert.AreEqual("KEY", SchemaUtils.Normalize("`Key`"));
		}
	}
}
