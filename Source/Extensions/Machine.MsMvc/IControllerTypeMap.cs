using System;

namespace Machine.MsMvc
{
  public interface IControllerTypeMap
  {
    Type LookupControllerType(string controllerName);
  }
}
