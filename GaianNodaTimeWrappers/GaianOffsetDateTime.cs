using System;
using System.Globalization;
using System.Numerics;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using NodaTime;
using NodaTime.Calendars;
using NodaTime.Text;

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

        private OffsetDateTime Value => _odt;

        public static implicit operator GaianOffsetDateTime(OffsetDateTime odt) => new GaianOffsetDateTime(odt);
        public static implicit operator OffsetDateTime(GaianOffsetDateTime godt) => godt._odt;

        // ===== Constructor (mirrors: OffsetDateTime(_ldt, Offset)) =====
        public GaianOffsetDateTime(GaianLocalDateTime localDateTime, Offset offset)
        {
            _odt = localDateTime.Value.WithOffset(offset);
        }

        public GaianOffsetDateTime(int year, int month, int day, int hour, int minute, Offset offset)
        {
            var gaianDateTime = new GaianLocalDateTime(year, month, day, hour, minute);
            _odt = gaianDateTime.Value.WithOffset(offset);
        }

        public GaianOffsetDateTime(int year, int month, int day, int hour, int minute, int second, Offset offset)
        {
            var gaianDateTime = new GaianLocalDateTime(year, month, day, hour, minute, second);
            _odt = gaianDateTime.Value.WithOffset(offset);
        }

        public GaianOffsetDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Offset offset)
        {
            var gaianDateTime = new GaianLocalDateTime(year, month, day, hour, minute, second, millisecond);
            _odt = gaianDateTime.Value.WithOffset(offset);
        }

        public GaianOffsetDateTime(OffsetDateTime odt)
        {
            this._odt = odt;
        }


        // --- Properties taken from LocalDate ---
        public CalendarSystem Calendar => _odt.Calendar;
        public int Day => GaianTools.GetDay(nodaDate);
        public IsoDayOfWeek DayOfWeek => GaianTools.GetDayOfWeek(nodaDate);
        public int DayOfYear => GaianTools.GetDayOfYear(nodaDate);
        public Era Era => throw new NotImplementedException();
        public static GaianLocalDate MaxIsoValue => throw new NotImplementedException();
        public static GaianLocalDate MinIsoValue => throw new NotImplementedException();
        public GaianMonth Month => GaianTools.GetMonth(nodaDate);
        public int Year => GaianTools.GetYear(nodaDate);



        public GaianLocalDate Date => new GaianLocalDate(nodaDate);



        // ===== Properties (mapped 1:1; Local types adapted to Gaian) =====
        public int ClockHourOfHalfDay => _odt.ClockHourOfHalfDay;
        public int Hour => _odt.Hour;
        public int Minute => _odt.Minute;
        public int Second => _odt.Second;
        public int Millisecond => _odt.Millisecond;
        public long NanosecondOfDay => _odt.NanosecondOfDay;
        public int NanosecondOfSecond => _odt.NanosecondOfSecond;
        public long TickOfDay => _odt.TickOfDay;
        public int TickOfSecond => _odt.TickOfSecond;
        public LocalTime TimeOfDay => _odt.TimeOfDay;
        public Offset Offset => _odt.Offset;

        public LocalDate nodaDate { get => _odt.Date;  }

        // ===== Static methods =====
        public static GaianOffsetDateTime Add(GaianOffsetDateTime value, Duration duration)
            => new GaianOffsetDateTime(value._odt.Plus(duration));

        public static XmlQualifiedName AddSchema(XmlSchemaSet xmlSchemaSet)
            => throw new NotImplementedException();

        public static GaianOffsetDateTime Subtract(GaianOffsetDateTime value, Duration duration)
            => new GaianOffsetDateTime(value._odt.Minus(duration));

        public static Duration Subtract(GaianOffsetDateTime end, GaianOffsetDateTime start)
            => end._odt.Minus(start._odt);

        // ===== Instance methods =====
        public bool Equals(GaianOffsetDateTime other)
            => _odt.Equals(other._odt);

        public override bool Equals(object? obj)
            => obj is GaianOffsetDateTime other && Equals(other);

        public override int GetHashCode()
            => _odt.GetHashCode();

        public GaianOffsetDateTime Plus(Duration duration)
            => new GaianOffsetDateTime(_odt.Plus(duration));

        public GaianOffsetDateTime PlusHours(int hours)
            => new GaianOffsetDateTime(_odt.PlusHours(hours));

        public GaianOffsetDateTime PlusMinutes(int minutes)
            => new GaianOffsetDateTime(_odt.PlusMinutes(minutes));

        public GaianOffsetDateTime PlusSeconds(long seconds)
            => new GaianOffsetDateTime(_odt.PlusSeconds(seconds));

        public GaianOffsetDateTime PlusMilliseconds(long milliseconds)
            => new GaianOffsetDateTime(_odt.PlusMilliseconds(milliseconds));

        public GaianOffsetDateTime PlusTicks(long ticks)
            => new GaianOffsetDateTime(_odt.PlusTicks(ticks));

        public GaianOffsetDateTime PlusNanoseconds(long nanoseconds)
            => new GaianOffsetDateTime(_odt.PlusNanoseconds(nanoseconds));

        public GaianOffsetDateTime Minus(Duration duration)
            => new GaianOffsetDateTime(_odt.Minus(duration));

        public Duration Minus(GaianOffsetDateTime other)
            => _odt.Minus(other._odt);

        public DateTimeOffset ToDateTimeOffset()
            => _odt.ToDateTimeOffset();

        public Instant ToInstant()
            => _odt.ToInstant();

        public OffsetDate ToOffsetDate()
            => _odt.ToOffsetDate();

        public OffsetTime ToOffsetTime()
            => _odt.ToOffsetTime();

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
            => new GaianOffsetDateTime(_odt.With(dateAdjuster));

        public GaianOffsetDateTime With(Func<LocalTime, LocalTime> timeAdjuster)
            => new GaianOffsetDateTime(_odt.With(timeAdjuster));

        public GaianOffsetDateTime WithCalendar(CalendarSystem calendar)
            => new GaianOffsetDateTime(_odt.WithCalendar(calendar));

        public GaianOffsetDateTime WithOffset(Offset offset)
            => new GaianOffsetDateTime(_odt.WithOffset(offset));

        // ===== Operators =====
        public static GaianOffsetDateTime operator +(GaianOffsetDateTime value, Duration duration)
            => new GaianOffsetDateTime(value._odt.Plus(duration));

        public static GaianOffsetDateTime operator -(GaianOffsetDateTime value, Duration duration)
            => new GaianOffsetDateTime(value._odt.Minus(duration));

        public static Duration operator -(GaianOffsetDateTime end, GaianOffsetDateTime start)
            => end._odt.Minus(start._odt);

        public static bool operator ==(GaianOffsetDateTime left, GaianOffsetDateTime right)
            => left._odt == right._odt;

        public static bool operator !=(GaianOffsetDateTime left, GaianOffsetDateTime right)
            => left._odt != right._odt;

        // ===== XML serialization (explicit) =====
        // Serializes as ISO 8601 offset date-time string (e.g. "2026-02-28T14:30:00+05:30").
        XmlSchema? IXmlSerializable.GetSchema() => null;

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            var text = reader.ReadElementContentAsString();
            var parsed = OffsetDateTimePattern.GeneralIso.Parse(text).GetValueOrThrow();
            System.Runtime.CompilerServices.Unsafe.AsRef(in this) = new GaianOffsetDateTime(parsed);
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteString(OffsetDateTimePattern.GeneralIso.Format(_odt));
        }

        // ===== Bridge helpers (optional, for your implementation) =====
        public static GaianOffsetDateTime FromNoda(OffsetDateTime odt)
            => new GaianOffsetDateTime(odt);

        public OffsetDateTime ToNoda()
            => _odt;
    }
}
