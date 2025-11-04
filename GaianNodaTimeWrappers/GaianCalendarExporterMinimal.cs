using NodaTime;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Gaian
{
    /// <summary>
    /// Minimal iCal exporter - removes all non-essential properties.
    /// No UIDs, minimal headers, just the bare minimum for compatibility.
    /// </summary>
    public class GaianCalendarExporterMinimal
    {
        private readonly StringBuilder _content;

        public GaianCalendarExporterMinimal()
        {
            _content = new StringBuilder();
            _content.AppendLine("BEGIN:VCALENDAR");
            _content.AppendLine("VERSION:2.0");
            _content.AppendLine("PRODID:-//Gaian//EN");
            _content.AppendLine("CALSCALE:GREGORIAN");
        }

        /// <summary>
        /// Generates calendar events with absolute minimum properties.
        /// </summary>
        public void GenerateDateRangeEvents(int startYear, int endYear)
        {
            LocalDate startDate = new LocalDate(startYear, 1, 1);
            LocalDate endDate = new LocalDate(endYear, 12, 31);

            LocalDate currentDate = startDate;
            while (currentDate <= endDate)
            {
                var gaianDateString = GaianTools.GaiaDateString(currentDate);
                var dateStr = currentDate.ToString("yyyyMMdd", null);
                var nextDate = currentDate.PlusDays(1).ToString("yyyyMMdd", null);

                // Absolute minimum: just DTSTART, DTEND, SUMMARY
                _content.AppendLine("BEGIN:VEVENT");
                _content.AppendLine($"DTSTART;VALUE=DATE:{dateStr}");
                _content.AppendLine($"DTEND;VALUE=DATE:{nextDate}");
                _content.AppendLine($"SUMMARY:{gaianDateString}");
                _content.AppendLine("END:VEVENT");

                currentDate = currentDate.PlusDays(1);
            }

            // Progress indicator
            Console.WriteLine($"Generated events for {endYear}");
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

            // Also write gzip compressed version
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
            Console.WriteLine($"\nUncompressed size: {fileInfo.Length / (1024.0 * 1024.0):F2} MB");
            Console.WriteLine($"Compressed size: {gzipInfo.Length / (1024.0 * 1024.0):F2} MB");
            Console.WriteLine($"Compression ratio: {(1.0 - (double)gzipInfo.Length / fileInfo.Length) * 100:F1}%");
        }

        public string GetSerializedContent()
        {
            return _content.ToString() + "END:VCALENDAR\r\n";
        }
    }
}
