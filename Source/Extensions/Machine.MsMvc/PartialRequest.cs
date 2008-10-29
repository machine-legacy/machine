using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Machine.MsMvc
{
  public class PartialRequest
  {
    public RouteValueDictionary RouteValues { get; private set; }

    public PartialRequest(object routeValues)
    {
      RouteValues = new RouteValueDictionary(routeValues);
    }

    public void Invoke(ControllerContext context)
    {
      RouteData rd = new RouteData(context.RouteData.Route, context.RouteData.RouteHandler);
      foreach (var pair in RouteValues)
      {
        rd.Values.Add(pair.Key, pair.Value);
      }
      IHttpHandler handler = new MvcHandler(new RequestContext(context.HttpContext, rd));
      handler.ProcessRequest(HttpContext.Current);
    }
  }
}