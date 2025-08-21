using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Numerics;               // for generic math operator interfaces
using NodaTime;
using NodaTime.Calendars;
using NodaTime.TimeZones;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using NodaTime.Text;
using System.Globalization;

namespace Gaian
{
    /// <summary>
    /// Gaian-local nodaDate and time wrapper mirroring NodaTime._ldt (3.2.x).
    /// </summary>
    public readonly struct GaianLocalDateTime :
        IEquatable<GaianLocalDateTime>,
        IComparable<GaianLocalDateTime>,
        IComparable,
        IFormattable,
        IXmlSerializable,
        IAdditionOperators<GaianLocalDateTime, Period, GaianLocalDateTime>,
        ISubtractionOperators<GaianLocalDateTime, GaianLocalDateTime, Period>,
        ISubtractionOperators<GaianLocalDateTime, Period, GaianLocalDateTime>,
        IComparisonOperators<GaianLocalDateTime, GaianLocalDateTime, bool>,
        IEqualityOperators<GaianLocalDateTime, GaianLocalDateTime, bool>
    {
        private readonly LocalDateTime _ldt;

        public LocalDateTime Value => _ldt;

        // ===== Constructors (mirror) =====
        public GaianLocalDateTime(int year, int month, int day, int hour, int minute)
        {
            var gaianDate = new GaianLocalDate(year, month, day);
            var time = new LocalTime(hour, minute);
            _ldt = gaianDate.Value.At(time);
        }

        public GaianLocalDateTime(int year, int month, int day, int hour, int minute, CalendarSystem calendar)
            => throw new NotImplementedException();

        public GaianLocalDateTime(int year, int month, int day, int hour, int minute, int second)
        {
            var gaianDate = new GaianLocalDate(year, month, day);
            var time = new LocalTime(hour, minute, second);
            _ldt = gaianDate.Value.At(time);
        }

        public GaianLocalDateTime(int year, int month, int day, int hour, int minute, int second, CalendarSystem calendar)
            => throw new NotImplementedException();

        public GaianLocalDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            var gaianDate = new GaianLocalDate(year, month, day);
            var time = new LocalTime(hour, minute, second, millisecond);
            _ldt = gaianDate.Value.At(time);
        }

