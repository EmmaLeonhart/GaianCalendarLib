using NodaTime;
using NodaTime.Calendars;
using System.Numerics;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;
using NodaTime.Text;
using System.Globalization;
using System.Runtime.CompilerServices;
using System;
using System.ComponentModel.Design;

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


        //putting these at the front 

        public bool Equals(GaianLocalDate other) => _date.Equals(other._date);
        public override bool Equals(object? obj) => obj is GaianLocalDate g && Equals(g);
        public override int GetHashCode() => _date.GetHashCode();

        public int CompareTo(GaianLocalDate other) => _date.CompareTo(other._date);
        int IComparable.CompareTo(object? obj) =>
            obj is GaianLocalDate g ? CompareTo(g) : throw new ArgumentException("Not a GaianLocalDate.");

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
        public GaianLocalDate(int year, int month, int day)
        {
            // Validate day range (1-28 for all months)
            if (day < 1 || day > 28)
            {
                throw new ArgumentOutOfRangeException(nameof(day), "Day must be between 1 and 28");
            }

                // Check if this is a leap year (has 53 weeks, allowing month 14)
                int isoWeekYear = year - 10000;
            var weekYearRules = WeekYearRules.Iso;
            int weeksInYear = weekYearRules.GetWeeksInWeekYear(isoWeekYear);
            bool isLeapYear = weeksInYear == 53;
            int maxMonth = isLeapYear ? 14 : 13;

            

            // Validate month range
            if (month < 1 || month > maxMonth)
            {
                string leapInfo = isLeapYear ? " (" + year + " is a leap year with 14 months)" : " (" + year + " is a regular year with 13 months)";
                throw new ArgumentOutOfRangeException(nameof(month), 
                    $"Month must be between 1 and {maxMonth}{leapInfo}");
            }

            if (month == 14 && day > 7) {
                throw new ArgumentOutOfRangeException("Leap month (14) only has 7 days");
            }

            try
            {
                // Convert Gaian components back to ISO week-based date
                int weekOfYear = ((month - 1) * 4) + ((day - 1) / 7) + 1;
                int dayOfWeek = ((day - 1) % 7) + 1;
                
                // Use NodaTime's week-year construction
                _date = weekYearRules.GetLocalDate(isoWeekYear, weekOfYear, (IsoDayOfWeek)dayOfWeek);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new ArgumentOutOfRangeException($"Invalid Gaian date: {year}/{month}/{day}. " +
                    $"This combination does not correspond to a valid date in the underlying calendar.", ex);
            }
        }
        public GaianLocalDate(int year, int month, int day, CalendarSystem calendar) => throw new NotImplementedException();

        // --- Properties (mirror) ---
        public CalendarSystem Calendar => _date.Calendar;
        public int Day => GaianTools.GetDay(_date);
        public IsoDayOfWeek DayOfWeek => GaianTools.GetDayOfWeek(_date);
        public int DayOfYear => GaianTools.GetDayOfYear(_date);
        public Era Era => throw new NotImplementedException();
        public static GaianLocalDate MaxIsoValue => new GaianLocalDate(new LocalDate(9999, 12, 31));
        public static GaianLocalDate MinIsoValue => new GaianLocalDate(new LocalDate(1, 1, 1));
        public GaianMonth Month => GaianTools.GetMonth(_date);
        public int Year => GaianTools.GetYear(_date);
        public static GaianLocalDate Today => new GaianLocalDate(SystemClock.Instance.GetCurrentInstant().InZone(DateTimeZoneProviders.Tzdb.GetSystemDefault()).Date);
        //I am not including  => throw new NotImplementedException();

        // --- Static methods (mirror) ---
        public static GaianLocalDate Add(GaianLocalDate date, Period period) => throw new NotImplementedException();
        public static XmlQualifiedName AddSchema(XmlSchemaSet xmlSchemaSet) => throw new NotImplementedException();
        public static GaianLocalDate FromDateOnly(DateOnly date)
        {
            return new GaianLocalDate(LocalDate.FromDateOnly(date));
            throw new NotImplementedException();
        }

        public static GaianLocalDate FromDateTime(DateTime dateTime)
        {
            return new GaianLocalDate(LocalDate.FromDateTime(dateTime));
        }

        public static GaianLocalDate Parse(string input)
        {
            if (TryParse(input, out var result))
                return result;
            throw new FormatException($"Unable to parse '{input}' as a Gaian date.");
        }

        public static bool TryParse(string input, out GaianLocalDate result)
        {
            result = default;
            if (GaianTools.TryParseGaianDate(input, out int year, out int month, out int day))
            {
                try
                {
                    result = new GaianLocalDate(year, month, day);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public static GaianLocalDate FromDateTime(DateTime dateTime, CalendarSystem calendar) => throw new NotImplementedException();
        public static GaianLocalDate FromWeekYearWeekAndDay(int weekYear, int weekOfWeekYear, IsoDayOfWeek dayOfWeek)
        {
            var weekYearRules = WeekYearRules.Iso;
            var localDate = weekYearRules.GetLocalDate(weekYear, weekOfWeekYear, dayOfWeek);
            return new GaianLocalDate(localDate);
        }
        public static GaianLocalDate FromYearMonthWeekAndDay(int year, int month, int occurrence, IsoDayOfWeek dayOfWeek) => throw new NotImplementedException();
        public static GaianLocalDate Max(GaianLocalDate x, GaianLocalDate y) => throw new NotImplementedException();
        public static GaianLocalDate Min(GaianLocalDate x, GaianLocalDate y) => throw new NotImplementedException();
        public static Period Subtract(GaianLocalDate lhs, GaianLocalDate rhs) => throw new NotImplementedException();
        public static GaianLocalDate Subtract(GaianLocalDate date, Period period) => throw new NotImplementedException();

        // --- Instance methods (mirror) ---
        public GaianLocalDateTime At(LocalTime time) => new GaianLocalDateTime(_date.At(time));
        public GaianLocalDateTime AtMidnight() => new GaianLocalDateTime(_date.AtMidnight());
        public ZonedDateTime AtStartOfDayInZone(DateTimeZone zone) => throw new NotImplementedException();

        //public int CompareTo(GaianLocalDate other) => throw new NotImplementedException();
        //int IComparable.CompareTo(object? obj) => throw new NotImplementedException();

        public void Deconstruct(out int year, out int month, out int day) => throw new NotImplementedException();
        public void Deconstruct(out int year, out int month, out int day, out CalendarSystem calendar) => throw new NotImplementedException();

        //public bool Equals(GaianLocalDate other) => throw new NotImplementedException();
        //public override bool Equals(object? obj) => throw new NotImplementedException();
        //public override int GetHashCode() => throw new NotImplementedException();

        public GaianLocalDate Minus(Period period) => throw new NotImplementedException();
        public Period Minus(GaianLocalDate date) => throw new NotImplementedException();

        public GaianLocalDate Next(IsoDayOfWeek targetDayOfWeek) => new GaianLocalDate(_date.Next(targetDayOfWeek));
        public GaianLocalDate Previous(IsoDayOfWeek targetDayOfWeek) => new GaianLocalDate(_date.Previous(targetDayOfWeek));

        public GaianLocalDate Plus(Period period) => throw new NotImplementedException();
        public GaianLocalDate PlusDays(int days) => new GaianLocalDate(_date.PlusDays(days));
        public GaianLocalDate PlusMonths(int months) => throw new NotImplementedException();
        public GaianLocalDate PlusWeeks(int weeks) => new GaianLocalDate(_date.PlusWeeks(weeks));
        public GaianLocalDate PlusYears(int years) => throw new NotImplementedException();

        public DateOnly ToDateOnly() => _date.ToDateOnly();
        public DateTime ToDateTimeUnspecified() => _date.ToDateTimeUnspecified();

        public double ToJulianDay() => GaianTools.ToJulianDay(_date);
        public static GaianLocalDate FromJulianDay(double julianDay) => new GaianLocalDate(GaianTools.FromJulianDay(julianDay));

        //public override string ToString() => throw new NotImplementedException();
        public string ToString(string? patternText, IFormatProvider? formatProvider)
        {
            if (patternText == null)
            {
                return ToString();
            }

            var culture = (formatProvider as CultureInfo) ?? CultureInfo.CurrentCulture;
            return GaianDateFormat.Format(_date, patternText, culture);
        }




        public override string ToString()
        {
            return GaianTools.GaiaDateString(_date);
        }


        public YearMonth ToYearMonth() => throw new NotImplementedException();
        public GaianLocalDate With(Func<GaianLocalDate, GaianLocalDate> adjuster) => throw new NotImplementedException();
        public GaianLocalDate WithCalendar(CalendarSystem calendar) => throw new NotImplementedException();
        public GaianOffsetDateTime WithOffset(Offset offset) => new GaianOffsetDateTime(_date.AtMidnight().WithOffset(offset));

        // --- Operators (mirror) ---
        public static LocalDateTime operator +(GaianLocalDate date, LocalTime time) => throw new NotImplementedException();
        public static GaianLocalDate operator +(GaianLocalDate date, Period period) => throw new NotImplementedException();
        // Equality / inequality
        public static bool operator ==(GaianLocalDate lhs, GaianLocalDate rhs) => lhs._date == rhs._date;
        public static bool operator !=(GaianLocalDate lhs, GaianLocalDate rhs) => lhs._date != rhs._date;

        // Ordering (use LocalDate's comparison)
        public static bool operator <(GaianLocalDate lhs, GaianLocalDate rhs) => lhs._date < rhs._date;
        public static bool operator <=(GaianLocalDate lhs, GaianLocalDate rhs) => lhs._date <= rhs._date;
        public static bool operator >(GaianLocalDate lhs, GaianLocalDate rhs) => lhs._date > rhs._date;
        public static bool operator >=(GaianLocalDate lhs, GaianLocalDate rhs) => lhs._date >= rhs._date;
        public static Period operator -(GaianLocalDate lhs, GaianLocalDate rhs) => throw new NotImplementedException();
        public static GaianLocalDate operator -(GaianLocalDate date, Period period) => throw new NotImplementedException();






        // --- XML serialization (explicit, mirror) ---
        XmlSchema? IXmlSerializable.GetSchema() => throw new NotImplementedException();
        void IXmlSerializable.ReadXml(XmlReader reader) => throw new NotImplementedException();
        void IXmlSerializable.WriteXml(XmlWriter writer) => throw new NotImplementedException();

    }
}