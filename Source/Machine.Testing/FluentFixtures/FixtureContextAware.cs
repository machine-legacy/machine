using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Testing.FluentFixtures
{
  public class FixtureContextAware
  {
    IFixtureContext _context;

    protected NewService New
    {
      get { return _context.New; }
    }

    protected CurrentService Current
    {
      get { return _context.Current; }
    }

    protected IExistingService Existing
    {
      get { return _context.Existing; }
    }

    protected IFixtureContext Context
    {
      get { return _context; }
      set { _context = value; }
    }

    protected FixtureContextAware(IFixtureContext context)
    {
      _context = context;
    }
  }

  public class StaticFixtureContextAware
  {
    static IFixtureContext _context;

    protected static NewService New
    {
      get { return _context.New; }
    }

    protected static CurrentService Current
    {
      get { return _context.Current; }
    }

    protected static IExistingService Existing
    {
      get { return _context.Existing; }
    }

    protected static IFixtureContext Context
    {
      get { return _context; }
      set { _context = value; }
    }

    protected static void SetContext(IFixtureContext context)
    {
      _context = context;
    }
  }
}