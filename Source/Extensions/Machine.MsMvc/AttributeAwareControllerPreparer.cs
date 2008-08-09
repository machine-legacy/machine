using System;
using System.Collections.Generic;
using System.Web.Mvc;

using Machine.Container.Services;
using Machine.Core.Utility;

namespace Machine.MsMvc
{
  public class AttributeAwareControllerPreparer : IControllerPreparer
  {
    private readonly IMachineContainer _container;

    public AttributeAwareControllerPreparer(IMachineContainer container)
    {
      _container = container;
    }

    #region IControllerPreparer Members
    public IController PrepareController(Type controllerType, IController controller)
    {
      Controller defaultController = controller as Controller;
      if (defaultController == null)
      {
        return controller;
      }
      ViewEngineAttribute viewEngineAttribute = ReflectionHelper.GetAttribute<ViewEngineAttribute>(controllerType, true);
      if (viewEngineAttribute != null)
      {
        IViewEngine viewEngine = (IViewEngine)_container.Resolve.Object(viewEngineAttribute.ViewEngineType);
        defaultController.ViewEngine = viewEngine;
      }
      return defaultController;
    }
    #endregion
  }
}
