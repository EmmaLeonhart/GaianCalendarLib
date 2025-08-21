using NodaTime;

namespace DateTimeNodaTimeExperiments
{
    public struct LocalStarDate
    {

        //this operates as a simple wrapper for a LocalDate in NodaTime
        private LocalDate date;

        public LocalStarDate(LocalDate date)
        {
            this.date = date;
        }

        public override string ToString() {
            return GaianTools.GaiaDateString(date);
        }
    }
}