using System;
using System.Globalization;

namespace Easy.Tools.DateTimeHelpers.Extensions
{
    /// <summary>
    /// Extension methods for DateTime calculations and conversions.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Calculates the exact age as years, months, and days from birthDate to referenceDate (defaults to today).
        /// It does not count the current year as completed unless the birth month and day have passed.
        /// </summary>
        /// <param name="birthDate">Date of birth.</param>
        /// <param name="referenceDate">Reference date to calculate age against. Defaults to today.</param>
        /// <returns>Tuple of years, months, and days representing the age.</returns>
        public static (int Years, int Months, int Days) CalculateAgeDetailed(this DateTime birthDate, DateTime? referenceDate = null)
        {
            var today = referenceDate ?? DateTime.Today;

            if (birthDate > today)
                throw new ArgumentException("Birthdate cannot be in the future.", nameof(birthDate));

            int years = today.Year - birthDate.Year;
            int months = today.Month - birthDate.Month;
            int days = today.Day - birthDate.Day;

            if (days < 0)
            {
                months--;
                var previousMonth = today.AddMonths(-1);
                days += DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }

            return (years, months, days);
        }

        /// <summary>
        /// Calculates the age in years based on the birth date and an optional reference date.
        /// Correctly handles cases where the birthday hasn't occurred yet in the reference year,
        /// including leap year birthdays on February 29.
        /// </summary>
        /// <param name="birthDate">The birth date to calculate age from.</param>
        /// <param name="referenceDate">The date to calculate the age at. Defaults to DateTime.Today if null.</param>
        /// <returns>The age in full years.</returns>
        public static int CalculateAge(this DateTime birthDate, DateTime? referenceDate = null)
        {
            var today = referenceDate?.Date ?? DateTime.Today;

            if (birthDate > today)
                throw new ArgumentException("Birth date cannot be in the future.", nameof(birthDate));

            int age = today.Year - birthDate.Year;

            // Handle leap day birthdays (Feb 29)
            DateTime birthdayThisYear;
            if (birthDate.Month == 2 && birthDate.Day == 29 && !DateTime.IsLeapYear(today.Year))
            {
                // For non-leap years, treat birthday as Feb 28
                birthdayThisYear = new DateTime(today.Year, 2, 28);
            }
            else
            {
                birthdayThisYear = new DateTime(today.Year, birthDate.Month, birthDate.Day);
            }

            if (today < birthdayThisYear)
                age--;

            return age;
        }

