using System;

namespace Machine.CastleMonoRail.Calendars.Monthly
{
  public interface IMonthlyCalendarParametersAssembler
  {
    IMonthlyCalendarParameters AssembleMonthlyParameters(DateTime selectedDate);
  }
}