using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Numerics;       // for generic operator interfaces
using System.Xml;
using System.Xml.Schema;
using NodaTime;
using NodaTime.Calendars;
using System.Xml.Serialization;
using NodaTime.Text;
using System.Globalization;

namespace Gaian
{
    /// <summary>
    /// Gaian wrapper mirroring NodaTime.OffsetDateTime (3.2.x).
    /// </summary>
    public readonly struct GaianOffsetDateTime :
        IEquatable<GaianOffsetDateTime>,
        IFormattable,
        IXmlSerializable,
        IAdditionOperators<GaianOffsetDateTime, Duration, GaianOffsetDateTime>,
        ISubtractionOperators<GaianOffsetDateTime, Duration, GaianOffsetDateTime>,
        ISubtractionOperators<GaianOffsetDateTime, GaianOffsetDateTime, Duration>,
        IEqualityOperators<GaianOffsetDateTime, GaianOffsetDateTime, bool>
    {
        private readonly OffsetDateTime _odt;
        //private  LocalDate nodaDate1;

        // ===== Constructor (mirrors: OffsetDateTime(_ldt, Offset)) =====
        public GaianOffsetDateTime(GaianLocalDateTime localDateTime, Offset offset)
            => throw new NotImplementedException();

        public GaianOffsetDateTime(OffsetDateTime odt)
        {
            this._odt = odt;
        }


        // --- Properties taken from LocalDate ---
        public CalendarSystem Calendar => throw new NotImplementedException();
        public int Day => GaianTools.GetDay(nodaDate);
        public IsoDayOfWeek DayOfWeek => GaianTools.GetDayOfWeek(nodaDate);
        public int DayOfYear => GaianTools.GetDayOfYear(nodaDate);
        public Era Era => throw new NotImplementedException();
        public static GaianLocalDate MaxIsoValue => throw new NotImplementedException();
        public static GaianLocalDate MinIsoValue => throw new NotImplementedException();
        public GaianMonth Month => GaianTools.GetMonth(nodaDate);
        public int Year => GaianTools.GetYear(nodaDate);
        //I am not including  => throw new NotImplementedException();


        // ===== Properties (mapped 1:1; Local types adapted to Gaian) =====
        //public CalendarSystem Calendar => throw new NotImplementedException();           // docs: Calendar
        public int ClockHourOfHalfDay => throw new NotImplementedException();            // docs: ClockHourOfHalfDay
        public GaianLocalDate Date => throw new NotImplementedException();               // docs: nodaDate (adapted)
        //public int Day => throw new NotImplementedException();                           // docs: Day
        //public IsoDayOfWeek DayOfWeek => throw new NotImplementedException();            // docs: DayOfWeek
        //public int DayOfYear => throw new NotImplementedException();                     // docs: DayOfYear
        //public Era Era => throw new NotImplementedException();                           // docs: Era
        public int Hour => throw new NotImplementedException();                          // docs: Hour
        public GaianLocalDateTime LocalDateTime => throw new NotImplementedException();  // docs: _ldt (adapted)
        public int Millisecond => throw new NotImplementedException();                   // docs: Millisecond
        public int Minute => throw new NotImplementedException();                        // docs: Minute
        //public int Month => throw new NotImplementedException();                         // docs: Month
        public long NanosecondOfDay => throw new NotImplementedException();              // docs: NanosecondOfDay
        public int NanosecondOfSecond => throw new NotImplementedException();            // docs: NanosecondOfSecond
        public Offset Offset => this._odt.Offset;                    // docs: Offset
        public int Second => throw new NotImplementedException();                        // docs: Second
        public long TickOfDay => throw new NotImplementedException();                    // docs: TickOfDay
        public int TickOfSecond => throw new NotImplementedException();                  // docs: TickOfSecond
        public LocalTime TimeOfDay => throw new NotImplementedException();               // docs: TimeOfDay
        //public int Year => throw new NotImplementedException();                          // docs: Year
        ////I am not including  => throw new NotImplementedException();                     // docs: YearOfEra

        public LocalDate nodaDate { get => _odt.Date;  }

        // ===== Static methods =====
        public static GaianOffsetDateTime Add(GaianOffsetDateTime value, Duration duration)
            => throw new NotImplementedException();                                      // docs: Add(OffsetDateTime, Duration)

        public static XmlQualifiedName AddSchema(XmlSchemaSet xmlSchemaSet)
            => throw new NotImplementedException();                                      // docs: AddSchema (added in 3.2)

        public static GaianOffsetDateTime Subtract(GaianOffsetDateTime value, Duration duration)
            => throw new NotImplementedException();                                      // docs: Subtract(OffsetDateTime, Duration)

        public static Duration Subtract(GaianOffsetDateTime end, GaianOffsetDateTime start)
            => throw new NotImplementedException();                                      // docs: Subtract(OffsetDateTime, OffsetDateTime)

        // ===== Instance methods =====
        public bool Equals(GaianOffsetDateTime other)
            => throw new NotImplementedException();

        public override bool Equals(object? obj)
            => throw new NotImplementedException();

        public override int GetHashCode()
            => throw new NotImplementedException();

        public GaianOffsetDateTime Plus(Duration duration)
            => throw new NotImplementedException();                                      // docs: Plus(Duration)

        public GaianOffsetDateTime PlusHours(int hours)
            => throw new NotImplementedException();                                      // docs: PlusHours(int)

        public GaianOffsetDateTime PlusMinutes(int minutes)
            => throw new NotImplementedException();                                      // docs: PlusMinutes(int)

        public GaianOffsetDateTime PlusSeconds(long seconds)
            => throw new NotImplementedException();                                      // docs: PlusSeconds(long)

        public GaianOffsetDateTime PlusMilliseconds(long milliseconds)
            => throw new NotImplementedException();                                      // docs: PlusMilliseconds(long)

        public GaianOffsetDateTime PlusTicks(long ticks)
            => throw new NotImplementedException();                                      // docs: PlusTicks(long)

        public GaianOffsetDateTime PlusNanoseconds(long nanoseconds)
            => throw new NotImplementedException();                                      // docs: PlusNanoseconds(long)

        public GaianOffsetDateTime Minus(Duration duration)
            => throw new NotImplementedException();                                      // symmetry

        public Duration Minus(GaianOffsetDateTime other)
            => throw new NotImplementedException();                                      // docs: Minus(OffsetDateTime)

        public DateTimeOffset ToDateTimeOffset()
            => throw new NotImplementedException();                                      // docs: ToDateTimeOffset()

        public Instant ToInstant()
            => throw new NotImplementedException();                                      // docs: ToInstant()

        public OffsetDate ToOffsetDate()
            => throw new NotImplementedException();                                      // docs: ToOffsetDate()

        public OffsetTime ToOffsetTime()
            => throw new NotImplementedException();                                      // docs: ToOffsetTime()

        public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

        public string ToString(string? patternText, IFormatProvider? formatProvider)
        {
            var culture = (formatProvider as CultureInfo) ?? CultureInfo.CurrentCulture;

            if (string.IsNullOrEmpty(patternText) ||
                string.Equals(patternText, "G", StringComparison.OrdinalIgnoreCase))
            {
                // Default: Gaian date + time + offset
                var gdate = new GaianLocalDate(_odt.Date).ToString(null, culture);
                var time = _odt.TimeOfDay.ToString("HH':'mm", culture);
                var off = _odt.Offset.ToString("g", culture); // "+HH:mm"
                return $"{gdate} {time} {off}";
            }

            // Need to supply a template OffsetDateTime for fields not present in the pattern
            var template = new LocalDateTime(2000, 1, 1, 0, 0).WithOffset(Offset.Zero);
            var pattern = OffsetDateTimePattern.Create(patternText, culture, template);
            return pattern.Format(_odt);
        }

        public GaianOffsetDateTime With(Func<LocalDate, LocalDate> dateAdjuster)
            => throw new NotImplementedException();                                      // docs: With(Func<LocalDate, LocalDate>)

        public GaianOffsetDateTime With(Func<LocalTime, LocalTime> timeAdjuster)
            => throw new NotImplementedException();                                      // docs: With(Func<LocalTime, LocalTime>)

        public GaianOffsetDateTime WithCalendar(CalendarSystem calendar)
            => throw new NotImplementedException();                                      // docs: WithCalendar(CalendarSystem)

        public GaianOffsetDateTime WithOffset(Offset offset)
            => throw new NotImplementedException();                                      // docs: WithOffset(Offset)

        // ===== Operators =====
        public static GaianOffsetDateTime operator +(GaianOffsetDateTime value, Duration duration)
            => throw new NotImplementedException();                                      // docs: operator +(OffsetDateTime, Duration)

        public static GaianOffsetDateTime operator -(GaianOffsetDateTime value, Duration duration)
            => throw new NotImplementedException();                                      // docs: operator -(OffsetDateTime, Duration)

        public static Duration operator -(GaianOffsetDateTime end, GaianOffsetDateTime start)
            => throw new NotImplementedException();                                      // docs: operator -(OffsetDateTime, OffsetDateTime)

        public static bool operator ==(GaianOffsetDateTime left, GaianOffsetDateTime right)
            => throw new NotImplementedException();                                      // docs: operator ==

        public static bool operator !=(GaianOffsetDateTime left, GaianOffsetDateTime right)
            => throw new NotImplementedException();                                      // docs: operator !=

        // ===== XML serialization (explicit) =====
        XmlSchema? IXmlSerializable.GetSchema()
            => throw new NotImplementedException();                                      // added in 3.2

        void IXmlSerializable.ReadXml(XmlReader reader)
            => throw new NotImplementedException();

        void IXmlSerializable.WriteXml(XmlWriter writer)
            => throw new NotImplementedException();

        // ===== Bridge helpers (optional, for your implementation) =====
        public static GaianOffsetDateTime FromNoda(OffsetDateTime odt)
            => throw new NotImplementedException();

        public OffsetDateTime ToNoda()
            => throw new NotImplementedException();
    }
}
