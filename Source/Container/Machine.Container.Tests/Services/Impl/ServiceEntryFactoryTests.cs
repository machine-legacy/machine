using System;
using System.Collections.Generic;

using Machine.Container.Model;

using NUnit.Framework;

namespace Machine.Container.Services.Impl
{
  [TestFixture]
  public class ServiceEntryFactoryTests : ScaffoldTests<ServiceEntryFactory>
  {
    [Test]
    public void CreateServiceEntry_Always_FillsInProperly()
    {
      ServiceEntry entry = _target.CreateServiceEntry(typeof(Service1DependsOn2), LifestyleType.Singleton);
      Assert.AreEqual(typeof(Service1DependsOn2), entry.ServiceType);
      Assert.AreEqual(LifestyleType.Singleton, entry.LifestyleType);
    }
  }
}