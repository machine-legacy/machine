using System.Reflection;
using Castle.Core.Interceptor;

namespace Machine.Mocks
{
  public class MockInterceptor : IInterceptor
  {
    static readonly MethodInfo[] ObjectMethods = typeof(object).GetMethods();

    public void Intercept(IInvocation invocation)
    {
      //if 
        invocation.Proceed();
    }
  }
}