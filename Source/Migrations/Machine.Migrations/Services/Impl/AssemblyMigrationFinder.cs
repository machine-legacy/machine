namespace Machine.Migrations.Services.Impl
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	public class AssemblyMigrationFinder : IMigrationFinder
	{
		private readonly Assembly assembly;

		public AssemblyMigrationFinder(Assembly assembly)
		{
			this.assembly = assembly;
		}

		public ICollection<MigrationReference> FindMigrations()
		{
			List<MigrationReference> refs = new List<MigrationReference>();

			foreach (Type type in assembly.GetTypes())
			{
				if (type.IsPublic && type.IsClass && !type.IsAbstract && typeof(IDatabaseMigration).IsAssignableFrom(type))
				{
					object[] attrs = type.GetCustomAttributes(typeof(MigrationAttribute), false);

					if (attrs.Length == 0)
					{
						throw new Exception("Found migration type that has no version information. See type " + type.FullName);
					}

					MigrationAttribute att = (MigrationAttribute)attrs[0];

					MigrationReference mref = new MigrationReference(att.Version, type.Name, "");
					mref.Reference = type;

					refs.Add(mref);
				}
			}

			refs.Sort(delegate(MigrationReference mr1, MigrationReference mr2) { return mr1.Version.CompareTo(mr2.Version); });

			return refs;
		}
	}
}
