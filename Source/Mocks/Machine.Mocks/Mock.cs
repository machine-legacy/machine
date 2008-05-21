using Machine.Mocks.Implementation.Generation;

namespace Machine.Mocks
{
  public class Mock
  {
    static readonly MockGenerator _mockGenerator = new MockGenerator();

    public static T Of<T>() where T : class 
    {
      return Of<T>(new object[] {});
    }

    public static T Of<T>(params object[] constructorArguments) where T : class
    {
      var mock = new Mock<T>(_mockGenerator, constructorArguments);

      return mock.Object;
    }
  }

  public class Mock<T>
  {
    T _object;

    public T Object
    { 
      get { return _object; }
    }

    public Mock(MockGenerator mockGenerator, object[] constructorArguments)
    {
      _object = (T)mockGenerator.GenerateMock(typeof(T), constructorArguments);
    }
  }
}