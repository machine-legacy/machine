namespace Machine.Migrations.Services.Impl
{
	using System;

	public class ActivatorBasedMigrationFactoryChooser : IMigrationFactoryChooser
	{
		private readonly ActivatorFactory factory = new ActivatorFactory();

		public IMigrationFactory ChooseFactory(MigrationReference migrationReference)
		{
			return factory;
		}

		public class ActivatorFactory : IMigrationFactory
		{
			public IDatabaseMigration CreateMigration(MigrationReference migrationReference)
			{
				return (IDatabaseMigration) Activator.CreateInstance(migrationReference.Reference);
			}
		}
	}
}
