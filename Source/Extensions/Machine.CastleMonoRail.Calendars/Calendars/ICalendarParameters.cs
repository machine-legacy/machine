using System;
using System.Collections.Generic;

namespace Machine.CastleMonoRail.Calendars
{
  public interface ICalendarParameters
  {
    DateTime SelectedDate
    {
      get;
      set;
    }
  }
}