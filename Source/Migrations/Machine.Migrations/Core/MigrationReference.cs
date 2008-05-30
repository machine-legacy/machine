using System;

namespace Machine.Migrations
{
  public class MigrationReference
  {
    short _version;
    string _name;
    string _path;
    Type _ref;

    public short Version
    {
      get { return _version; }
      set { _version = value; }
    }

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public string Path
    {
      get { return _path; }
      set { _path = value; }
    }

    public Type Reference
    {
      get { return _ref; }
      set { _ref = value; }
    }

    public MigrationReference()
    {
    }

    public MigrationReference(short version, string name, string path)
    {
      _version = version;
      _name = name;
      _path = path;
    }

    public override string ToString()
    {
      return String.Format("Migration<{0}, {1}>", _version, _name);
    }
  }
}