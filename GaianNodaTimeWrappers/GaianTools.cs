using NodaTime.Calendars;
using NodaTime;

namespace Gaian
{
    internal class GaianTools
    {

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

            int weekInMonth0 = (weekOfYear - 1) % 4;
            int dayOfMonth = weekInMonth0 * 7 + day;

            return dayOfMonth;
        }

        internal static IsoDayOfWeek GetDayOfWeek(LocalDate date)
        {
            return date.DayOfWeek;
        }

        internal static int GetDayOfYear(LocalDate date)
        {
            var weekYearRules = WeekYearRules.Iso;

            //int weekYear = weekYearRules.GetWeekYear(date);          // ISO week-year
            int weekOfYear = weekYearRules.GetWeekOfWeekYear(date);    // 1..52/53
            int day = (int)date.DayOfWeek;                      // Mon=1..Sun=7

            return (weekOfYear - 1) * 7 + day;
        }

        internal static GaianMonth GetMonth(LocalDate date)
        {
            var weekYearRules = WeekYearRules.Iso;

            //int weekYear = weekYearRules.GetWeekYear(date);          // ISO week-year
            int weekOfYear = weekYearRules.GetWeekOfWeekYear(date);    // 1..52/53
            //int day = (int)date.DayOfWeek;                      // Mon=1..Sun=7

            int monthIndex0 = (weekOfYear - 1) / 4;
            int month = monthIndex0 + 1;
            return new GaianMonth(month);
        }

        internal static int GetYear(LocalDate date)
        {
            var weekYearRules = WeekYearRules.Iso;

            int weekYear = weekYearRules.GetWeekYear(date);          // ISO week-year
            int gaianYear = weekYear + 10000;
            return gaianYear;
        }
    }
}