using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Machine.MsMvc
{
  public class AsyncController : Controller
  {
    Func<IAsyncResult, ActionResult> _endInvoke;
    AsyncCallback AsyncCallback { get; set; }
    IAsyncResult AsyncResult { get; set; }

    public AsyncController()
    {
      ActionInvoker = new AsyncActionInvoker();
    }

    protected override void Execute(RequestContext requestContext)
    {
      throw new NotSupportedException("Sorry, Liskov!");
    }

    public IAsyncResult Execute(RequestContext requestContext, AsyncCallback asyncCallback)
    {
      AsyncCallback = asyncCallback;

      base.Execute(requestContext);

      if (AsyncResult == null)
      {
        return new SynchronousAsyncResult();
      }

      return AsyncResult;
    }

    protected virtual AsyncResult Async(Func<AsyncCallback, IAsyncResult> beginInvoke, Func<IAsyncResult, ActionResult> endInvoke)
    {
      return new AsyncResult(beginInvoke, endInvoke);
    }

    public virtual void FinishExecute(IAsyncResult result, ControllerContext context)
    {
      this.ControllerContext = context;
    }

    public IAsyncResult BeginAsync(Func<AsyncCallback, IAsyncResult> beginInvoke, Func<IAsyncResult, ActionResult> endInvoke)
    {
      if (beginInvoke == null) throw new ArgumentNullException("beginInvoke");
      if (endInvoke == null) throw new ArgumentNullException("endInvoke");

      _endInvoke = endInvoke;
      AsyncResult = beginInvoke(AsyncCallback);

      return AsyncResult;
    }

    public void EndAsync()
    {
      AsyncActionInvoker invoker = (AsyncActionInvoker)this.ActionInvoker;
      invoker.InvokeActionEnd(() => _endInvoke(AsyncResult));
    }
  }
}