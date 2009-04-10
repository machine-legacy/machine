using System;
using System.Collections.Generic;

using Castle.MonoRail.Framework;

using Machine.Container;

namespace Machine.CastleMonoRail
{
  public class MachineControllerFactory : IControllerFactory
  {
    private readonly IControllerTree _controllerTree;
    private readonly IResolutionOnlyContainer _container;

    public MachineControllerFactory(IResolutionOnlyContainer container, IControllerTree controllerTree)
    {
      _container = container;
      _controllerTree = controllerTree;
    }

    #region IControllerFactory Members
    public IController CreateController(string area, string controller)
    {
      Type controllerType = _controllerTree.GetController(area, controller);
      if (controllerType == null)
      {
        throw new InvalidOperationException("Failed to resolve controller type: " + area + "/" + controller);
      }
      return CreateController(controllerType);
    }

    public IController CreateController(Type controllerType)
    {
      IController controller = _container.Resolve.Object(controllerType) as IController;
      if (controller == null)
      {
        throw new InvalidOperationException("Controller should be an IController: " + controllerType);
      }
      return controller;
    }

    public void Release(IController controller)
    {
      _container.Deactivate(controller);
    }
    #endregion
  }
}
