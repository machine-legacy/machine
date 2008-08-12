using System;

namespace Machine.CastleMonoRail.Calendars.Daily
{
  public interface IDailyCalendarParameters : ICalendarParameters
  {
    TimeSpan StartTime
    {
      get; set;
    }
    TimeSpan EndTime
    {
      get; set;
    }
    TimeSpan MinorStep
    {
      get; set;
    }
    TimeSpan MajorStep
    {
      get; set;
    }
    DailyCalendarCellParameters Cell
    {
      set;
    }
  }
}