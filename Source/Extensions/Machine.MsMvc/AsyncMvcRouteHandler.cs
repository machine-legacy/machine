using System;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace Machine.MsMvc
{
  public class AsyncMvcRouteHandler : IRouteHandler
  {
    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
      return new AsyncMvcHandler(requestContext);
    }

    public class AsyncMvcHandler : IHttpAsyncHandler, IRequiresSessionState
    {
      readonly RequestContext _requestContext;
      AsyncController _asyncController;
      HttpContext _httpContext;
      IControllerFactory _factory;

      public AsyncMvcHandler(RequestContext context)
      {
        _requestContext = context;
      }

      // IHttpHandler members
      public bool IsReusable { get { return false; } }
      public void ProcessRequest(HttpContext httpContext) { throw new NotImplementedException(); }

      // IHttpAsyncHandler members
      public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback asyncCallback, object extraData)
      {
        IAsyncResult result = null;
        // Get the controller type
        string controllerName = _requestContext.RouteData.GetRequiredString("controller");

        // Obtain an instance of the controller
        _factory = ControllerBuilder.Current.GetControllerFactory();
        var controller = _factory.CreateController(_requestContext, controllerName) as ControllerBase;
        if (controller == null)
          throw new InvalidOperationException("Can't locate the controller " + controllerName);

        try
        {
          _asyncController = controller as AsyncController;
          if (_asyncController == null)
            throw new InvalidOperationException("Controller isn't an AsyncController.");

          // Set up asynchronous processing
          _httpContext = HttpContext.Current; // Save this for later

          result = _asyncController.Execute(_requestContext, asyncCallback);
        }
        finally
        {
          if (result == null || result.CompletedSynchronously)
          {
            this._factory.ReleaseController(controller);
          }
        }

        return result;
      }

      public void EndProcessRequest(IAsyncResult result)
      {
        try
        {
          CallContext.HostContext = _httpContext; // So that RenderView() works

          _asyncController.EndAsync();
        }
        finally
        {
          _factory.ReleaseController(_asyncController);
        }
      }
    }
  }
}