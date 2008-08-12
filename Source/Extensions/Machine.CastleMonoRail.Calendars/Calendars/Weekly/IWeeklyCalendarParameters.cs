using System;

using Machine.CastleMonoRail.Calendars.Monthly;

namespace Machine.CastleMonoRail.Calendars.Weekly
{
  public interface IWeeklyCalendarParameters : ICalendarParameters
  {
    bool StartFromToday
    {
      get;
      set;
    }
    DateTime FirstVisibleDate
    {
      set;
    }
    DateTime LastVisibleDate
    {
      set;
    }
    WeeklyNavigationParameters Navigation
    {
      set;
    }
    WeeklyColumnParameters Column
    {
      set;
    }
    CalendarCellParameters Cell
    {
      set;
    }
  }
}