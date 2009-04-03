using System;
using System.Collections.Generic;

using NUnit.Framework;
using Rhino.Mocks;

using Machine.Container.Services.Impl;

namespace Machine.Container.Model
{
  [TestFixture]
  public class ServiceEntryTests : ScaffoldTests<ServiceEntry>
  {
    [Test]
    public void ToString_Always_ReturnsAString()
    {
      Run();
      Assert.IsNotNull(_target.ToString());
    }

    protected override ServiceEntry Create()
    {
      return new ServiceEntry(typeof(SimpleService1), LifestyleType.Singleton);
    }
  }
}