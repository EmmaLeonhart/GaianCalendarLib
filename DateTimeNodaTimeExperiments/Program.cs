using NodaTime;
using NodaTime.Calendars;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Gaian
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Pick your time zone
            DateTimeZone zone = DateTimeZoneProviders.Tzdb["America/Vancouver"];

            // Pick your clock (normally the system clock)
            IClock clock = SystemClock.Instance;

            // Get the current "now" instant
            Instant now = clock.GetCurrentInstant();

            // Convert to a ZonedDateTime
            ZonedDateTime zdt = now.InZone(zone);

            // Convert to an OffsetDateTime
            OffsetDateTime odt = zdt.ToOffsetDateTime();

            Console.WriteLine(odt); // baseline NodaTime output

            // Wrap in your GaianOffsetDateTime
            GaianOffsetDateTime gdt = new GaianOffsetDateTime(odt);

            Console.WriteLine(gdt);              // Gaian-style ToString
            Console.WriteLine(gdt.Year);         // Year number
            Console.WriteLine(gdt.Month.ToString());        // GaianMonth wrapper (calls ToString → name)
            Console.WriteLine(gdt.DayOfWeek);    // IsoDayOfWeek
            Console.WriteLine(gdt.DayOfYear);    // 1..365/366
            Console.WriteLine(gdt.Day);          // Day number in month
            Console.WriteLine(gdt.Offset);       // Offset, e.g. -07
        }



        private static string GaiaDateString(LocalDate date)
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



        private static string GetMonth(int month)
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


        //private static string GetMonth(int month)
        //{
        //    switch (month)
        //    {
        //        case 1: return "Sagittarius";
        //        case 2: return "Capricorn";
        //        case 3: return "Aquarius";
        //        case 4: return "Pisces";
        //        case 5: return "Aries";
        //        case 6: return "Taurus";
        //        case 7: return "Gemini";
        //        case 8: return "Cancer";
        //        case 9: return "Leo";
        //        case 10: return "Virgo";
        //        case 11: return "Libra";
        //        case 12: return "Scorpio";
        //        case 13: return "Ophiuchus";
        //        case 14: return "Horus";
        //        default: throw new ArgumentOutOfRangeException(nameof(month), "Month must be 1–12.");
        //    }
        //}

    }
}
