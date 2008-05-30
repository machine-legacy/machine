using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using Rhino.Mocks;

namespace Machine.Migrations
{
  public delegate void DoUp(FluentMigration migration);
  public class ConcreteFluentMigration : FluentMigration
  {
    readonly DoUp _doUp;

    public ConcreteFluentMigration(DoUp doUp)
    {
      _doUp = doUp;
    }

    public override void Up()
    {
      _doUp(this);
    }

    public override void Down()
    {
    }
  }

  [TestFixture]
  public class WhenCreatingTwoTables
  {
    MockMigrationContext context;

    [SetUp]
    public void Setup()
    {
      var migration = new ConcreteFluentMigration(x=>
      {
        var company = x.Schema.CreateTable("Company", t=>
        {
          t.AddPrimaryKey<int>("Id").Identity();
          t.AddColumn<string>("Name", 25).Unique();
        });

        x.Schema.CreateTable("Project", t=>
        {
          t.AddPrimaryKey<int>("Id").Identity();
          t.AddColumn<string>("Name", 25);
          t.AddForeignKey("CompanyID", company);
        });
      });

      context = new MockMigrationContext();
      migration.Initialize(context);

      migration.Up();
    }

    [Test]
    public void ItShouldCreateTheCompanyTableWithTwoColumns()
    {
      context.SchemaProvider.AssertWasCalled(x => x.AddTable(
        Arg<string>.Matches(y=>y.Equals("Company")), 
        Arg<ICollection<Column>>.Matches(z => z.Count == 2)));
    }

    [Test]
    public void ItShouldCreateTheProjectTableWithThreeColumns()
    {
      context.SchemaProvider.AssertWasCalled(x => x.AddTable(
        Arg<string>.Matches(y=>y.Equals("Project")), 
        Arg<ICollection<Column>>.Matches(z => z.Count == 3)));
    }
  }

  [TestFixture]
  public class WhenAlteringATable
  {
    MockMigrationContext context;

    [SetUp]
    public void Setup()
    {
      var migration = new ConcreteFluentMigration(x=>
      {
        x.Schema.AlterTable("Company", t=>
        {
          t.AddColumn<string>("Name", 25);
          t.DropColumn("Foobar");
          t.RenameColumn("Blah", "Yadda");
        });
      });

      context = new MockMigrationContext();
      migration.Initialize(context);

      migration.Up();
    }

    [Test]
    public void ItShouldAddNameColumn()
    {
      context.SchemaProvider.AssertWasCalled(x => x.AddColumn(
        Arg<string>.Matches(y=>y.Equals("Company")), 
        Arg<string>.Matches(y=>y.Equals("Name")), 
        Arg<Type>.Matches(y=>y.Equals(typeof(string))), 
        Arg<short>.Matches(y=>y.Equals(25)), 
        Arg<bool>.Matches(y=>y.Equals(false)), 
        Arg<bool>.Matches(y=>y.Equals(true))
        ));
    }

    [Test]
    public void ItShouldDropFoobarColumn()
    {
      context.SchemaProvider.AssertWasCalled(x => x.RemoveColumn(
        Arg<string>.Matches(y=>y.Equals("Company")), 
        Arg<string>.Matches(y=>y.Equals("Name"))
        ));
    }

    [Test]
    public void ItShouldRenameBlahColumn()
    {
      context.SchemaProvider.AssertWasCalled(x => x.RenameColumn(
        Arg<string>.Matches(y=>y.Equals("Company")), 
        Arg<string>.Matches(y=>y.Equals("Blah")),
        Arg<string>.Matches(y=>y.Equals("Yadda"))
        ));
    }
  }
}
