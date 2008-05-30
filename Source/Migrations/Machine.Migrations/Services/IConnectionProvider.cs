using System.Data;

namespace Machine.Migrations.Services
{
  public interface IConnectionProvider
  {
    IDbConnection OpenConnection();
    IDbConnection CurrentConnection { get; }
  }
}