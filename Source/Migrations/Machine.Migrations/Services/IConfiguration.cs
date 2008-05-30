using System;

namespace Machine.Migrations.Services
{
  public interface IConfiguration
  {
  	string Scope
  	{
  		get;
  	}

    Type ConnectionProviderType
    {
      get;
    }

    Type TransactionProviderType
    {
      get;
    }

    Type SchemaProviderType
    {
      get;
    }

    Type DatabaseProviderType
    {
      get;
    }

    string ConnectionString
    {
      get;
    }

    string MigrationsDirectory
    {
      get;
    }

    short DesiredVersion
    {
      get;
    }

    bool ShowDiagnostics
    {
      get;
    }

    string[] References
    {
      get;
    }

    int CommandTimeout
    { 
      get;
    }

    void SetCommandTimeout(int commandTimeout);
  }
}
