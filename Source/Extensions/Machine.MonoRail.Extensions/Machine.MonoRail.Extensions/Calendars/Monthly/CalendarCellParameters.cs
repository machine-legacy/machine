using System;

namespace Machine.MonoRail.Extensions.Calendars.Monthly
{
  public class CalendarCellParameters
  {
    readonly DateTime _date;
    readonly bool _isSelected;
    readonly bool _isToday;
    readonly bool _isDayOfVisibleMonth;

    public DateTime Date
    {
      get { return _date; }
    }

    public bool IsSelected
    {
      get { return _isSelected; }
    }

    public bool IsToday
    {
      get { return _isToday; }
    }

    public bool IsDayOfVisibleMonth
    {
      get { return _isDayOfVisibleMonth; }
    }

    public CalendarCellParameters(DateTime date, bool isSelected, bool isDayOfVisibleMonth, bool isToday)
    {
      _date = date;
      _isToday = isToday;
      _isDayOfVisibleMonth = isDayOfVisibleMonth;
      _isSelected = isSelected;
    }
  }
}