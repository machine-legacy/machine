using System;
using System.Collections.Generic;

using Machine.Core;
using Machine.Migrations.Core;
using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;
using Machine.Migrations.Services;

using NUnit.Framework;

namespace Machine.Migrations
{
  [TestFixture]
  public class SimpleMigrationTests : StandardFixture<ConcreteSimpleMigration>
  {
    public override ConcreteSimpleMigration Create()
    {
      return new ConcreteSimpleMigration();
    }

    [Test]
    public void Initialize_Always_SetsServices()
    {
      var context = new MockMigrationContext();
      _target.Initialize(context);
      Assert.AreEqual(context.DatabaseProvider, _target.Database);
      Assert.AreEqual(context.SchemaProvider, _target.Schema);
    }
  }

  public class ConcreteSimpleMigration : SimpleMigration
  {
    public override void Up()
    {
    }

    public override void Down()
    {
    }
  }
}