using System;
using System.Collections.Generic;

namespace Machine.Container.Model
{
  public class ResolvedConstructorCandidate
  {
    private readonly ConstructorCandidate _candidate;
    private readonly IList<ResolvedServiceEntry> _resolvedDependencies;

    public ConstructorCandidate Candidate
    {
      get { return _candidate; }
    }

    public IList<ResolvedServiceEntry> ResolvedDependencies
    {
      get { return _resolvedDependencies; }
    }

    public ResolvedConstructorCandidate(ConstructorCandidate candidate, IList<ResolvedServiceEntry> resolvedDependencies)
    {
      _candidate = candidate;
      _resolvedDependencies = resolvedDependencies;
    }
  }
}