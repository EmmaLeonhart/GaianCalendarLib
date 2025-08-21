using NodaTime;
using NodaTime.Calendars;
using System.Numerics;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;
using NodaTime.Text;
using System.Globalization;

namespace Gaian
{
    public struct GaianLocalDate :
        IEquatable<GaianLocalDate>,
        IComparable<GaianLocalDate>,
        IComparable,
        IFormattable,
        IXmlSerializable,
        // C# 11 generic operators mirrored for parity
        IAdditionOperators<GaianLocalDate, Period, GaianLocalDate>,
        IAdditionOperators<GaianLocalDate, LocalTime, LocalDateTime>,
        ISubtractionOperators<GaianLocalDate, Period, GaianLocalDate>,
        ISubtractionOperators<GaianLocalDate, GaianLocalDate, Period>,
        IComparisonOperators<GaianLocalDate, GaianLocalDate, bool>,
        IEqualityOperators<GaianLocalDate, GaianLocalDate, bool>
    {

        //this operates as a simple wrapper for a LocalDate in NodaTime
        private readonly LocalDate _date;

        public GaianLocalDate(LocalDate date) => _date = date;

        // Expose underlying date if needed
        public LocalDate Value => _date;

        // Implicit: NodaTime.LocalDate → GaianLocalDate
        public static implicit operator GaianLocalDate(LocalDate d) => new GaianLocalDate(d);

        // Implicit: GaianLocalDate → NodaTime.LocalDate
        public static implicit operator LocalDate(GaianLocalDate g) => g._date;

        // Example equality override
        //public override string ToString() => _date.ToString();



        public GaianLocalDate(Era era, int yearOfEra, int month, int day) => throw new NotImplementedException();
        public GaianLocalDate(Era era, int yearOfEra, int month, int day, CalendarSystem calendar) => throw new NotImplementedException();
        public GaianLocalDate(int year, int month, int day) => throw new NotImplementedException();
        public GaianLocalDate(int year, int month, int day, CalendarSystem calendar) => throw new NotImplementedException();

        // --- Properties (mirror) ---
        public CalendarSystem Calendar => throw new NotImplementedException();
        public int Day => GaianTools.GetDay(_date);
        public IsoDayOfWeek DayOfWeek => GaianTools.GetDayOfWeek(_date);
        public int DayOfYear => GaianTools.GetDayOfYear(_date);
        public Era Era => throw new NotImplementedException();
        public static GaianLocalDate MaxIsoValue => throw new NotImplementedException();
        public static GaianLocalDate MinIsoValue => throw new NotImplementedException();
        public int GaianMonth => GaianTools.GetMonth(_date);
        public int Year => GaianTools.GetYear(_date);
        public int YearOfEra => throw new NotImplementedException();

        // --- Static methods (mirror) ---
        public static GaianLocalDate Add(GaianLocalDate date, Period period) => throw new NotImplementedException();
        public static XmlQualifiedName AddSchema(XmlSchemaSet xmlSchemaSet) => throw new NotImplementedException();
        public static GaianLocalDate FromDateOnly(DateOnly date) => throw new NotImplementedException();
        public static GaianLocalDate FromDateTime(DateTime dateTime) => throw new NotImplementedException();
        public static GaianLocalDate FromDateTime(DateTime dateTime, CalendarSystem calendar) => throw new NotImplementedException();
        public static GaianLocalDate FromWeekYearWeekAndDay(int weekYear, int weekOfWeekYear, IsoDayOfWeek dayOfWeek) => throw new NotImplementedException();
        public static GaianLocalDate FromYearMonthWeekAndDay(int year, int month, int occurrence, IsoDayOfWeek dayOfWeek) => throw new NotImplementedException();
        public static GaianLocalDate Max(GaianLocalDate x, GaianLocalDate y) => throw new NotImplementedException();
        public static GaianLocalDate Min(GaianLocalDate x, GaianLocalDate y) => throw new NotImplementedException();
        public static Period Subtract(GaianLocalDate lhs, GaianLocalDate rhs) => throw new NotImplementedException();
        public static GaianLocalDate Subtract(GaianLocalDate date, Period period) => throw new NotImplementedException();

        // --- Instance methods (mirror) ---
        public LocalDateTime At(LocalTime time) => throw new NotImplementedException();
        public LocalDateTime AtMidnight() => throw new NotImplementedException();
        public ZonedDateTime AtStartOfDayInZone(DateTimeZone zone) => throw new NotImplementedException();

        public int CompareTo(GaianLocalDate other) => throw new NotImplementedException();
        int IComparable.CompareTo(object? obj) => throw new NotImplementedException();

        public void Deconstruct(out int year, out int month, out int day) => throw new NotImplementedException();
        public void Deconstruct(out int year, out int month, out int day, out CalendarSystem calendar) => throw new NotImplementedException();

        public bool Equals(GaianLocalDate other) => throw new NotImplementedException();
        public override bool Equals(object? obj) => throw new NotImplementedException();
        public override int GetHashCode() => throw new NotImplementedException();

        public GaianLocalDate Minus(Period period) => throw new NotImplementedException();
        public Period Minus(GaianLocalDate date) => throw new NotImplementedException();

        public GaianLocalDate Next(IsoDayOfWeek targetDayOfWeek) => throw new NotImplementedException();
        public GaianLocalDate Previous(IsoDayOfWeek targetDayOfWeek) => throw new NotImplementedException();

        public GaianLocalDate Plus(Period period) => throw new NotImplementedException();
        public GaianLocalDate PlusDays(int days) => throw new NotImplementedException();
        public GaianLocalDate PlusMonths(int months) => throw new NotImplementedException();
        public GaianLocalDate PlusWeeks(int weeks) => throw new NotImplementedException();
        public GaianLocalDate PlusYears(int years) => throw new NotImplementedException();

        public DateOnly ToDateOnly() => throw new NotImplementedException();
        public DateTime ToDateTimeUnspecified() => throw new NotImplementedException();

        //public override string ToString() => throw new NotImplementedException();
        public string ToString(string? patternText, IFormatProvider? formatProvider)
        {
            if (patternText == null)
            {
                // Default formatting
                return ToString();
            }

            var culture = (formatProvider as CultureInfo) ?? CultureInfo.CurrentCulture;

            // For example, if this is a LocalDateTime-like struct:
            var pattern = LocalDateTimePattern.Create(patternText, culture);
            throw new NotImplementedException();
            //return pattern.Format(_date);
        }




        public override string ToString()
        {
            return GaianTools.GaiaDateString(_date);
        }


        public YearMonth ToYearMonth() => throw new NotImplementedException();
        public GaianLocalDate With(Func<GaianLocalDate, GaianLocalDate> adjuster) => throw new NotImplementedException();
        public GaianLocalDate WithCalendar(CalendarSystem calendar) => throw new NotImplementedException();
        public OffsetDate WithOffset(Offset offset) => throw new NotImplementedException();

        // --- Operators (mirror) ---
        public static LocalDateTime operator +(GaianLocalDate date, LocalTime time) => throw new NotImplementedException();
        public static GaianLocalDate operator +(GaianLocalDate date, Period period) => throw new NotImplementedException();
        public static bool operator ==(GaianLocalDate lhs, GaianLocalDate rhs) => throw new NotImplementedException();
        public static bool operator !=(GaianLocalDate lhs, GaianLocalDate rhs) => throw new NotImplementedException();
        public static bool operator <(GaianLocalDate lhs, GaianLocalDate rhs) => throw new NotImplementedException();
        public static bool operator <=(GaianLocalDate lhs, GaianLocalDate rhs) => throw new NotImplementedException();
        public static bool operator >(GaianLocalDate lhs, GaianLocalDate rhs) => throw new NotImplementedException();
        public static bool operator >=(GaianLocalDate lhs, GaianLocalDate rhs) => throw new NotImplementedException();
        public static Period operator -(GaianLocalDate lhs, GaianLocalDate rhs) => throw new NotImplementedException();
        public static GaianLocalDate operator -(GaianLocalDate date, Period period) => throw new NotImplementedException();

        // --- XML serialization (explicit, mirror) ---
        XmlSchema? IXmlSerializable.GetSchema() => throw new NotImplementedException();
        void IXmlSerializable.ReadXml(XmlReader reader) => throw new NotImplementedException();
        void IXmlSerializable.WriteXml(XmlWriter writer) => throw new NotImplementedException();

    }
}