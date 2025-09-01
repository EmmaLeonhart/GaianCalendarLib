using NodaTime;
using NodaTime.Calendars;
using System.ComponentModel;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Gaian
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Gaian Calendar Advanced Formatting Demo ===\n");
            
            // Use current date
            var gaianDate = GaianLocalDate.Today;
            var gaianDateTime = GaianLocalDateTime.Now;
            Console.WriteLine(gaianDate.ToDateTimeUnspecified());
            Console.WriteLine($"Test Date: {gaianDate} (actual month: {gaianDate.Month})\n");
            
            // Demonstrate date formatting patterns
            Console.WriteLine("=== Date Formatting Patterns ===");
            Console.WriteLine($"Default:        {gaianDate}");
            Console.WriteLine($"\"MMMM d, yyyy\": {gaianDate.ToString("MMMM d, yyyy", null)}");
            Console.WriteLine($"\"MMM d, yy\":    {gaianDate.ToString("MMM d, yy", null)}");
            Console.WriteLine($"\"M/d/yyyy\":     {gaianDate.ToString("M/d/yyyy", null)}");
            Console.WriteLine($"\"MM/dd/yyyy\":   {gaianDate.ToString("MM/dd/yyyy", null)}");
            Console.WriteLine($"\"ddd\":          {gaianDate.ToString("ddd", null)}");
            Console.WriteLine($"\"dddd\":         {gaianDate.ToString("dddd", null)}");
            Console.WriteLine($"\"MMM*\":         {gaianDate.ToString("MMM*", null)}");
            Console.WriteLine($"\"W\":            {gaianDate.ToString("W", null)}");
            Console.WriteLine($"\"WWW\":          {gaianDate.ToString("WWW", null)}");
            Console.WriteLine($"\"WWWW\":         {gaianDate.ToString("WWWW", null)}");
            Console.WriteLine($"\"DDD\":          {gaianDate.ToString("DDD", null)}");
            
            Console.WriteLine("\n=== DateTime Formatting Patterns ===");
            Console.WriteLine($"Default:               {gaianDateTime}");
            Console.WriteLine($"\"MMMM d, yyyy HH:mm\": {gaianDateTime.ToString("MMMM d, yyyy HH:mm", null)}");
            Console.WriteLine($"\"MMM* d h:mm tt\":     {gaianDateTime.ToString("MMM* d h:mm tt", null)}");
            Console.WriteLine($"\"W WWW d, yyyy\":      {gaianDateTime.ToString("W WWW d, yyyy", null)}");
            Console.WriteLine($"\"ddd HH:mm:ss\":       {gaianDateTime.ToString("ddd HH:mm:ss", null)}");
            
            Console.WriteLine("\n=== Parsing Demo ===");
            string[] testInputs = { 
                "Aquarius 15, 12025", 
                "3/15/12025", 
                "12025-03-15",
                "Aqu 15, 12025"
            };
            
            foreach (var input in testInputs)
            {
                if (GaianLocalDate.TryParse(input, out var parsed))
                    Console.WriteLine($"Parsed '{input}' -> {parsed}");
                else
                    Console.WriteLine($"Failed to parse '{input}'");
            }
            
            Console.WriteLine("\n=== FromWeekYearWeekAndDay Test ===");
            // Test: Create date from ISO week-year components
            // 2024 week 10 (Aquarius period), Monday
            var fromWeek = GaianLocalDate.FromWeekYearWeekAndDay(2024, 10, IsoDayOfWeek.Monday);
            Console.WriteLine($"Week-year 2024, week 10, Monday: {fromWeek}");
            
            // Test: Create datetime with same components plus time
            var fromWeekTime = GaianLocalDateTime.FromWeekYearWeekAndDay(2024, 10, IsoDayOfWeek.Monday, new LocalTime(15, 30));
            Console.WriteLine($"Week-year 2024, week 10, Monday 15:30: {fromWeekTime}");
            
            // Verify round-trip: convert back to ISO components
            var originalDate = fromWeek.Value;  // Get underlying NodaTime LocalDate
            var weekYearRules = WeekYearRules.Iso;
            var weekYear = weekYearRules.GetWeekYear(originalDate);
            var weekOfYear = weekYearRules.GetWeekOfWeekYear(originalDate);
            var dayOfWeek = originalDate.DayOfWeek;
            Console.WriteLine($"Round-trip verification: week-year {weekYear}, week {weekOfYear}, {dayOfWeek}");
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
