using System.Reflection;
using Castle.Core.Interceptor;
using System.Linq;

namespace Machine.Mocks
{
  public class MockInterceptor : IInterceptor
  {
    static readonly MethodInfo[] ObjectMethods = typeof(object).GetMethods();

    public void Intercept(IInvocation invocation)
    {
      if (ObjectMethods.Contains(invocation.Method))
      {
        invocation.Proceed();
      }
    }
  }
}