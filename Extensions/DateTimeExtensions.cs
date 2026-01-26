using System;
using System.Globalization;

namespace Easy.Tools.DateTimeHelpers.Extensions
{
    /// <summary>
    /// Provides extension methods for DateTime calculations and conversions.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Calculates the exact age in years, months, and days based on the birth date and a reference date.
        /// </summary>
        /// <param name="birthDate">The date of birth.</param>
        /// <param name="referenceDate">The date to calculate the age against. Defaults to DateTime.Today if null.</param>
        /// <returns>A tuple containing the number of Years, Months, and Days.</returns>
        /// <exception cref="ArgumentException">Thrown when birthDate is in the future relative to referenceDate.</exception>
        public static (int Years, int Months, int Days) CalculateAgeDetailed(this DateTime birthDate, DateTime? referenceDate = null)
        {
            var today = referenceDate ?? DateTime.Today;

            if (birthDate > today)
                throw new ArgumentException("Birth date cannot be in the future relative to the reference date.", nameof(birthDate));

            int years = today.Year - birthDate.Year;
            int months = today.Month - birthDate.Month;
            int days = today.Day - birthDate.Day;

            // Adjust for negative days (borrow from previous month)
            if (days < 0)
            {
                months--;
                var previousMonth = today.AddMonths(-1);
                days += DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
            }

            // Adjust for negative months (borrow from previous year)
            if (months < 0)
            {
                years--;
                months += 12;
            }

            return (years, months, days);
        }

        /// <summary>
        /// Calculates the age in years. Handles leap years correctly.
        /// </summary>
        /// <param name="birthDate">The date of birth.</param>
        /// <param name="referenceDate">The date to calculate the age against. Defaults to DateTime.Today if null.</param>
        /// <returns>The age in full years.</returns>
        /// <exception cref="ArgumentException">Thrown when birthDate is in the future.</exception>
        public static int CalculateAge(this DateTime birthDate, DateTime? referenceDate = null)
        {
            var today = referenceDate?.Date ?? DateTime.Today;

            if (birthDate > today)
                throw new ArgumentException("Birth date cannot be in the future.", nameof(birthDate));

            int age = today.Year - birthDate.Year;

            // If the birthday hasn't occurred yet this year, subtract 1 from age
            if (today.Month < birthDate.Month || (today.Month == birthDate.Month && today.Day < birthDate.Day))
            {
                age--;
            }

            return age;
        }

