using NodaTime.Calendars;
using NodaTime;

namespace Gaian
{
    internal class GaianTools
    {
        //Many common operations that occur between many of these. Like formation of dates and such. No need to calculate everything afresh all the time

        public static string GaiaDateString(LocalDate date)
        {
            var weekYearRules = WeekYearRules.Iso;

            int weekYear = weekYearRules.GetWeekYear(date);          // ISO week-year
            int weekOfYear = weekYearRules.GetWeekOfWeekYear(date);    // 1..52/53
            int day = (int)date.DayOfWeek;                      // Mon=1..Sun=7

            // --- FIXED: zero-based math for 4-week "months" ---
            int monthIndex0 = (weekOfYear - 1) / 4;      // 0..12 (13 only if week 53)
            int weekInMonth0 = (weekOfYear - 1) % 4;      // 0..3
            int month = monthIndex0 + 1;           // 1..14 (14 only if week 53)
            int dayOfMonth = weekInMonth0 * 7 + day;    // 1..28

            string monthName = GetMonth(month);
            int gaianYear = weekYear + 10000;

            return $"{monthName} {dayOfMonth}, {gaianYear}";
        }



        public static string GetMonth(int month)
        {
            // 4-week months; week 53 becomes month 14 ("Horus")
            switch (month)
            {
                case 1: return "Sagittarius";
                case 2: return "Capricorn";
                case 3: return "Aquarius";
                case 4: return "Pisces";
                case 5: return "Aries";
                case 6: return "Taurus";
                case 7: return "Gemini";
                case 8: return "Cancer";
                case 9: return "Leo";
                case 10: return "Virgo";
                case 11: return "Libra";
                case 12: return "Scorpio";
                case 13: return "Ophiuchus";  // weeks 49–52
                case 14: return "Horus";      // week 53 (intercalary)
                default: throw new ArgumentOutOfRangeException(nameof(month), "Month must be 1–14.");
            }
        }

        internal static int GetDay(LocalDate date)
        {
            var weekYearRules = WeekYearRules.Iso;

            int weekYear = weekYearRules.GetWeekYear(date);          // ISO week-year
            int weekOfYear = weekYearRules.GetWeekOfWeekYear(date);    // 1..52/53
            int day = (int)date.DayOfWeek;                      // Mon=1..Sun=7

            // --- FIXED: zero-based math for 4-week "months" ---
            int monthIndex0 = (weekOfYear - 1) / 4;      // 0..12 (13 only if week 53)
            int weekInMonth0 = (weekOfYear - 1) % 4;      // 0..3
            int month = monthIndex0 + 1;           // 1..14 (14 only if week 53)
            int dayOfMonth = weekInMonth0 * 7 + day;    // 1..28

            return dayOfMonth;

            string monthName = GetMonth(month);
            int gaianYear = weekYear + 10000;
            throw new NotImplementedException();
            //return $"{monthName} {dayOfMonth}, {gaianYear}";
        }

        internal static IsoDayOfWeek GetDayOfWeek(LocalDate date)
        {
            return date.DayOfWeek;
            var weekYearRules = WeekYearRules.Iso;

            int weekYear = weekYearRules.GetWeekYear(date);          // ISO week-year
            int weekOfYear = weekYearRules.GetWeekOfWeekYear(date);    // 1..52/53
            int day = (int)date.DayOfWeek;                      // Mon=1..Sun=7

            // --- FIXED: zero-based math for 4-week "months" ---
            int monthIndex0 = (weekOfYear - 1) / 4;      // 0..12 (13 only if week 53)
            int weekInMonth0 = (weekOfYear - 1) % 4;      // 0..3
            int month = monthIndex0 + 1;           // 1..14 (14 only if week 53)
            int dayOfMonth = weekInMonth0 * 7 + day;    // 1..28

            string monthName = GetMonth(month);
            int gaianYear = weekYear + 10000;
            throw new NotImplementedException();
            //return $"{monthName} {dayOfMonth}, {gaianYear}";
        }

        internal static int GetDayOfYear(LocalDate date)
        {
            var weekYearRules = WeekYearRules.Iso;

            int weekYear = weekYearRules.GetWeekYear(date);          // ISO week-year
            int weekOfYear = weekYearRules.GetWeekOfWeekYear(date);    // 1..52/53
            int day = (int)date.DayOfWeek;                      // Mon=1..Sun=7

            return (weekOfYear - 1) * 7 + day;

            // --- FIXED: zero-based math for 4-week "months" ---
            int monthIndex0 = (weekOfYear - 1) / 4;      // 0..12 (13 only if week 53)
            int weekInMonth0 = (weekOfYear - 1) % 4;      // 0..3
            int month = monthIndex0 + 1;           // 1..14 (14 only if week 53)
            int dayOfMonth = weekInMonth0 * 7 + day;    // 1..28

            string monthName = GetMonth(month);
            int gaianYear = weekYear + 10000;
            throw new NotImplementedException();
            //return $"{monthName} {dayOfMonth}, {gaianYear}";
        }

        internal static GaianMonth GetMonth(LocalDate date)
        {
            var weekYearRules = WeekYearRules.Iso;

            int weekYear = weekYearRules.GetWeekYear(date);          // ISO week-year
            int weekOfYear = weekYearRules.GetWeekOfWeekYear(date);    // 1..52/53
            int day = (int)date.DayOfWeek;                      // Mon=1..Sun=7

            // --- FIXED: zero-based math for 4-week "months" ---
            int monthIndex0 = (weekOfYear - 1) / 4;      // 0..12 (13 only if week 53)
            int weekInMonth0 = (weekOfYear - 1) % 4;      // 0..3
            int month = monthIndex0 + 1;           // 1..14 (14 only if week 53)
            return new GaianMonth(month);
            int dayOfMonth = weekInMonth0 * 7 + day;    // 1..28

            string monthName = GetMonth(month);
            int gaianYear = weekYear + 10000;
            throw new NotImplementedException();
            //return $"{monthName} {dayOfMonth}, {gaianYear}";
        }

        internal static int GetYear(LocalDate date)
        {
            var weekYearRules = WeekYearRules.Iso;

            int weekYear = weekYearRules.GetWeekYear(date);          // ISO week-year
            int weekOfYear = weekYearRules.GetWeekOfWeekYear(date);    // 1..52/53
            int day = (int)date.DayOfWeek;                      // Mon=1..Sun=7

            // --- FIXED: zero-based math for 4-week "months" ---
            int monthIndex0 = (weekOfYear - 1) / 4;      // 0..12 (13 only if week 53)
            int weekInMonth0 = (weekOfYear - 1) % 4;      // 0..3
            int month = monthIndex0 + 1;           // 1..14 (14 only if week 53)
            int dayOfMonth = weekInMonth0 * 7 + day;    // 1..28

            string monthName = GetMonth(month);
            int gaianYear = weekYear + 10000;
            return gaianYear;
            throw new NotImplementedException();
            //return $"{monthName} {dayOfMonth}, {gaianYear}";
        }
    }
}