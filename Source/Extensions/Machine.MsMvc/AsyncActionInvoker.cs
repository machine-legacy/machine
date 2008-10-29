using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Machine.MsMvc
{
  public class AsyncActionInvoker : ControllerActionInvoker
  {
    FilterInfo _filterInfo;

    protected override FilterInfo GetFiltersForActionMethod(System.Reflection.MethodInfo methodInfo)
    {
      _filterInfo = base.GetFiltersForActionMethod(methodInfo);
      return _filterInfo;
    }

    protected override ActionExecutedContext InvokeActionMethodWithFilters(System.Reflection.MethodInfo methodInfo, IDictionary<string, object> parameters, IList<IActionFilter> filters)
    {
      if (methodInfo == null)
      {
        throw new ArgumentNullException("methodInfo");
      }
      if (parameters == null)
      {
        throw new ArgumentNullException("parameters");
      }
      if (filters == null)
      {
        throw new ArgumentNullException("filters");
      }

      ActionExecutingContext preContext = new ActionExecutingContext(ControllerContext, parameters);
      Func<ActionExecutedContext> continuation = () =>
        new ActionExecutedContext(ControllerContext, false /* canceled */, null /* exception */)
        {
          Result = InvokeActionMethod(methodInfo, parameters)
        };

      // need to reverse the filter list because the continuations are built up backward
      Func<ActionExecutedContext> thunk = filters.Reverse().Aggregate(continuation,
        (next, filter) => () => InvokeActionMethodFilter(filter, preContext, next));
      return thunk();
    }

    protected override ResultExecutedContext InvokeActionResultWithFilters(ActionResult actionResult, IList<IResultFilter> filters)
    {
      if (actionResult is AsyncResult)
      {
        actionResult.ExecuteResult(ControllerContext);
        return null;
      }
      return base.InvokeActionResultWithFilters(actionResult, filters);
    }

    static ActionExecutedContext InvokeActionMethodFilter(IActionFilter filter, ActionExecutingContext preContext, Func<ActionExecutedContext> continuation)
    {
      filter.OnActionExecuting(preContext);
      if (preContext.Result != null)
      {
        return new ActionExecutedContext(preContext, true /* canceled */, null /* exception */)
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
        postContext = new ActionExecutedContext(preContext, false /* canceled */, ex);
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
      ActionExecutedContext postContext = new ActionExecutedContext(ControllerContext, false, null);
      try
      {
        postContext.Result = invoke();
      }
      catch (Exception ex)
      {
        wasError = true;
        postContext = new ActionExecutedContext(ControllerContext, false, ex);
      }

      foreach (var filter in _filterInfo.ActionFilters)
      {
        try
        {
          filter.OnActionExecuted(postContext);
        }
        catch (Exception ex)
        {
          postContext = new ActionExecutedContext(postContext, false, ex);
        }
      }

      if (postContext.Exception != null && !postContext.ExceptionHandled)
      {
        throw postContext.Exception;
      }

      InvokeActionResultWithFilters(postContext.Result, _filterInfo.ResultFilters);
    }
  }
}