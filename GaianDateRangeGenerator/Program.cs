using NodaTime;
using System.Globalization;
using System.Text;
using Gaian;

namespace GaianDateRangeGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Fast Gaian Year Page Generator ===\n");
            
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: dotnet run <gaian_year>");
                Console.WriteLine("Example: dotnet run 12024");
                Console.WriteLine("Generates year page info for a single Gaian year instantly");
                return;
            }
            
            if (!int.TryParse(args[0], out int gaianYear))
            {
                Console.WriteLine("Error: Please provide a valid Gaian year number");
                return;
            }
            
            GenerateYearPage(gaianYear);
        }
        
        static void GenerateYearPage(int gaianYear)
        {
            try
            {
                Console.WriteLine($"=== Gaian Year {gaianYear} ===\n");
                
                // Calculate basic year info
                int isoYear = gaianYear - 10000;
                string eraDisplay = isoYear <= 0 ? $"{Math.Abs(isoYear - 1)} BC" : $"{isoYear} CE";
                Console.WriteLine($"Corresponds to: {eraDisplay}");
                
                // Get start and end dates
                var startDate = new GaianLocalDate(gaianYear, 1, 1);  // Sagittarius 1
                var startGregorian = startDate.Value;
                
                // Check if intercalary (has Horus month)
                bool hasIntercalary;
                GaianLocalDate endDate;
                try
                {
                    endDate = new GaianLocalDate(gaianYear, 14, 7);  // Try Horus 7
                    hasIntercalary = true;
                }
                catch
                {
                    endDate = new GaianLocalDate(gaianYear, 13, 28);  // Ophiuchus 28
                    hasIntercalary = false;
                }
                var endGregorian = endDate.Value;
                
                int totalDays = hasIntercalary ? 371 : 364;
                
                Console.WriteLine($"Duration: {FormatGregorianDate(startGregorian, gaianYear)} to {FormatGregorianDate(endGregorian, gaianYear)}");
                Console.WriteLine($"Total days: {totalDays} ({(hasIntercalary ? "intercalary" : "regular")} year)");
                Console.WriteLine($"Intercalary month (Horus): {(hasIntercalary ? "Yes" : "No")}");
                Console.WriteLine();
                
                // Month summary
                string[] monthNames = {
                    "Sagittarius", "Capricorn", "Aquarius", "Pisces", "Aries", "Taurus",
                    "Gemini", "Cancer", "Leo", "Virgo", "Libra", "Scorpio", "Ophiuchus", "Horus"
                };
                
                Console.WriteLine("Month breakdown:");
                for (int month = 1; month <= 13; month++)
                {
                    var monthStart = new GaianLocalDate(gaianYear, month, 1);
                    var monthEnd = new GaianLocalDate(gaianYear, month, 28);
                    Console.WriteLine($"{month,2}. {monthNames[month-1],-12} | {FormatGregorianDate(monthStart.Value, gaianYear)} to {FormatGregorianDate(monthEnd.Value, gaianYear)}");
                }
                
                if (hasIntercalary)
                {
                    var horusStart = new GaianLocalDate(gaianYear, 14, 1);
                    var horusEnd = new GaianLocalDate(gaianYear, 14, 7);
                    Console.WriteLine($"14. {"Horus",-12} | {FormatGregorianDate(horusStart.Value, gaianYear)} to {FormatGregorianDate(horusEnd.Value, gaianYear)} (intercalary)");
                }
                
                Console.WriteLine($"\nGenerated in milliseconds - no CSV bloat!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        
        static string FormatGregorianDate(LocalDate date, int gaianYear)
        {
            int isoYear = gaianYear - 10000;
            
            if (isoYear <= 0)
            {
                int bcYear = Math.Abs(isoYear - 1);
                return $"{bcYear:D4}-{date.Month:D2}-{date.Day:D2} BC";
            }
            else
            {
                return $"{date.Year:D4}-{date.Month:D2}-{date.Day:D2}";
            }
        }
    }
}