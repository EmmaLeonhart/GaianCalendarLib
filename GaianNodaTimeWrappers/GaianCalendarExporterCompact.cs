using NodaTime;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Gaian
{
    /// <summary>
    /// Ultra-compact iCal exporter that minimizes file size.
    /// Removes all non-essential properties while maintaining ICS validity.
    /// </summary>
    public class GaianCalendarExporterCompact
    {
        private readonly StringBuilder _content;

        public GaianCalendarExporterCompact()
        {
            _content = new StringBuilder();
            _content.AppendLine("BEGIN:VCALENDAR");
            _content.AppendLine("VERSION:2.0");
            _content.AppendLine("PRODID:-//Gaian Calendar//EN");
            _content.AppendLine("CALSCALE:GREGORIAN");
        }

        /// <summary>
        /// Generates calendar events for all dates in a given year range.
        /// Minimal format: only required properties.
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
                    var nextDate = currentDate.PlusDays(1).ToString("yyyyMMdd", null);

                    // Ultra-minimal: only DTSTART, DTEND, SUMMARY, UID (required for most apps)
                    _content.AppendLine("BEGIN:VEVENT");
                    _content.AppendLine($"DTSTART;VALUE=DATE:{dateStr}");
                    _content.AppendLine($"DTEND;VALUE=DATE:{nextDate}");
                    _content.AppendLine($"SUMMARY:{gaianDateString}");
                    _content.AppendLine($"UID:{dateStr}@gaian");
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
        /// Outputs both .ics and .ics.gz for flexibility.
        /// </summary>
        public void WriteToFile(string filePath)
        {
            _content.AppendLine("END:VCALENDAR");
            var icsContent = _content.ToString();

            // Write uncompressed ICS
            File.WriteAllText(filePath, icsContent, Encoding.UTF8);
            Console.WriteLine($"Calendar saved to: {filePath}");

            // Also write gzip compressed version for better portability
            string gzipPath = filePath + ".gz";
            using (var fileStream = new FileStream(gzipPath, FileMode.Create))
            using (var gzipStream = new GZipStream(fileStream, CompressionMode.Compress))
            using (var writer = new StreamWriter(gzipStream, Encoding.UTF8))
            {
                writer.Write(icsContent);
            }
            Console.WriteLine($"Compressed version saved to: {gzipPath}");

            var fileInfo = new FileInfo(filePath);
            var gzipInfo = new FileInfo(gzipPath);
            Console.WriteLine($"Uncompressed size: {fileInfo.Length / (1024.0 * 1024.0):F2} MB");
            Console.WriteLine($"Compressed size: {gzipInfo.Length / (1024.0 * 1024.0):F2} MB");
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
