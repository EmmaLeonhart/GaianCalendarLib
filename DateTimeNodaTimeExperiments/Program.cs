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
            //Console.WriteLine(DateTime.Now.MonthName);
            // Example _date
            // Pick your time zone
            DateTimeZone zone = DateTimeZoneProviders.Tzdb["America/Vancouver"];

            // Pick your clock (normally the system clock)
            IClock clock = SystemClock.Instance;

            // Get the current "now" instant
            Instant now = clock.GetCurrentInstant();

            // Convert that to a ZonedDateTime
            ZonedDateTime zdt = now.InZone(zone);

            // Extract the LocalDate
            LocalDate today = zdt.Date;

            Console.WriteLine(today);  // e.g. 2025-08-20


            GaianLocalDate dt = new GaianLocalDate(today);

            Console.WriteLine(dt);

            Console.WriteLine(dt.Year);

            Console.WriteLine(dt.Month.ToString());


            Console.WriteLine(dt.DayOfWeek);


            Console.WriteLine(dt.DayOfYear);


            Console.WriteLine(dt.Day);






            //// Use ISO week rules
            //var weekYearRules = WeekYearRules.Iso;

            //// Get the ISO week year and ISO week number
            //int weekYear = weekYearRules.GetWeekYear(_date);
            //int weekOfYear = weekYearRules.GetWeekOfWeekYear(_date);
            //int day = (int)_date.DayOfWeek; // Mon=1 … Sun=7

            ////Console.WriteLine($"ISO Week Year: {weekYear}, Week: {weekOfYear}, Day: {day}");

            //var (q, r) = Math.DivRem(weekOfYear, 4);

            ////Console.WriteLine("q = zero indexed month");
            ////Console.WriteLine("q: " + q);
            ////Console.WriteLine("q + 1 = " + (q + 1));
            //int month = q + 1;


            //string monthName = GetMonth(month);
            //int monthday = day + weekOfYear - 1;

            //Console.WriteLine("Day name: " + monthName + " " + monthday);

            //int i = 1;

            //while (i < 365)
            //{
            //    _date = _date.PlusDays(1);
            //    //Console.WriteLine(_date);
            //    string g = GaiaDateString(_date);
            //    Console.WriteLine(g);
            //    i++;
            //}


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
