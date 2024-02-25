using System.Globalization;

namespace Organiser.Cores.Models.Helpers
{
    public class MonthsBetweenDatesHelper
    {
        public MonthsBetweenDatesHelper() { }

        public static IEnumerable<(string MonthName, int Month, int Year)> MonthsBetween(DateTime startDate, DateTime endDate)
        {
            DateTime iterator;
            DateTime limit;

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            else
            {
                iterator = new DateTime(endDate.Year, endDate.Month, 1);
                limit = startDate;
            }

            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            while (iterator <= limit)
            {
                yield return (
                    dateTimeFormat.GetMonthName(iterator.Month).ToUpper(),
                    iterator.Month,
                    iterator.Year
                );

                iterator = iterator.AddMonths(1);
            }
        }
    }
}
