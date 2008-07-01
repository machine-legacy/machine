using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Plugins;

using NUnit.Framework;

namespace Machine.Container.Services.Impl
{
  [TestFixture]
  public class ThrowsPendingActivatorResolverTests : ScaffoldTests<ThrowsPendingActivatorResolver>
  {
    [Test]
    [ExpectedException(typeof(PendingDependencyException))]
    public void ResolveActivator_Always_Throws()
    {
      ServiceEntry serviceEntry = ServiceEntryHelper.NewEntry();
      IResolutionServices services = Create<ResolutionServices>();
      _target.ResolveActivator(services, serviceEntry);
    }
  }
}