        /// <summary>
        /// Converts a DateTime to a Unix timestamp (seconds since 1970-01-01 UTC).
        /// </summary>
        /// <param name="dateTime">The DateTime to convert.</param>
        /// <returns>The Unix timestamp as a long integer.</returns>
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            // Ensure UTC to prevent timezone offsets affecting the timestamp
            var utcDate = dateTime.Kind == DateTimeKind.Utc ? dateTime : dateTime.ToUniversalTime();
            return new DateTimeOffset(utcDate).ToUnixTimeSeconds();
        }

        /// <summary>
        /// Creates a DateTime from a Unix timestamp.
        /// </summary>
        /// <param name="unixTimestamp">The Unix timestamp in seconds.</param>
        /// <returns>A DateTime representing the timestamp (in UTC).</returns>
        public static DateTime FromUnixTimestamp(long unixTimestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
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
        /// <returns>True if the date is Monday through Friday; otherwise, false.</returns>
        public static bool IsWeekday(this DateTime date) => !date.IsWeekend();

        /// <summary>
        /// Returns a new DateTime representing the start of the day (00:00:00).
        /// </summary>
        /// <param name="date">The source date.</param>
        /// <returns>The date with time set to midnight.</returns>
        public static DateTime StartOfDay(this DateTime date) => date.Date;

        /// <summary>
        /// Returns a new DateTime representing the end of the day (23:59:59.9999999).
        /// </summary>
        /// <param name="date">The source date.</param>
        /// <returns>The date with time set to the last tick of the day.</returns>
        public static DateTime EndOfDay(this DateTime date) => date.Date.AddDays(1).AddTicks(-1);

        /// <summary>
        /// Returns the start date of the week (Monday).
        /// </summary>
        /// <param name="date">The source date.</param>
        /// <returns>The Monday of the week containing the date.</returns>
        public static DateTime StartOfWeek(this DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.Date.AddDays(-diff);
        }

        /// <summary>
        /// Returns the end date of the week (Sunday).
        /// </summary>
        /// <param name="date">The source date.</param>
        /// <returns>The Sunday of the week containing the date.</returns>
        public static DateTime EndOfWeek(this DateTime date)
        {
            return date.StartOfWeek().AddDays(6).EndOfDay();
        }

        /// <summary>
        /// Returns the next occurrence of the specified day of the week.
        /// </summary>
        /// <param name="date">The starting date.</param>
        /// <param name="dayOfWeek">The target day of the week.</param>
        /// <returns>The next date that matches the specified day of the week.</returns>
        public static DateTime NextWeekday(this DateTime date, DayOfWeek dayOfWeek)
        {
            int daysToAdd = ((int)dayOfWeek - (int)date.DayOfWeek + 7) % 7;
            if (daysToAdd == 0) daysToAdd = 7;
            return date.AddDays(daysToAdd);
        }

        /// <summary>
        /// Returns the previous occurrence of the specified day of the week.
        /// </summary>
        /// <param name="date">The starting date.</param>
        /// <param name="dayOfWeek">The target day of the week.</param>
        /// <returns>The previous date that matches the specified day of the week.</returns>
        public static DateTime PreviousWeekday(this DateTime date, DayOfWeek dayOfWeek)
        {
            int daysToSubtract = ((int)date.DayOfWeek - (int)dayOfWeek + 7) % 7;
            if (daysToSubtract == 0) daysToSubtract = 7;
            return date.AddDays(-daysToSubtract);
        }

        /// <summary>
        /// Returns the total number of whole weeks between two dates.
        /// </summary>
        /// <param name="fromDate">The start date.</param>
        /// <param name="toDate">The end date.</param>
        /// <returns>The number of full weeks.</returns>
        public static int TotalWeeksBetween(this DateTime fromDate, DateTime toDate)
        {
            return (int)((toDate.Date - fromDate.Date).TotalDays / 7);
        }

        /// <summary>
        /// Returns the first day of the month for the specified date.
        /// </summary>
        /// <param name="date">The source date.</param>
        /// <returns>The 1st of the month at 00:00:00.</returns>
        public static DateTime StartOfMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1);

        /// <summary>
        /// Returns the last day of the month for the specified date.
        /// </summary>
        /// <param name="date">The source date.</param>
        /// <returns>The last day of the month at 23:59:59.</returns>
        public static DateTime EndOfMonth(this DateTime date) => date.StartOfMonth().AddMonths(1).AddTicks(-1);

        /// <summary>
        /// Returns the number of days in the month for the specified date.
        /// </summary>
        /// <param name="date">The source date.</param>
        /// <returns>The number of days (28, 29, 30, or 31).</returns>
        public static int DaysInMonth(this DateTime date) => DateTime.DaysInMonth(date.Year, date.Month);

        /// <summary>
        /// Determines whether the year of the specified date is a leap year.
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <returns>True if it is a leap year; otherwise, false.</returns>
        public static bool IsLeapYear(this DateTime date) => DateTime.IsLeapYear(date.Year);

        /// <summary>
        /// Checks if the date is between two specified dates (inclusive).
        /// </summary>
        /// <param name="dateTime">The date to check.</param>
        /// <param name="start">The start date.</param>
        /// <param name="end">The end date.</param>
        /// <returns>True if the date falls within the range.</returns>
        public static bool IsBetween(this DateTime dateTime, DateTime start, DateTime end)
        {
            return dateTime >= start && dateTime <= end;
        }

        /// <summary>
        /// Converts the DateTime to the specified time zone.
        /// </summary>
        /// <param name="dateTime">The source DateTime.</param>
        /// <param name="timeZoneId">The ID of the target time zone (e.g., "Eastern Standard Time").</param>
        /// <returns>The converted DateTime.</returns>
        public static DateTime ConvertToTimeZone(this DateTime dateTime, string timeZoneId)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTime(dateTime, timeZone);
        }

        /// <summary>
        /// Returns the number of days until the specified date from today.
        /// </summary>
        /// <param name="targetDate">The target date.</param>
        /// <returns>The number of days remaining.</returns>
        public static int DaysUntil(this DateTime targetDate)
        {
            return (targetDate.Date - DateTime.Today).Days;
        }

        /// <summary>
        /// Returns the total number of days in the year.
        /// </summary>
        /// <param name="date">The source date.</param>
        /// <returns>366 for leap years, otherwise 365.</returns>
        public static int DaysInYear(this DateTime date) => DateTime.IsLeapYear(date.Year) ? 366 : 365;

        /// <summary>
        /// Returns the total number of weeks in the year based on the ISO 8601 standard.
        /// </summary>
        /// <param name="date">The source date.</param>
        /// <returns>52 or 53 weeks.</returns>
        public static int WeeksInYear(this DateTime date)
        {
            // ISO 8601 Rule:
            // If Jan 1 is a Thursday, or if it's a leap year and Jan 1 is a Wednesday, then it has 53 weeks.
            // Otherwise, it has 52 weeks.
            var jan1 = new DateTime(date.Year, 1, 1);
            if (jan1.DayOfWeek == DayOfWeek.Thursday ||
               (DateTime.IsLeapYear(date.Year) && jan1.DayOfWeek == DayOfWeek.Wednesday))
            {
                return 53;
            }
            return 52;
        }

        /// <summary>
        /// Checks if two DateTime values represent the same calendar date (ignores time).
        /// </summary>
        /// <param name="date">The first date.</param>
        /// <param name="otherDate">The second date.</param>
        /// <returns>True if they are the same day.</returns>
        public static bool IsSameDate(this DateTime date, DateTime otherDate) => date.Date == otherDate.Date;

        /// <summary>
        /// Adds a specified number of business days (weekdays) to the date.
        /// Note: This method skips weekends but does not account for official holidays.
        /// </summary>
        /// <param name="date">The starting date.</param>
        /// <param name="days">The number of business days to add.</param>
        /// <returns>The new date.</returns>
        public static DateTime AddBusinessDays(this DateTime date, int days)
        {
            if (days == 0) return date;

            int sign = Math.Sign(days);
            int unsignedDays = Math.Abs(days);

            for (int i = 0; i < unsignedDays; i++)
            {
                do
                {
                    date = date.AddDays(sign);
                }
                while (date.IsWeekend());
            }

            return date;
        }

        /// <summary>
        /// Subtracts a specified number of business days (weekdays) from the date.
        /// Note: This method skips weekends but does not account for official holidays.
        /// </summary>
        /// <param name="date">The starting date.</param>
        /// <param name="days">The number of business days to subtract.</param>
        /// <returns>The new date.</returns>
        public static DateTime SubtractBusinessDays(this DateTime date, int days)
        {
            return date.AddBusinessDays(-days);
        }
    }
}