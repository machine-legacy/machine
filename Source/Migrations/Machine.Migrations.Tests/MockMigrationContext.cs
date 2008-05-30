using System;
using System.Collections.Generic;
using System.Text;

using Machine.Migrations.Core;
using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;
using Machine.Migrations.Services;

using Rhino.Mocks;

namespace Machine.Migrations
{
  public class MockMigrationContext : MigrationContext
  {
    public MockMigrationContext() : base(
      MockRepository.GenerateStub<IConfiguration>(),
      MockRepository.GenerateStub<IDatabaseProvider>(),
      MockRepository.GenerateStub<ISchemaProvider>(),
      MockRepository.GenerateStub<ICommonTransformations>(),
      MockRepository.GenerateStub<IConnectionProvider>())
    {
    }
  }
}