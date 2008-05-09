using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace Machine.Core.Services.Impl
{
  [TestFixture]
  public class NamerTests : StandardFixture<Namer>
  {
    public override Namer Create()
    {
      return new Namer();
    }

    [Test]
    public void ToLowerCamelCase_IsCamelCase_JustMakesLowerCamelCase()
    {
      Assert.AreEqual("thisIsCamelCase", _target.ToLowerCamelCase("ThisIsCamelCase"));
    }

    [Test]
    public void ToLowerCamelCase_IsNotCamelCase_MakesLowerCamelCase()
    {
      Assert.AreEqual("thisIsCamelCase", _target.ToLowerCamelCase("This_is_Camel_case"));
    }

    [Test]
    public void ToCamelCase_IsCamelCase_DoesNothing()
    {
      Assert.AreEqual("ThisIsCamelCase", _target.ToCamelCase("ThisIsCamelCase"));
    }

    [Test]
    public void ToCamelCase_IsNotCamelCase_MakesCamelCase()
    {
      Assert.AreEqual("ThisIsCamelCase", _target.ToCamelCase("This_is_Camel_case"));
    }

    [Test]
    public void ToUnderscoreDelimited_IsMixed_MakesDelimited()
    {
      Assert.AreEqual("this_is_camel_case", _target.ToUnderscoreDelimited("This_is_CamelCase"));
    }

    [Test]
    public void ToUnderscoreDelimited_IsCamelCase_MakesDelimited()
    {
      Assert.AreEqual("this_is_camel_case", _target.ToUnderscoreDelimited(("ThisIsCamelCase")));
    }

    [Test]
    public void ToCamelCase_IsJustAWord_MakesCamelCase()
    {
      Assert.AreEqual("This", _target.ToCamelCase("this"));
    }

    [Test]
    public void MakeRandomName_Always_MakesString()
    {
      Assert.AreEqual(8, _target.MakeRandomName().Length);
    }
  }
}