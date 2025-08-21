using NodaTime;
using NodaTime.Calendars;
using System.Numerics;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;

namespace DateTimeNodaTimeExperiments
{
    public struct LocalGaianDate :
        IEquatable<LocalGaianDate>,
        IComparable<LocalGaianDate>,
        IComparable,
        IFormattable,
        IXmlSerializable,
        // C# 11 generic operators mirrored for parity
        IAdditionOperators<LocalGaianDate, Period, LocalGaianDate>,
        IAdditionOperators<LocalGaianDate, LocalTime, LocalDateTime>,
        ISubtractionOperators<LocalGaianDate, Period, LocalGaianDate>,
        ISubtractionOperators<LocalGaianDate, LocalGaianDate, Period>,
        IComparisonOperators<LocalGaianDate, LocalGaianDate, bool>,
        IEqualityOperators<LocalGaianDate, LocalGaianDate, bool>
    {

        //this operates as a simple wrapper for a LocalDate in NodaTime
        private LocalDate date;

        public LocalGaianDate(LocalDate date)
        {
            this.date = date;
        }

        public override string ToString() {
            return GaianTools.GaiaDateString(date);
        }


        public LocalGaianDate(Era era, int yearOfEra, int month, int day) => throw new NotImplementedException();
        public LocalGaianDate(Era era, int yearOfEra, int month, int day, CalendarSystem calendar) => throw new NotImplementedException();
        public LocalGaianDate(int year, int month, int day) => throw new NotImplementedException();
        public LocalGaianDate(int year, int month, int day, CalendarSystem calendar) => throw new NotImplementedException();

        // --- Properties (mirror) ---
        public CalendarSystem Calendar => throw new NotImplementedException();
        public int Day => throw new NotImplementedException();
        public IsoDayOfWeek DayOfWeek => throw new NotImplementedException();
        public int DayOfYear => throw new NotImplementedException();
        public Era Era => throw new NotImplementedException();
        public static LocalGaianDate MaxIsoValue => throw new NotImplementedException();
        public static LocalGaianDate MinIsoValue => throw new NotImplementedException();
        public int Month => throw new NotImplementedException();
        public int Year => throw new NotImplementedException();
        public int YearOfEra => throw new NotImplementedException();

        // --- Static methods (mirror) ---
        public static LocalGaianDate Add(LocalGaianDate date, Period period) => throw new NotImplementedException();
        public static XmlQualifiedName AddSchema(XmlSchemaSet xmlSchemaSet) => throw new NotImplementedException();
        public static LocalGaianDate FromDateOnly(DateOnly date) => throw new NotImplementedException();
        public static LocalGaianDate FromDateTime(DateTime dateTime) => throw new NotImplementedException();
        public static LocalGaianDate FromDateTime(DateTime dateTime, CalendarSystem calendar) => throw new NotImplementedException();
        public static LocalGaianDate FromWeekYearWeekAndDay(int weekYear, int weekOfWeekYear, IsoDayOfWeek dayOfWeek) => throw new NotImplementedException();
        public static LocalGaianDate FromYearMonthWeekAndDay(int year, int month, int occurrence, IsoDayOfWeek dayOfWeek) => throw new NotImplementedException();
        public static LocalGaianDate Max(LocalGaianDate x, LocalGaianDate y) => throw new NotImplementedException();
        public static LocalGaianDate Min(LocalGaianDate x, LocalGaianDate y) => throw new NotImplementedException();
        public static Period Subtract(LocalGaianDate lhs, LocalGaianDate rhs) => throw new NotImplementedException();
        public static LocalGaianDate Subtract(LocalGaianDate date, Period period) => throw new NotImplementedException();

        // --- Instance methods (mirror) ---
        public LocalDateTime At(LocalTime time) => throw new NotImplementedException();
        public LocalDateTime AtMidnight() => throw new NotImplementedException();
        public ZonedDateTime AtStartOfDayInZone(DateTimeZone zone) => throw new NotImplementedException();

        public int CompareTo(LocalGaianDate other) => throw new NotImplementedException();
        int IComparable.CompareTo(object? obj) => throw new NotImplementedException();

        public void Deconstruct(out int year, out int month, out int day) => throw new NotImplementedException();
        public void Deconstruct(out int year, out int month, out int day, out CalendarSystem calendar) => throw new NotImplementedException();

        public bool Equals(LocalGaianDate other) => throw new NotImplementedException();
        public override bool Equals(object? obj) => throw new NotImplementedException();
        public override int GetHashCode() => throw new NotImplementedException();

        public LocalGaianDate Minus(Period period) => throw new NotImplementedException();
        public Period Minus(LocalGaianDate date) => throw new NotImplementedException();

        public LocalGaianDate Next(IsoDayOfWeek targetDayOfWeek) => throw new NotImplementedException();
        public LocalGaianDate Previous(IsoDayOfWeek targetDayOfWeek) => throw new NotImplementedException();

        public LocalGaianDate Plus(Period period) => throw new NotImplementedException();
        public LocalGaianDate PlusDays(int days) => throw new NotImplementedException();
        public LocalGaianDate PlusMonths(int months) => throw new NotImplementedException();
        public LocalGaianDate PlusWeeks(int weeks) => throw new NotImplementedException();
        public LocalGaianDate PlusYears(int years) => throw new NotImplementedException();

        public DateOnly ToDateOnly() => throw new NotImplementedException();
        public DateTime ToDateTimeUnspecified() => throw new NotImplementedException();

        //public override string ToString() => throw new NotImplementedException();
        public string ToString(string? patternText, IFormatProvider? formatProvider) => throw new NotImplementedException();

        public YearMonth ToYearMonth() => throw new NotImplementedException();
        public LocalGaianDate With(Func<LocalGaianDate, LocalGaianDate> adjuster) => throw new NotImplementedException();
        public LocalGaianDate WithCalendar(CalendarSystem calendar) => throw new NotImplementedException();
        public OffsetDate WithOffset(Offset offset) => throw new NotImplementedException();

        // --- Operators (mirror) ---
        public static LocalDateTime operator +(LocalGaianDate date, LocalTime time) => throw new NotImplementedException();
        public static LocalGaianDate operator +(LocalGaianDate date, Period period) => throw new NotImplementedException();
        public static bool operator ==(LocalGaianDate lhs, LocalGaianDate rhs) => throw new NotImplementedException();
        public static bool operator !=(LocalGaianDate lhs, LocalGaianDate rhs) => throw new NotImplementedException();
        public static bool operator <(LocalGaianDate lhs, LocalGaianDate rhs) => throw new NotImplementedException();
        public static bool operator <=(LocalGaianDate lhs, LocalGaianDate rhs) => throw new NotImplementedException();
        public static bool operator >(LocalGaianDate lhs, LocalGaianDate rhs) => throw new NotImplementedException();
        public static bool operator >=(LocalGaianDate lhs, LocalGaianDate rhs) => throw new NotImplementedException();
        public static Period operator -(LocalGaianDate lhs, LocalGaianDate rhs) => throw new NotImplementedException();
        public static LocalGaianDate operator -(LocalGaianDate date, Period period) => throw new NotImplementedException();

        // --- XML serialization (explicit, mirror) ---
        XmlSchema? IXmlSerializable.GetSchema() => throw new NotImplementedException();
        void IXmlSerializable.ReadXml(XmlReader reader) => throw new NotImplementedException();
        void IXmlSerializable.WriteXml(XmlWriter writer) => throw new NotImplementedException();

    }
}