using System.Collections.Generic;

using Machine.Migrations.Core;

namespace Machine.Migrations.Services
{
  public interface IVersionStateFactory
  {
    VersionState CreateVersionState(ICollection<MigrationReference> migrations);
  }
}