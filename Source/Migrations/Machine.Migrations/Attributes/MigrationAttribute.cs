namespace Machine.Migrations
{
	using System;

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class MigrationAttribute : Attribute
	{
		private short version;

		public MigrationAttribute(short version)
		{
			this.version = version;
		}

		public short Version
		{
			get { return version; }
			set { version = value; }
		}
	}
}
