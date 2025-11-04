using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using NodaTime;
using System.IO;

namespace Gaian
{
    /// <summary>
    /// Exports Gaian calendar dates to iCal (ICS) format.
    /// Creates all-day events mapping Gregorian dates to their Gaian equivalents.
    /// </summary>
    public class GaianCalendarExporter
    {
        private readonly Calendar _calendar;

        public GaianCalendarExporter()
        {
            _calendar = new Calendar
            {
                ProductId = "-//Gaian Calendar//Gaian Calendar System//EN",
                Version = "2.0"
            };

            _calendar.Name = "Gaian Calendar Conversion";
        }

        /// <summary>
        /// Generates calendar events for all dates in a given year range.
        /// Each date gets an all-day event with the Gaian calendar equivalent.
        /// </summary>
        /// <param name="startYear">Start year (Gregorian)</param>
        /// <param name="endYear">End year (Gregorian)</param>
        public void GenerateDateRangeEvents(int startYear, int endYear)
        {
            for (int year = startYear; year <= endYear; year++)
            {
                // For each year, iterate through all days
                LocalDate startDate = new LocalDate(year, 1, 1);
                LocalDate endDate = new LocalDate(year, 12, 31);

                LocalDate currentDate = startDate;
                while (currentDate <= endDate)
                {
                    // Create event for this date
                    var gaianDateString = GaianTools.GaiaDateString(currentDate);
                    var gregorianDateString = currentDate.ToString("yyyy-MM-dd", null);

                    // Create an all-day event using just the date
                    var dateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);

                    var calendarEvent = new CalendarEvent
                    {
                        Uid = $"gaian-{gregorianDateString}@gaian-calendar",
                        Created = new CalDateTime(DateTime.UtcNow),
                        LastModified = new CalDateTime(DateTime.UtcNow),
                        DtStart = new CalDateTime(dateTime, "UTC"),
                        Summary = gaianDateString,
                        Description = $"Gregorian: {gregorianDateString}\nGaian: {gaianDateString}"
                    };

                    // Mark as all-day by only setting the date part
                    // In Ical.Net, we need to use a Date (not DateTime) for all-day events
                    calendarEvent.DtStart = new CalDateTime(dateTime.Date);
                    calendarEvent.DtEnd = new CalDateTime(dateTime.Date.AddDays(1));

                    // Add categories for filtering
                    calendarEvent.Categories.Add("Gaian Calendar");

                    _calendar.Events.Add(calendarEvent);

                    // Move to next day
                    currentDate = currentDate.PlusDays(1);
                }

                // Progress indicator (print every 10 years)
                if ((year - startYear) % 10 == 0)
                {
                    Console.WriteLine($"Generated events for year {year}...");
                }
            }
        }

        /// <summary>
        /// Writes the calendar to an ICS file.
        /// </summary>
        public void WriteToFile(string filePath)
        {
            var serializer = new CalendarSerializer();
            var serialized = serializer.SerializeToString(_calendar);
            File.WriteAllText(filePath, serialized);
            Console.WriteLine($"Calendar saved to: {filePath}");
            Console.WriteLine($"Total events: {_calendar.Events.Count}");
        }

        /// <summary>
        /// Gets the serialized calendar content as a string.
        /// </summary>
        public string? GetSerializedContent()
        {
            var serializer = new CalendarSerializer();
            return serializer.SerializeToString(_calendar);
        }
    }
}
