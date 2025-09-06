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
            Console.WriteLine("=== Fast Gaian Year Generator ===\n");
            
            if (args.Length == 0)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  dotnet run <gaian_year>           - Generate single year page");
                Console.WriteLine("  dotnet run csv <start> <end>      - Generate minimal CSV");
                Console.WriteLine("Examples:");
                Console.WriteLine("  dotnet run 12024");
                Console.WriteLine("  dotnet run csv 3 12100");
                return;
            }
            
            if (args[0].ToLower() == "csv")
            {
                if (args.Length < 3 || !int.TryParse(args[1], out int start) || !int.TryParse(args[2], out int end))
                {
                    Console.WriteLine("Error: CSV mode needs start and end years");
                    Console.WriteLine("Example: dotnet run csv 3 12100");
                    return;
                }
                
                string filename = args.Length >= 4 ? args[3] : "gaian_minimal.csv";
                GenerateMinimalCSV(start, end, filename);
            }
            else
            {
                if (!int.TryParse(args[0], out int gaianYear))
                {
                    Console.WriteLine("Error: Please provide a valid Gaian year number");
                    return;
                }
                
                GenerateYearPage(gaianYear);
            }
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
        
        static void GenerateMinimalCSV(int startYear, int endYear, string filename)
        {
            Console.WriteLine($"Generating minimal CSV for years {startYear} to {endYear}...");
            
            var csv = new StringBuilder();
            csv.AppendLine("GaianYear,StartDate,GregorianLeapYear,GaianLeapYear,DaysInYear");
            
            int count = 0;
            int total = endYear - startYear + 1;
            
            for (int gaianYear = startYear; gaianYear <= endYear; gaianYear++)
            {
                try
                {
                    var startDate = new GaianLocalDate(gaianYear, 1, 1);
                    var startGregorian = startDate.Value;
                    
                    int isoYear = gaianYear - 10000;
                    
                    // Check Gregorian leap year
                    bool gregorianLeap = false;
                    if (isoYear > 0)
                    {
                        gregorianLeap = DateTime.IsLeapYear(isoYear);
                    }
                    else
                    {
                        // BC years - convert and check
                        int bcYear = Math.Abs(isoYear - 1);
                        if (bcYear % 4 == 0 && (bcYear % 100 != 0 || bcYear % 400 == 0))
                            gregorianLeap = true;
                    }
                    
                    // Check Gaian leap year (has Horus month)
                    bool gaianLeap;
                    try
                    {
                        new GaianLocalDate(gaianYear, 14, 1);
                        gaianLeap = true;
                    }
                    catch
                    {
                        gaianLeap = false;
                    }
                    
                    int daysInYear = gaianLeap ? 371 : 364;
                    string startDateStr = FormatHumanDate(startGregorian, gaianYear);
                    
                    csv.AppendLine($"{gaianYear},{startDateStr},{gregorianLeap},{gaianLeap},{daysInYear}");
                    count++;
                    
                    if (count % 1000 == 0)
                    {
                        Console.WriteLine($"Progress: {count}/{total} ({100.0 * count / total:F1}%)");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing year {gaianYear}: {ex.Message}");
                }
            }
            
            File.WriteAllText(filename, csv.ToString());
            Console.WriteLine($"Generated {filename} with {count} years");
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
        
        static string FormatHumanDate(LocalDate date, int gaianYear)
        {
            int isoYear = gaianYear - 10000;
            string[] monthNames = { "January", "February", "March", "April", "May", "June",
                                   "July", "August", "September", "October", "November", "December" };
            
            if (isoYear <= 0)
            {
                int bcYear = Math.Abs(isoYear - 1);
                return $"{monthNames[date.Month - 1]} {date.Day}, {bcYear} BC";
            }
            else
            {
                return $"{monthNames[date.Month - 1]} {date.Day}, {date.Year}";
            }
        }
    }
}