        /// <summary>
        /// Converts a DateTime to Unix timestamp (seconds since 1970-01-01 UTC).
        /// </summary>
        /// <param name="dateTime">The DateTime to convert. Assumed to be in UTC or converted to UTC.</param>
        /// <returns>The Unix timestamp as a long integer.</returns>
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            var utcDate = dateTime.Kind == DateTimeKind.Utc ? dateTime : dateTime.ToUniversalTime();
            var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((utcDate - unixStart).TotalSeconds);
        }

        /// <summary>
        /// Creates a DateTime from a Unix timestamp (seconds since 1970-01-01 UTC).
        /// </summary>
        /// <param name="unixTimestamp">The Unix timestamp in seconds.</param>
        /// <returns>A DateTime instance in UTC.</returns>
        public static DateTime FromUnixTimestamp(long unixTimestamp)
        {
            var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return unixStart.AddSeconds(unixTimestamp);
        }

        /// <summary>
        /// Determines whether the given date falls on a weekend (Saturday or Sunday).
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <returns>True if the date is Saturday or Sunday; otherwise, false.</returns>
        public static bool IsWeekend(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Determines whether the given date falls on a weekday (Monday to Friday).
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <returns>True if the date is Monday to Friday; otherwise, false.</returns>
        public static bool IsWeekday(this DateTime date)
        {
            return !date.IsWeekend();
        }

        /// <summary>
        /// Returns a new DateTime representing the start of the day (00:00:00) for the given date.
        /// </summary>
        /// <param name="date">The date to get the start of day for.</param>
        /// <returns>DateTime at 00:00:00 on the same date.</returns>
        public static DateTime StartOfDay(this DateTime date)
        {
            return date.Date;
        }
        /// <summary>
        /// Returns a new DateTime representing the end of the day (23:59:59.9999999) for the given date.
        /// </summary>
        /// <param name="date">The date to get the end of day for.</param>
        /// <returns>DateTime at 23:59:59.9999999 on the same date.</returns>
        public static DateTime EndOfDay(this DateTime date)
        {
            return date.Date.AddDays(1).AddTicks(-1);
        }

        /// <summary>
        /// Returns the start date of the week containing the specified date.
        /// Week start is considered Monday.
        /// </summary>
        /// <param name="date">The date to get the start of the week for.</param>
        /// <returns>The Monday date of the week containing the specified date.</returns>
        public static DateTime StartOfWeek(this DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.Date.AddDays(-diff);
        }

        /// <summary>
        /// Returns the end date of the week containing the specified date.
        /// Week end is considered Sunday.
        /// </summary>
        /// <param name="date">The date to get the end of the week for.</param>
        /// <returns>The Sunday date of the week containing the specified date.</returns>
        public static DateTime EndOfWeek(this DateTime date)
        {
            return date.StartOfWeek().AddDays(6).EndOfDay();
        }

        /// <summary>
        /// Returns the next specified weekday after the given date.
        /// </summary>
        /// <param name="date">The starting date.</param>
        /// <param name="dayOfWeek">The target day of the week.</param>
        /// <returns>The next date with the specified weekday.</returns>
        public static DateTime NextWeekday(this DateTime date, DayOfWeek dayOfWeek)
        {
            int daysToAdd = ((int)dayOfWeek - (int)date.DayOfWeek + 7) % 7;
            if (daysToAdd == 0)
                daysToAdd = 7;
            return date.AddDays(daysToAdd);
        }

        /// <summary>
        /// Returns the previous specified weekday before the given date.
        /// </summary>
        /// <param name="date">The starting date.</param>
        /// <param name="dayOfWeek">The target day of the week.</param>
        /// <returns>The previous date with the specified weekday.</returns>
        public static DateTime PreviousWeekday(this DateTime date, DayOfWeek dayOfWeek)
        {
            int daysToSubtract = ((int)date.DayOfWeek - (int)dayOfWeek + 7) % 7;
            if (daysToSubtract == 0)
                daysToSubtract = 7;
            return date.AddDays(-daysToSubtract);
        }

        /// <summary>
        /// Returns the total number of whole weeks between two dates.
        /// </summary>
        /// <param name="fromDate">The start date.</param>
        /// <param name="toDate">The end date.</param>
        /// <returns>The total number of weeks between fromDate and toDate.</returns>
        public static int TotalWeeksBetween(this DateTime fromDate, DateTime toDate)
        {
            return (int)((toDate.Date - fromDate.Date).TotalDays / 7);
        }

        /// <summary>
        /// Returns the start date of the month containing the specified date.
        /// Time is set to 00:00:00.
        /// </summary>
        /// <param name="date">The date to get the start of the month for.</param>
        /// <returns>The first day of the month at 00:00:00.</returns>
        public static DateTime StartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Returns the end date of the month containing the specified date.
        /// Time is set to 23:59:59.9999999.
        /// </summary>
        /// <param name="date">The date to get the end of the month for.</param>
        /// <returns>The last day of the month at 23:59:59.9999999.</returns>
        public static DateTime EndOfMonth(this DateTime date)
        {
            return date.StartOfMonth().AddMonths(1).AddTicks(-1);
        }

        /// <summary>
        /// Returns the number of days in the month for the specified date.
        /// </summary>
        /// <param name="date">The date to get the days in month for.</param>
        /// <returns>The total number of days in the month.</returns>
        public static int DaysInMonth(this DateTime date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month);
        }

        /// <summary>
        /// Determines whether the specified year is a leap year.
        /// </summary>
        /// <param name="date">The date to check the year for.</param>
        /// <returns>True if the year is a leap year; otherwise, false.</returns>
        public static bool IsLeapYear(this DateTime date)
        {
            return DateTime.IsLeapYear(date.Year);
        }

        /// <summary>
        /// Checks if the DateTime is between two specified DateTimes (inclusive).
        /// </summary>
        /// <param name="dateTime">The DateTime to check.</param>
        /// <param name="start">Start of the range.</param>
        /// <param name="end">End of the range.</param>
        /// <returns>True if dateTime is between start and end (inclusive); otherwise false.</returns>
        public static bool IsBetween(this DateTime dateTime, DateTime start, DateTime end)
        {
            return dateTime >= start && dateTime <= end;
        }

        /// <summary>
        /// Converts the DateTime to the specified time zone.
        /// </summary>
        /// <param name="dateTime">The DateTime to convert.</param>
        /// <param name="timeZoneId">The destination time zone ID (e.g., "Eastern Standard Time").</param>
        /// <returns>The DateTime converted to the specified time zone.</returns>
        public static DateTime ConvertToTimeZone(this DateTime dateTime, string timeZoneId)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTime(dateTime, timeZone);
        }

        /// <summary>
        /// Returns the number of days until the specified date from now.
        /// </summary>
        /// <param name="targetDate">The target DateTime.</param>
        /// <returns>Number of days until targetDate. Negative if targetDate is in the past.</returns>
        public static int DaysUntil(this DateTime targetDate)
        {
            return (targetDate.Date - DateTime.Today).Days;
        }

        /// <summary>
        /// Returns the total number of days in the year of the specified date.
        /// </summary>
        /// <param name="date">The date to get the days in the year for.</param>
        /// <returns>The total number of days in the year (365 or 366).</returns>
        public static int DaysInYear(this DateTime date)
        {
            return DateTime.IsLeapYear(date.Year) ? 366 : 365;
        }

        /// <summary>
        /// Returns the total number of weeks in the year of the specified date.
        /// Uses ISO 8601 week date system where a week starts on Monday and the first week has the year's first Thursday.
        /// </summary>
        /// <param name="date">The date to get the weeks in the year for.</param>
        /// <returns>The total number of weeks in the year (52 or 53).</returns>
        public static int WeeksInYear(this DateTime date)
        {
            // ISO 8601: weeks start on Monday, and week 1 is the one with the first Thursday of the year
            var jan1 = new DateTime(date.Year, 1, 1);
            var dec31 = new DateTime(date.Year, 12, 31);

            var cal = System.Globalization.CultureInfo.InvariantCulture.Calendar;
            var firstWeek = cal.GetWeekOfYear(jan1, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            var lastWeek = cal.GetWeekOfYear(dec31, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            return lastWeek == 1 ? 52 : lastWeek; // If lastWeek is 1, year has 52 weeks; otherwise lastWeek (52 or 53)
        }

        /// <summary>
        /// Checks if two DateTime values represent the same calendar date (ignores time).
        /// </summary>
        /// <param name="date">First date.</param>
        /// <param name="otherDate">Second date.</param>
        /// <returns>True if both dates represent the same day; otherwise false.</returns>
        public static bool IsSameDate(this DateTime date, DateTime otherDate)
        {
            return date.Date == otherDate.Date;
        }

        /// <summary>
        /// Adds a specified number of business days (weekdays) to the date.
        /// </summary>
        /// <param name="date">The starting date.</param>
        /// <param name="days">The number of business days to add.</param>
        /// <returns>The resulting date after adding the business days.</returns>
        public static DateTime AddBusinessDays(this DateTime date, int days)
        {
            if (days == 0) return date;

            int direction = days < 0 ? -1 : 1;
            int absDays = Math.Abs(days);

            while (absDays > 0)
            {
                date = date.AddDays(direction);
                if (date.IsWeekday())
                    absDays--;
            }

            return date;
        }

        /// <summary>
        /// Subtracts a specified number of business days (weekdays) from the date.
        /// </summary>
        /// <param name="date">The starting date.</param>
        /// <param name="days">The number of business days to subtract.</param>
        /// <returns>The resulting date after subtracting the business days.</returns>
        public static DateTime SubtractBusinessDays(this DateTime date, int days)
        {
            return date.AddBusinessDays(-days);
        }

    }
}
