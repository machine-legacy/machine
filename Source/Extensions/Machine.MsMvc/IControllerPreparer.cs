using System;
using System.Web.Mvc;

namespace Machine.MsMvc
{
  public interface IControllerPreparer
  {
    IController PrepareController(Type controllerType, IController controller);
  }
}
