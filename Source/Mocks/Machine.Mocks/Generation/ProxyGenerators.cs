using System;
using System.Collections.Generic;
using Castle.DynamicProxy;

namespace Machine.Mocks.Generation
{
  public class ProxyGenerators
  {
    Dictionary<Type, ProxyGenerator> _generatorMap;


    public ProxyGenerators()
    {
      _generatorMap = new Dictionary<Type, ProxyGenerator>();
    }

    public ProxyGenerator GetGenerator(Type type)
    {
      if (!_generatorMap.ContainsKey(type))
      {
        _generatorMap[type] = new ProxyGenerator();
      }

      return _generatorMap[type];
    }
  }
}