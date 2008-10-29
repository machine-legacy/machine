using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Machine.Specifications;
using Moq;
using It=Machine.Specifications.It;
using Arg=Moq.It;

namespace Machine.MsMvc.Specs
{
  [Subject("Async Controller")]
  public class when_invoking_an_action_on_an_asynchronous_controller_that_finishes_syncrhonously
    : with_async_infrastructure
  {
    Establish context = () =>
    {
      PrepareRequestTo("Test", "Synchronous");
      TestAttribute.Reset();
    };

    Because of = () =>
      asyncResult = httpHandler.BeginProcessRequest(httpContext, Callback, null);

    It should_execute_before_action_filter = () =>
      TestAttribute.OnActionExecutingCalled.ShouldBeTrue();

    It should_execute_after_action_filter = () =>
      TestAttribute.OnActionExecutingCalled.ShouldBeTrue();

    It should_execute_before_result_filter = () =>
      TestAttribute.OnResultExecutingCalled.ShouldBeTrue();

    It should_execute_after_result_filter = () =>
      TestAttribute.OnResultExecutedCalled.ShouldBeTrue();

    It should_complete_synchronously = () =>
      asyncResult.CompletedSynchronously.ShouldBeTrue();

    static void Callback(IAsyncResult ar)
    {
      throw new Exception("This should not get called");
    }
  }

  [Subject("Async Controller")]
  public class when_invoking_an_action_on_an_asynchronous_controller_that_has_not_yet_finished
    : with_async_infrastructure
  {
    Establish context = () =>
    {
      PrepareRequestTo("Test", "AsyncNeverFinish");
      TestAttribute.Reset();
    };

    Because of = () =>
      asyncResult = httpHandler.BeginProcessRequest(httpContext, Callback, null);

    It should_execute_before_action_filter = () =>
      TestAttribute.OnActionExecutingCalled.ShouldBeTrue();

    It should_not_execute_after_action_filter = () =>
      TestAttribute.OnActionExecutedCalled.ShouldBeFalse();

    It should_not_execute_before_result_filter = () =>
      TestAttribute.OnResultExecutingCalled.ShouldBeFalse();

    It should_not_execute_after_result_filter = () =>
      TestAttribute.OnResultExecutedCalled.ShouldBeFalse();

    It should_not_complete_synchronously = () =>
      asyncResult.CompletedSynchronously.ShouldBeFalse();

    static void Callback(IAsyncResult ar)
    {
      throw new Exception("This should not get called");
    }
  }

  [Subject("Async Controller")]
  public class when_invoking_an_action_on_an_asynchronous_controller_that_finishes
    : with_async_infrastructure
  {
    static ManualResetEvent signal;

    Establish context = () =>
    {
      PrepareRequestTo("Test", "AsyncFinish");
      TestAttribute.Reset();
      TestResult.Executed = false;
      signal = new ManualResetEvent(false);
    };

    Because of = () =>
    {
      asyncResult = httpHandler.BeginProcessRequest(httpContext, Callback, null);
      signal.WaitOne(2000);
    };

    It should_execute_before_action_filter = () =>
      TestAttribute.OnActionExecutingCalled.ShouldBeTrue();

    It should_execute_after_action_filter = () =>
      TestAttribute.OnActionExecutingCalled.ShouldBeTrue();

    It should_execute_before_result_filter = () =>
      TestAttribute.OnResultExecutingCalled.ShouldBeTrue();

    It should_execute_after_result_filter = () =>
      TestAttribute.OnResultExecutedCalled.ShouldBeTrue();

    It should_not_complete_synchronously = () =>
      asyncResult.CompletedSynchronously.ShouldBeFalse();

    It should_complete = () =>
      asyncResult.IsCompleted.ShouldBeTrue();

    It should_execute_the_ActionResult = () =>
      TestResult.Executed.ShouldBeTrue();

    static void Callback(IAsyncResult ar)
    {
      asyncResult = ar;
      httpHandler.EndProcessRequest(ar);
      signal.Set();
    }
  }

  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
  public class TestAttribute : ActionFilterAttribute
  {
    public static bool OnActionExecutingCalled;
    public static bool OnActionExecutedCalled;
    public static bool OnResultExecutedCalled;
    public static bool OnResultExecutingCalled;
    string name = "test";

    public static void Reset()
    {
      OnActionExecutingCalled = false;
      OnActionExecutedCalled = false;
      OnResultExecutingCalled = false;
      OnResultExecutedCalled = false;
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      Debug.WriteLine(name + " OnActionExecuting");
      OnActionExecutingCalled = true;
    }

    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
      Debug.WriteLine(name + " OnActionExecuted");
      OnActionExecutedCalled = true;
    }

    public override void OnResultExecuted(ResultExecutedContext filterContext)
    {
      Debug.WriteLine(name + " OnResultExecuted");
      OnResultExecutedCalled = true;
    }

    public override void OnResultExecuting(ResultExecutingContext filterContext)
    {
      Debug.WriteLine(name + " OnResultExecuting");
      OnResultExecutingCalled = true;
    }
  }

  public class TestResult : ActionResult
  {
    public static bool Executed = false;

    public override void ExecuteResult(ControllerContext context)
    {
      Executed = true;
    }
  }

  [Test]
  public class TestController : AsyncController
  {
    public ActionResult Synchronous()
    {
      Debug.WriteLine("Action");
      return new EmptyResult();
    }

    public ActionResult AsyncNeverFinish()
    {
      return Async(
        callback => new AsynchronousAsyncResult(),
        result => { throw new Exception("Should never get called"); }
        );
    }

    public ActionResult AsyncFinish()
    {
      return Async(DoAsync, DoResult);
    }

    ActionResult DoResult(IAsyncResult arg)
    {
      return new TestResult();
    }

    IAsyncResult DoAsync(AsyncCallback asyncCallback)
    {
      var result = new AsynchronousAsyncResult();
      new Thread(() =>
      {
        result.SetIsCompleted(null);
        asyncCallback(result);
      }).Start();

      return result;
    }
  }

  public class with_async_infrastructure
  {
    public static IAsyncResult asyncResult;
    public static IHttpAsyncHandler httpHandler;
    public static HttpContext httpContext;

    public static void PrepareRequestTo(string controller, string action)
    {
      var routeHandler = new AsyncMvcRouteHandler();
      var httpContextBase = new Mock<HttpContextBase>();
      var sessionState = new Mock<HttpSessionStateBase>();
      var writer = new StringWriter();
      var controllerFactory = new Mock<IControllerFactory>();

      var routeData = new RouteData();
      routeData.Values["controller"] = controller;
      routeData.Values["action"] = action;

      httpContextBase.ExpectGet(x => x.Session).Returns(sessionState);

      controllerFactory.Expect(x => x.CreateController(Arg.IsAny<RequestContext>(), controller)).Returns(new TestController());

      ControllerBuilder.Current.SetControllerFactory(controllerFactory.Object);

      httpContext = new HttpContext(new HttpRequest("foo", "http://foo", "foo"), new HttpResponse(writer));
      CallContext.HostContext = httpContext;

      var requestContext = new RequestContext(httpContextBase, routeData);

      httpHandler = (IHttpAsyncHandler)routeHandler.GetHttpHandler(requestContext);
    }
  }
}