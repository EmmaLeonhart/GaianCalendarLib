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
            Console.WriteLine("=== Gaian Calendar BC Date Range Generator ===\n");
            
            // Parse command line arguments
            int startYear = 0;        // Default: Year 0 GE (10001 BC)
            int endYear = 9999;       // Default: Year 9999 GE (1 BC)
            string outputFile = "gaian_bc_dates.csv";
            bool detailedMode = false;  // New flag for detailed day-by-day output
            
            if (args.Length >= 1 && int.TryParse(args[0], out int argStart))
                startYear = argStart;
            if (args.Length >= 2 && int.TryParse(args[1], out int argEnd))
                endYear = argEnd;
            if (args.Length >= 3)
                outputFile = args[2];
            if (args.Length >= 4 && args[3].ToLower() == "detailed")
                detailedMode = true;
            
            Console.WriteLine($"Generating date ranges for Gaian years {startYear} to {endYear}");
            Console.WriteLine($"Output file: {outputFile}");
            Console.WriteLine($"Mode: {(detailedMode ? "Detailed (day-by-day)" : "Summary (year ranges)")}\n");
            
            if (detailedMode)
                GenerateDetailedCSV(startYear, endYear, outputFile);
            else
                GenerateCSV(startYear, endYear, outputFile);
            
            Console.WriteLine($"\nCompleted! Generated {(detailedMode ? "detailed day mappings" : "year summaries")}.");
            Console.WriteLine($"CSV file saved as: {outputFile}");
        }
        
        static void GenerateCSV(int startYear, int endYear, string fileName)
        {
            var csv = new StringBuilder();
            
            // CSV header
            csv.AppendLine("GaianYear,StartDate,EndDate,ISOYear,DaysInYear,HasIntercalary,StartWeekday,EndWeekday");
            
            int successCount = 0;
            int failCount = 0;
            
            for (int gaianYear = startYear; gaianYear <= endYear; gaianYear++)
            {
                try
                {
                    // Create first day of Gaian year (Sagittarius 1)
                    var startDate = new GaianLocalDate(gaianYear, 1, 1);
                    
                    // Determine if this is an intercalary year (has Horus month)
                    bool hasIntercalary;
                    GaianLocalDate endDate;
                    
                    try
                    {
                        // Try to create Horus 7 (last day of intercalary month)
                        endDate = new GaianLocalDate(gaianYear, 14, 7);
                        hasIntercalary = true;
                    }
                    catch
                    {
                        // No Horus month, end with Ophiuchus 28
                        endDate = new GaianLocalDate(gaianYear, 13, 28);
                        hasIntercalary = false;
                    }
                    
                    // Get the underlying NodaTime LocalDate values
                    var startLocalDate = startDate.Value;
                    var endLocalDate = endDate.Value;
                    
                    // Calculate ISO year (Gaian year - 10000)
                    int isoYear = gaianYear - 10000;
                    
                    // Calculate days in year
                    int daysInYear = hasIntercalary ? 371 : 364;
                    
                    // Get weekdays
                    string startWeekday = startLocalDate.DayOfWeek.ToString();
                    string endWeekday = endLocalDate.DayOfWeek.ToString();
                    
                    // Format dates as ISO strings (YYYY-MM-DD format)
                    string startDateStr, endDateStr;
                    
                    if (isoYear <= 0)
                    {
                        // BC years: format as negative or with BC suffix
                        int bcYear = Math.Abs(isoYear - 1); // Convert to BC (0 = 1 BC, -1 = 2 BC, etc.)
                        startDateStr = $"{bcYear:D4}-{startLocalDate.Month:D2}-{startLocalDate.Day:D2} BC";
                        endDateStr = $"{bcYear:D4}-{endLocalDate.Month:D2}-{endLocalDate.Day:D2} BC";
                    }
                    else
                    {
                        // AD years: normal format
                        startDateStr = $"{startLocalDate.Year:D4}-{startLocalDate.Month:D2}-{startLocalDate.Day:D2}";
                        endDateStr = $"{endLocalDate.Year:D4}-{endLocalDate.Month:D2}-{endLocalDate.Day:D2}";
                    }
                    
                    // Add CSV row
                    csv.AppendLine($"{gaianYear},{startDateStr},{endDateStr},{isoYear},{daysInYear},{hasIntercalary},{startWeekday},{endWeekday}");
                    
                    successCount++;
                    
                    // Progress indicator every 100 years
                    if (gaianYear % 100 == 0)
                    {
                        Console.WriteLine($"Processed year {gaianYear} ({gaianYear - startYear + 1}/{endYear - startYear + 1})");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing Gaian year {gaianYear}: {ex.Message}");
                    failCount++;
                }
            }
            
            // Write to file
            File.WriteAllText(fileName, csv.ToString());
            
            Console.WriteLine($"Processing complete: {successCount} successful, {failCount} failed");
        }
        
        static void GenerateDetailedCSV(int startYear, int endYear, string fileName)
        {
            var csv = new StringBuilder();
            
            // CSV header for detailed mode
            csv.AppendLine("GaianYear,GaianMonth,GaianDay,MonthName,GregorianDate,ISOYear,Weekday");
            
            int successCount = 0;
            int failCount = 0;
            
            string[] monthNames = {
                "Sagittarius", "Capricorn", "Aquarius", "Pisces", "Aries", "Taurus",
                "Gemini", "Cancer", "Leo", "Virgo", "Libra", "Scorpio", "Ophiuchus", "Horus"
            };
            
            for (int gaianYear = startYear; gaianYear <= endYear; gaianYear++)
            {
                try
                {
                    // Process regular months (1-13)
                    for (int month = 1; month <= 13; month++)
                    {
                        for (int day = 1; day <= 28; day++)
                        {
                            try
                            {
                                var gaianDate = new GaianLocalDate(gaianYear, month, day);
                                var gregorianDate = gaianDate.Value;
                                var monthName = monthNames[month - 1];
                                
                                // Format gregorian date
                                string gregorianStr = FormatGregorianDate(gregorianDate, gaianYear);
                                int isoYear = gaianYear - 10000;
                                string weekday = gregorianDate.DayOfWeek.ToString();
                                
                                csv.AppendLine($"{gaianYear},{month},{day},{monthName},{gregorianStr},{isoYear},{weekday}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error processing {gaianYear}/{month}/{day}: {ex.Message}");
                                failCount++;
                            }
                        }
                    }
                    
                    // Process Horus intercalary days if they exist
                    try
                    {
                        for (int day = 1; day <= 7; day++)
                        {
                            var gaianDate = new GaianLocalDate(gaianYear, 14, day);
                            var gregorianDate = gaianDate.Value;
                            
                            string gregorianStr = FormatGregorianDate(gregorianDate, gaianYear);
                            int isoYear = gaianYear - 10000;
                            string weekday = gregorianDate.DayOfWeek.ToString();
                            
                            csv.AppendLine($"{gaianYear},14,{day},Horus,{gregorianStr},{isoYear},{weekday}");
                        }
                    }
                    catch
                    {
                        // No Horus days in this year - skip
                    }
                    
                    successCount++;
                    
                    // Progress indicator
                    if (gaianYear % 10 == 0)
                    {
                        Console.WriteLine($"Processed year {gaianYear}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing year {gaianYear}: {ex.Message}");
                    failCount++;
                }
            }
            
            // Write to file
            File.WriteAllText(fileName, csv.ToString());
            
            Console.WriteLine($"Processing complete: {successCount} years successful, {failCount} errors");
        }
        
        static string FormatGregorianDate(LocalDate date, int gaianYear)
        {
            int isoYear = gaianYear - 10000;
            
            if (isoYear <= 0)
            {
                // BC years: format with BC suffix
                int bcYear = Math.Abs(isoYear - 1); // Convert to BC (0 = 1 BC, -1 = 2 BC, etc.)
                return $"{bcYear:D4}-{date.Month:D2}-{date.Day:D2} BC";
            }
            else
            {
                // AD years: normal format
                return $"{date.Year:D4}-{date.Month:D2}-{date.Day:D2}";
            }
        }
    }
}