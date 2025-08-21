using NodaTime;

namespace DateTimeNodaTimeExperiments
{
    internal class StarDate
    {
        private static StarDate now;
        private LocalTime startTime;

        public StarDate(LocalTime startTime)
        {
            this.startTime = startTime;
        }

        public static StarDate Now { get => StarDate.GetNow(); internal set => now = value; }

        private static StarDate GetNow()
        {

            Console.WriteLine("Hello, World!");
            DateTime dt = DateTime.Now;
            Console.WriteLine(dt.ToString());

            var tz = DateTimeZoneProviders.Tzdb.GetSystemDefault();
            var clock = SystemClock.Instance;

            // Snapshot "now"
            Instant start = clock.GetCurrentInstant();
            ZonedDateTime startLocal = start.InZone(tz);

            // Keep the wall-clock time constant
            LocalDate startDate = startLocal.Date;
            LocalTime startTime = startLocal.TimeOfDay;

            StarDate sd = new StarDate(startTime);

            return sd;

            throw new NotImplementedException();
        }
    }
}