        public GaianLocalDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, CalendarSystem calendar)
            => throw new NotImplementedException();

        public GaianLocalDateTime(LocalDateTime today)
        {
            this._ldt = today;
        }




        public CalendarSystem Calendar => throw new NotImplementedException();
        public int Day => GaianTools.GetDay(nodaDate);
        public IsoDayOfWeek DayOfWeek => GaianTools.GetDayOfWeek(nodaDate);
        public int DayOfYear => GaianTools.GetDayOfYear(nodaDate);
        public Era Era => throw new NotImplementedException();
        public static GaianLocalDate MaxIsoValue => throw new NotImplementedException();
        public static GaianLocalDate MinIsoValue => throw new NotImplementedException();
        public GaianMonth Month => GaianTools.GetMonth(nodaDate);
        public int Year => GaianTools.GetYear(nodaDate);



        public int ClockHourOfHalfDay => throw new NotImplementedException();
        public LocalDate nodaDate => this._ldt.Date;
        public GaianLocalDate Date => new GaianLocalDate(nodaDate);
        public int Hour => _ldt.Hour;

        public int Millisecond => _ldt.Millisecond;
        public int Minute => _ldt.Minute;
        public long NanosecondOfDay => _ldt.NanosecondOfDay;
        public int NanosecondOfSecond => _ldt.NanosecondOfSecond;
        public int Second => _ldt.Second;
        public long TickOfDay => _ldt.TickOfDay;
        public int TickOfSecond => _ldt.TickOfSecond;
        public LocalTime TimeOfDay => _ldt.TimeOfDay;

        // ===== Static methods (mirror) =====
        public static GaianLocalDateTime Add(GaianLocalDateTime localDateTime, Period period)
            => throw new NotImplementedException();

        public static XmlQualifiedName AddSchema(XmlSchemaSet xmlSchemaSet)
            => throw new NotImplementedException();

        public static GaianLocalDateTime FromDateTime(DateTime dateTime)
        {
            return new GaianLocalDateTime(LocalDateTime.FromDateTime(dateTime));    
            throw new NotImplementedException();
        }

        public static GaianLocalDateTime FromDateTime(DateTime dateTime, CalendarSystem calendar)
            => throw new NotImplementedException();

        public static GaianLocalDateTime Max(GaianLocalDateTime x, GaianLocalDateTime y)
            => new GaianLocalDateTime(LocalDateTime.Max(x._ldt, y._ldt));

        public static GaianLocalDateTime Min(GaianLocalDateTime x, GaianLocalDateTime y)
            => new GaianLocalDateTime(LocalDateTime.Min(x._ldt, y._ldt));

        public static Period Subtract(GaianLocalDateTime lhs, GaianLocalDateTime rhs)
            => throw new NotImplementedException();

        public static GaianLocalDateTime Subtract(GaianLocalDateTime localDateTime, Period period)
            => throw new NotImplementedException();

        // ===== Instance methods (mirror) =====
        public bool Equals(GaianLocalDateTime other)
            => _ldt.Equals(other._ldt);

        public override bool Equals(object? obj)
            => obj is GaianLocalDateTime other && Equals(other);

        public override int GetHashCode()
            => _ldt.GetHashCode();

        public GaianZonedDateTime InUtc()
            => new GaianZonedDateTime(_ldt.InUtc());

        public GaianZonedDateTime InZone(DateTimeZone zone, ZoneLocalMappingResolver resolver)
            => new GaianZonedDateTime(_ldt.InZone(zone, resolver));

        public GaianZonedDateTime InZoneLeniently(DateTimeZone zone)
            => new GaianZonedDateTime(_ldt.InZoneLeniently(zone));

        public GaianZonedDateTime InZoneStrictly(DateTimeZone zone)
            => new GaianZonedDateTime(_ldt.InZoneStrictly(zone));

        public GaianLocalDateTime Minus(Period period)
            => throw new NotImplementedException();

        public Period Minus(GaianLocalDateTime localDateTime)
            => throw new NotImplementedException();

        public GaianLocalDateTime Next(IsoDayOfWeek targetDayOfWeek)
            => throw new NotImplementedException();

        public GaianLocalDateTime Previous(IsoDayOfWeek targetDayOfWeek)
            => throw new NotImplementedException();

        public GaianLocalDateTime Plus(Period period)
            => throw new NotImplementedException();

        public GaianLocalDateTime PlusDays(int days)
            => new GaianLocalDateTime(_ldt.PlusDays(days));

        public GaianLocalDateTime PlusHours(long hours)
            => new GaianLocalDateTime(_ldt.PlusHours(hours));

        public GaianLocalDateTime PlusMilliseconds(long milliseconds)
            => new GaianLocalDateTime(_ldt.PlusMilliseconds(milliseconds));

        public GaianLocalDateTime PlusMinutes(long minutes)
            => new GaianLocalDateTime(_ldt.PlusMinutes(minutes));

        public GaianLocalDateTime PlusMonths(int months)
            => throw new NotImplementedException();

        public GaianLocalDateTime PlusNanoseconds(long nanoseconds)
            => new GaianLocalDateTime(_ldt.PlusNanoseconds(nanoseconds));

        public GaianLocalDateTime PlusSeconds(long seconds)
            => new GaianLocalDateTime(_ldt.PlusSeconds(seconds));

        public GaianLocalDateTime PlusTicks(long ticks)
            => new GaianLocalDateTime(_ldt.PlusTicks(ticks));

        public GaianLocalDateTime PlusWeeks(int weeks)
            => new GaianLocalDateTime(_ldt.PlusWeeks(weeks));

        public GaianLocalDateTime PlusYears(int years)
            => throw new NotImplementedException();

        public DateTime ToDateTimeUnspecified()
            => _ldt.ToDateTimeUnspecified();

        public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

        public string ToString(string? patternText, IFormatProvider? formatProvider)
        {
            var culture = (formatProvider as CultureInfo) ?? CultureInfo.CurrentCulture;

            if (string.IsNullOrEmpty(patternText) || string.Equals(patternText, "G", StringComparison.OrdinalIgnoreCase))
            {
                var gdate = new GaianLocalDate(_ldt.Date);
                string timeText = _ldt.TimeOfDay.ToString("HH':'mm", culture);
                return $"{gdate.ToString(null, culture)} {timeText}";
            }
            var pattern = LocalDateTimePattern.Create(patternText, culture);
            return pattern.Format(_ldt);
        }

        public GaianLocalDateTime With(Func<LocalDate, LocalDate> adjuster)
            => throw new NotImplementedException();

        public GaianLocalDateTime With(Func<LocalTime, LocalTime> adjuster)
            => throw new NotImplementedException();

        public GaianLocalDateTime WithCalendar(CalendarSystem calendar)
            => throw new NotImplementedException();

        public OffsetDateTime WithOffset(Offset offset)
            => throw new NotImplementedException();

        // ===== Comparison (mirror) =====
        public int CompareTo(GaianLocalDateTime other)
            => _ldt.CompareTo(other._ldt);

        int IComparable.CompareTo(object? obj)
            => obj is GaianLocalDateTime other ? CompareTo(other) : throw new ArgumentException("Not a GaianLocalDateTime.");

        // ===== Operators (mirror) =====
        public static GaianLocalDateTime operator +(GaianLocalDateTime localDateTime, Period period)
            => throw new NotImplementedException();

        public static bool operator ==(GaianLocalDateTime left, GaianLocalDateTime right)
            => left._ldt == right._ldt;

        public static bool operator !=(GaianLocalDateTime left, GaianLocalDateTime right)
            => left._ldt != right._ldt;

        public static bool operator >(GaianLocalDateTime lhs, GaianLocalDateTime rhs)
            => lhs._ldt > rhs._ldt;

        public static bool operator >=(GaianLocalDateTime lhs, GaianLocalDateTime rhs)
            => lhs._ldt >= rhs._ldt;

        public static bool operator <(GaianLocalDateTime lhs, GaianLocalDateTime rhs)
            => lhs._ldt < rhs._ldt;

        public static bool operator <=(GaianLocalDateTime lhs, GaianLocalDateTime rhs)
            => lhs._ldt <= rhs._ldt;

        public static Period operator -(GaianLocalDateTime lhs, GaianLocalDateTime rhs)
            => throw new NotImplementedException();

        public static GaianLocalDateTime operator -(GaianLocalDateTime localDateTime, Period period)
            => throw new NotImplementedException();

        // ===== XML serialization (explicit) =====
        XmlSchema? IXmlSerializable.GetSchema()
            => throw new NotImplementedException();

        void IXmlSerializable.ReadXml(XmlReader reader)
            => throw new NotImplementedException();

        void IXmlSerializable.WriteXml(XmlWriter writer)
            => throw new NotImplementedException();
    }
}
