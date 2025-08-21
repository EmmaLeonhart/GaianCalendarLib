using NodaTime;
using NodaTime.Calendars;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DateTimeNodaTimeExperiments
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Example date
            var zone = DateTimeZoneProviders.Tzdb.GetSystemDefault(); // or: DateTimeZoneProviders.Tzdb["America/Vancouver"]
            LocalDate date = SystemClock.Instance.GetCurrentInstant().InZone(zone).Date;
            date = new LocalDate(2026, 1, 1);
            // Use ISO week rules
            var weekYearRules = WeekYearRules.Iso;

            // Get the ISO week year and ISO week number
            int weekYear = weekYearRules.GetWeekYear(date);
            int weekOfYear = weekYearRules.GetWeekOfWeekYear(date);
            int day = (int)date.DayOfWeek; // Mon=1 … Sun=7

            //Console.WriteLine($"ISO Week Year: {weekYear}, Week: {weekOfYear}, Day: {day}");

            var (q, r) = Math.DivRem(weekOfYear, 4);

            //Console.WriteLine("q = zero indexed month");
            //Console.WriteLine("q: " + q);
            //Console.WriteLine("q + 1 = " + (q + 1));
            int month = q + 1;
            //Console.WriteLine("r = 1 indexed week");
            //Console.WriteLine("r: " + r);

            string monthName = GetMonth(month);
            int monthday = day + weekOfYear - 1;

            Console.WriteLine("Day name: " + monthName + " " + monthday);

            int i = 1;

            while (i < 365)
            {
                date = date.PlusDays(1);
                //Console.WriteLine(date);
                string g = GaiaDateString(date);
                Console.WriteLine(g);
                i++;
            }

            //Console.WriteLine("Hello, World!");
            //DateTime dt = DateTime.Now;
            //Console.WriteLine(dt.ToString());

            //var tz = DateTimeZoneProviders.Tzdb.GetSystemDefault();
            //var clock = SystemClock.Instance;

            //// Snapshot "now"
            //Instant start = clock.GetCurrentInstant();
            //ZonedDateTime startLocal = start.InZone(tz);

            //// Keep the wall-clock time constant
            //LocalDate startDate = startLocal.Date;
            //LocalTime startTime = startLocal.TimeOfDay;

            //StarDate sd = new StarDate(startTime);

            //sd = StarDate.Now;

            //Console.WriteLine($"Start (local): {startLocal}");

            //for (int i = 0; i < 365; i++)
            //{
            //    LocalDate d = startDate.PlusDays(i);
            //    LocalDateTime ldt = d + startTime;

            //    // Resolve DST gaps/overlaps sensibly
            //    ZonedDateTime zdt = tz.AtLeniently(ldt);
            //    Console.WriteLine(zdt);
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
