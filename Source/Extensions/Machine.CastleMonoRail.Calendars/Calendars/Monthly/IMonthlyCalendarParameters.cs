using System;

namespace Machine.CastleMonoRail.Calendars.Monthly
{
  public interface IMonthlyCalendarParameters : ICalendarParameters
  {
    DateTime FirstVisibleDate
    {
      set;
      get;
    }
    DateTime LastVisibleDate
    {
      set;
      get;
    }
  }
}