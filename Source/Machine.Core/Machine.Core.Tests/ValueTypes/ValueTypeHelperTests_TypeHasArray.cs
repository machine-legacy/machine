using NUnit.Framework;

namespace Machine.Core.ValueTypes
{
  [TestFixture]
  public class ValueTypeHelperTests_TypeHasArray
  {
    [Test]
    public void AreEqual_WithDifferentValues_IsFalse()
    {
      Assert.IsFalse(ValueTypeHelper.AreEqual(new MessageWithArray("1", "B"), new MessageWithArray("1", "A")));
    }

    [Test]
    public void AreEqual_WithEqualValues_IsTrue()
    {
      Assert.IsTrue(ValueTypeHelper.AreEqual(new MessageWithArray("1", "A"), new MessageWithArray("1", "A")));
    }

    [Test]
    public void AreEqual_WithOneNull_IsFalse()
    {
      Assert.IsFalse(ValueTypeHelper.AreEqual(new MessageWithArray(null), new MessageWithArray("1", "A")));
    }

    [Test]
    public void AreEqual_WithBothNull_IsTrue()
    {
      Assert.IsTrue(ValueTypeHelper.AreEqual(new MessageWithArray(null), new MessageWithArray(null)));
    }

    [Test]
    public void AreEqual_IsSameInstance_IsTrue()
    {
      MessageWithArray value = new MessageWithArray("1", "A");
      Assert.IsTrue(ValueTypeHelper.AreEqual(value, value));
    }

    [Test]
    public void GetHashCode_WithEqualValues_AreEqual()
    {
      Assert.AreEqual(ValueTypeHelper.CalculateHashCode(new MessageWithArray("1", "A")),
        ValueTypeHelper.CalculateHashCode(new MessageWithArray("1", "A")));
    }

    [Test]
    public void GetHashCode_WithDifferentValues_AreNotEqual()
    {
      Assert.AreNotEqual(ValueTypeHelper.CalculateHashCode(new MessageWithArray("1", "B")),
        ValueTypeHelper.CalculateHashCode(new MessageWithArray("1", "A")));
    }

    [Test]
    public void GetHashCode_BothAreNull_AreEqual()
    {
      Assert.AreEqual(ValueTypeHelper.CalculateHashCode(new MessageWithArray(null)),
        ValueTypeHelper.CalculateHashCode(new MessageWithArray(null)));
    }

    [Test]
    public void GetHashCode_OneIsNull_AreNotEqual()
    {
      Assert.AreNotEqual(ValueTypeHelper.CalculateHashCode(new MessageWithArray("A")),
        ValueTypeHelper.CalculateHashCode(new MessageWithArray(null)));
    }
  }
}