using System;
using System.Collections.Generic;

namespace Machine.CastleMonoRail.Calendars.Monthly
{
  public interface IMonthlyCalendarView : IMonthlyCalendarParameters
  {
    MonthlyNavigationParameters Navigation
    { 
      get;
      set;
    }
    MonthlyColumnParameters Column
    {
      get;
      set;
    }
    CalendarCellParameters Cell
    {
      get;
      set;
    }
  }
}
