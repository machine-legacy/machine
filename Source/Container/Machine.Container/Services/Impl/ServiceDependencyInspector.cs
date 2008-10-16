using System;
using System.Collections.Generic;
using System.Reflection;

using Machine.Container.Model;

namespace Machine.Container.Services.Impl
{
  public class ServiceDependencyInspector : IServiceDependencyInspector
  {
    #region IServiceDependencyInspector
    public ConstructorCandidate SelectConstructor(Type type)
    {
      if (type.IsAbstract)
      {
        throw new InvalidOperationException("Can't resolve dependencies on Abstract type: " + type);
      }
      List<ConstructorCandidate> candidates = DetermineCandidates(type);
      ConstructorCandidate selected = null;
      foreach (ConstructorCandidate candidate in candidates)
      {
        if (selected == null)
        {
          selected = candidate;
        }
        else
        {
          BailOutIfEqualNumberOfArguments(type, selected, candidate);
          selected = ChooseOneWithMoreArguments(selected, candidate);
        }
      }
      if (selected == null)
      {
        throw new InvalidOperationException("No constructor candidate found: " + type);
      }
      return selected;
    }
    #endregion

    private static List<ConstructorCandidate> DetermineCandidates(Type type)
    {
      List<ConstructorCandidate> candidates = new List<ConstructorCandidate>();
      foreach (ConstructorInfo ctor in type.GetConstructors())
      {
        ConstructorCandidate candidate = new ConstructorCandidate(ctor);
        foreach (ParameterInfo parameter in ctor.GetParameters())
        {
          candidate.AddParameterDependency(new ServiceDependency(parameter.ParameterType, DependencyType.Constructor, parameter.Name));
        }
        candidates.Add(candidate);
      }
      return candidates;
    }

    private static void BailOutIfEqualNumberOfArguments(Type type, ConstructorCandidate first, ConstructorCandidate second)
    {
      if (first.Dependencies.Count == second.Dependencies.Count)
      {
        throw new InvalidOperationException("Two constructors with equal number of arguments on " + type);
      }
    }

    private static ConstructorCandidate ChooseOneWithMoreArguments(ConstructorCandidate first, ConstructorCandidate second)
    {
      return first.Dependencies.Count > second.Dependencies.Count ? first : second;
    }
  }
}