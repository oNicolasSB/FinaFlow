namespace FinaFlow.Core.Common;

public static class DatetimeExtension
{
    public static DateTime GetFirstDayOfMonth(this DateTime date, int? year = null, int? month = null)
        => new(year ?? date.Year, month ?? date.Month, 1);

    public static DateTime GetLastDayOfMonth(this DateTime date, int? year = null, int? month = null)
        => date.GetFirstDayOfMonth(year, month).AddMonths(1).AddDays(-1);
}