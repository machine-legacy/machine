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
  public class FluentMigrationTests
  {
    MockMigrationContext context;

    [SetUp]
    public void Setup()
    {
      var migration = new ConcreteFluentMigration(x=>
      {
        var company = x.CreateTable("Company", t=>
        {
          t.AddPrimaryKey<int>("Id").Identity();
          t.AddColumn<string>("Name", 25).Unique();
        });

        x.CreateTable("Project", t=>
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
}
