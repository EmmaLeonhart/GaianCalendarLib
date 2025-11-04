using NodaTime;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Gaian
{
    /// <summary>
    /// Google Calendar compatible iCal exporter with all required fields.
    /// - Includes UID and DTSTAMP (required by Google)
    /// - Proper END:VCALENDAR closing
    /// - X-WR-TIMEZONE header
    /// - Split by year to avoid size/event limits
    /// </summary>
    public class GaianCalendarExporterGoogleCompatible
    {
        /// <summary>
        /// Generates a single iCal file for a date range with all required fields.
        /// </summary>
        public void GenerateDateRangeEvents(int year, string outputPath)
        {
            var content = new StringBuilder();

            // Header with timezone
            content.Append("BEGIN:VCALENDAR\r\n");
            content.Append("VERSION:2.0\r\n");
            content.Append("PRODID:-//Gaian//EN\r\n");
            content.Append("CALSCALE:GREGORIAN\r\n");
            content.Append("X-WR-TIMEZONE:UTC\r\n");

            LocalDate startDate = new LocalDate(year, 1, 1);
            LocalDate endDate = new LocalDate(year, 12, 31);

            LocalDate currentDate = startDate;
            int eventCount = 0;

            while (currentDate <= endDate)
            {
                var gaianDateString = GaianTools.GaiaDateString(currentDate);
                var dateStr = currentDate.ToString("yyyyMMdd", null);
                var nextDate = currentDate.PlusDays(1).ToString("yyyyMMdd", null);
                var dtstamp = DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ");

                content.Append("BEGIN:VEVENT\r\n");

                // Required by Google Calendar
                content.Append($"UID:{dateStr}-Gaian@local\r\n");
                content.Append($"DTSTAMP:{dtstamp}\r\n");

                // Date fields
                content.Append($"DTSTART;VALUE=DATE:{dateStr}\r\n");
                content.Append($"DTEND;VALUE=DATE:{nextDate}\r\n");

                // Summary
                var summaryLine = $"SUMMARY:{gaianDateString}";
                if (summaryLine.Length > 75)
                {
                    // Fold long lines
                    content.Append(summaryLine.Substring(0, 75)).Append("\r\n");
                    int pos = 75;
                    while (pos < summaryLine.Length)
                    {
                        int chunkSize = Math.Min(74, summaryLine.Length - pos);
                        content.Append(" ").Append(summaryLine.Substring(pos, chunkSize)).Append("\r\n");
                        pos += chunkSize;
                    }
                }
                else
                {
                    content.Append(summaryLine).Append("\r\n");
                }

                content.Append("END:VEVENT\r\n");

                eventCount++;
                currentDate = currentDate.PlusDays(1);
            }

            // Proper ending - CRITICAL
            content.Append("END:VCALENDAR\r\n");

            // Write with UTF-8 encoding (no BOM)
            var bytes = Encoding.UTF8.GetBytes(content.ToString());
            File.WriteAllBytes(outputPath, bytes);

            Console.WriteLine($"Generated {Path.GetFileName(outputPath)} with {eventCount} events");

            // Also create compressed version
            string gzipPath = outputPath + ".gz";
            using (var fileStream = new FileStream(gzipPath, FileMode.Create))
            using (var gzipStream = new GZipStream(fileStream, CompressionMode.Compress))
            {
                gzipStream.Write(bytes, 0, bytes.Length);
            }

            var fileInfo = new FileInfo(outputPath);
            var gzipInfo = new FileInfo(gzipPath);
            Console.WriteLine($"  Uncompressed: {fileInfo.Length / 1024.0:F1} KB");
            Console.WriteLine($"  Compressed: {gzipInfo.Length / 1024.0:F1} KB");
        }
    }
}
