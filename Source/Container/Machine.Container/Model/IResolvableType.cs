using System;
using System.Collections.Generic;

using Machine.Container.Services;

namespace Machine.Container.Model
{
  public interface IResolvableType
  {
    ServiceEntry ToServiceEntry(IResolutionServices services);
  }
}