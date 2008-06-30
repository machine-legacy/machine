using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services
{
  public interface ILifestyleFactory
  {
    ILifestyle CreateLifestyle(ServiceEntry entry);
  }
}