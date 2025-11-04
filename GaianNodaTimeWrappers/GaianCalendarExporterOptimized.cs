using NodaTime;
using System.IO;
using System.Text;

namespace Gaian
{
    /// <summary>
    /// Optimized iCal exporter that generates minimal but valid ICS format.
    /// Uses raw string building instead of Ical.Net to reduce file size significantly.
    /// </summary>
    public class GaianCalendarExporterOptimized
    {
        private readonly StringBuilder _content;

        public GaianCalendarExporterOptimized()
        {
            _content = new StringBuilder();
            _content.AppendLine("BEGIN:VCALENDAR");
            _content.AppendLine("VERSION:2.0");
            _content.AppendLine("PRODID:-//Gaian Calendar//Gaian Calendar System//EN");
            _content.AppendLine("CALSCALE:GREGORIAN");
            _content.AppendLine("METHOD:PUBLISH");
            _content.AppendLine("X-WR-CALNAME:Gaian Calendar Conversion");
            _content.AppendLine("X-WR-TIMEZONE:UTC");
        }

        /// <summary>
        /// Generates calendar events for all dates in a given year range.
        /// Each date gets a minimal all-day event with the Gaian calendar equivalent.
        /// </summary>
        public void GenerateDateRangeEvents(int startYear, int endYear)
        {
            for (int year = startYear; year <= endYear; year++)
            {
                LocalDate startDate = new LocalDate(year, 1, 1);
                LocalDate endDate = new LocalDate(year, 12, 31);

                LocalDate currentDate = startDate;
                while (currentDate <= endDate)
                {
                    var gaianDateString = GaianTools.GaiaDateString(currentDate);
                    var dateStr = currentDate.ToString("yyyyMMdd", null);

                    // Minimal event format for all-day events
                    _content.AppendLine("BEGIN:VEVENT");
                    _content.AppendLine($"DTSTART;VALUE=DATE:{dateStr}");
                    _content.AppendLine($"DTEND;VALUE=DATE:{currentDate.PlusDays(1).ToString("yyyyMMdd", null)}");
                    _content.AppendLine($"SUMMARY:{gaianDateString}");
                    _content.AppendLine($"UID:gaian-{dateStr}@gaian-calendar");
                    _content.AppendLine("END:VEVENT");

                    currentDate = currentDate.PlusDays(1);
                }

                // Progress indicator
                if ((year - startYear) % 10 == 0)
                {
                    Console.WriteLine($"Generated events for year {year}...");
                }
            }
        }

        /// <summary>
        /// Finalizes and writes the calendar to an ICS file.
        /// </summary>
        public void WriteToFile(string filePath)
        {
            _content.AppendLine("END:VCALENDAR");

            File.WriteAllText(filePath, _content.ToString(), Encoding.UTF8);

            Console.WriteLine($"Calendar saved to: {filePath}");

            var lines = _content.ToString().Split('\n').Length;
            Console.WriteLine($"Total lines: {lines}");
        }

        /// <summary>
        /// Gets the serialized calendar content as a string.
        /// </summary>
        public string GetSerializedContent()
        {
            return _content.ToString() + "END:VCALENDAR\r\n";
        }
    }
}
