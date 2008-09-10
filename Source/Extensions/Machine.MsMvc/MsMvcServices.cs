using System;
using System.Collections.Generic;

using Machine.Container;
using Machine.Container.Plugins;

namespace Machine.MsMvc
{
  public class MsMvcServices : IServiceCollection
  {
    private readonly Type _controllerPreparerType;

    public MsMvcServices()
      : this(typeof(NullControllerPreparer))
    {
    }

    public MsMvcServices(Type controllerPreparerType)
    {
      _controllerPreparerType = controllerPreparerType;
    }

    #region IServiceCollection Members
    public void RegisterServices(ContainerRegisterer register)
    {
      register.Type<MachineControllerFactory>();
      register.Type<ReflectiveControllerTypeMap>();
      register.Type(_controllerPreparerType);
    }
    #endregion
  }
}
