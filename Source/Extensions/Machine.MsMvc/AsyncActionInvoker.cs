using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace Machine.MsMvc
{
  public class AsyncActionInvoker : ControllerActionInvoker
  {
    FilterInfo _filterInfo;
    ControllerContext _controllerContext;
    ActionDescriptor _actionDescriptor;

    protected override ActionExecutedContext InvokeActionMethodWithFilters(ControllerContext controllerContext, IList<IActionFilter> filters, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)
    {
      ActionExecutingContext preContext = new ActionExecutingContext(controllerContext, actionDescriptor, parameters);
      Func<ActionExecutedContext> continuation = () =>
        new ActionExecutedContext(controllerContext, actionDescriptor, false /* canceled */, null /* exception */)
        {
          Result = InvokeActionMethod(controllerContext, actionDescriptor, parameters)
        };

      // need to reverse the filter list because the continuations are built up backward
      Func<ActionExecutedContext> thunk = filters.Reverse().Aggregate(continuation,
        (next, filter) => () => InvokeActionMethodFilter(filter, preContext, next));
      return thunk();
    }

    protected override FilterInfo GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
    {
      // XXX: this is a terribl place to set these, but whatever
      _controllerContext = controllerContext;
      _actionDescriptor = actionDescriptor;
      _filterInfo = base.GetFilters(controllerContext, actionDescriptor);
      return _filterInfo;
    } 

    protected override ResultExecutedContext InvokeActionResultWithFilters(ControllerContext controllerContext, IList<IResultFilter> filters, ActionResult actionResult)
    {
      if (actionResult is AsyncResult)
      {
        actionResult.ExecuteResult(controllerContext);
        return null;
      }
      return base.InvokeActionResultWithFilters(controllerContext, filters, actionResult);
    }

    static ActionExecutedContext InvokeActionMethodFilter(IActionFilter filter, ActionExecutingContext preContext, Func<ActionExecutedContext> continuation)
    {
      filter.OnActionExecuting(preContext);
      if (preContext.Result != null)
      {
        return new ActionExecutedContext(preContext, preContext.ActionDescriptor, true /* canceled */, null /* exception */)
        {
          Result = preContext.Result
        };
      }

      bool wasError = false;
      ActionExecutedContext postContext = null;
      try
      {
        postContext = continuation();
      }
      catch (Exception ex)
      {
        wasError = true;
        postContext = new ActionExecutedContext(preContext, preContext.ActionDescriptor, false /* canceled */, ex);
        filter.OnActionExecuted(postContext);
        if (!postContext.ExceptionHandled)
        {
          throw;
        }
      }
      if (!wasError)
      {
        if (!(postContext.Result is AsyncResult))
        {
          filter.OnActionExecuted(postContext);
        }
      }
      return postContext;
    }

    public void InvokeActionEnd(Func<ActionResult> invoke)
    {
      bool wasError = false;
      ActionExecutedContext postContext = new ActionExecutedContext(_controllerContext, _actionDescriptor, false, null);
      try
      {
        postContext.Result = invoke();
      }
      catch (Exception ex)
      {
        wasError = true;
        postContext = new ActionExecutedContext(_controllerContext, _actionDescriptor, false, ex);
      }

      foreach (var filter in _filterInfo.ActionFilters)
      {
        try
        {
          filter.OnActionExecuted(postContext);
        }
        catch (Exception ex)
        {
          postContext = new ActionExecutedContext(_controllerContext, _actionDescriptor, false, ex);
        }
      }

      if (postContext.Exception != null && !postContext.ExceptionHandled)
      {
        throw postContext.Exception;
      }

      InvokeActionResultWithFilters(_controllerContext, _filterInfo.ResultFilters, postContext.Result);
    }
  }
}