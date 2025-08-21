using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using NodaTime;
using Gaian;
using System;
using System.Numerics;                // for generic operator interfaces
using System.Xml;
using System.Xml.Schema;
using NodaTime;
using NodaTime.Calendars;
using NodaTime.TimeZones;
using System.Xml.Serialization;
using NodaTime.Text;
using System.Globalization;

namespace Gaian
{
    /// <summary>
    /// Gaian wrapper mirroring NodaTime.ZonedDateTime (3.2.x).
    /// </summary>
    public readonly struct GaianZonedDateTime :
        IEquatable<GaianZonedDateTime>,
        IFormattable,
        IXmlSerializable,
        IAdditionOperators<GaianZonedDateTime, Duration, GaianZonedDateTime>,
        ISubtractionOperators<GaianZonedDateTime, GaianZonedDateTime, Duration>,
        ISubtractionOperators<GaianZonedDateTime, Duration, GaianZonedDateTime>,
        IEqualityOperators<GaianZonedDateTime, GaianZonedDateTime, bool>
    {
        private readonly ZonedDateTime _zdt;

        private ZonedDateTime Value => _zdt;

        public GaianZonedDateTime(Instant instant, DateTimeZone zone) => throw new NotImplementedException();
        public GaianZonedDateTime(Instant instant, DateTimeZone zone, CalendarSystem calendar) => throw new NotImplementedException();
        public GaianZonedDateTime(GaianLocalDateTime localDateTime, DateTimeZone zone, Offset offset)
        {
            _zdt = localDateTime.Value.InZone(zone, Resolvers.CreateMappingResolver(Resolvers.LenientResolver, zone, localDateTime.Value, offset));
        }

        public GaianZonedDateTime(int year, int month, int day, int hour, int minute, DateTimeZone zone)
        {
            var gaianDateTime = new GaianLocalDateTime(year, month, day, hour, minute);
            _zdt = gaianDateTime.Value.InZoneLeniently(zone);
        }

        public GaianZonedDateTime(int year, int month, int day, int hour, int minute, int second, DateTimeZone zone)
        {
            var gaianDateTime = new GaianLocalDateTime(year, month, day, hour, minute, second);
            _zdt = gaianDateTime.Value.InZoneLeniently(zone);
        }

        public GaianZonedDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeZone zone)
        {
            var gaianDateTime = new GaianLocalDateTime(year, month, day, hour, minute, second, millisecond);
            _zdt = gaianDateTime.Value.InZoneLeniently(zone);
        }

        public GaianZonedDateTime(ZonedDateTime zdt)
        {
            this._zdt = zdt;
        }



        public LocalDate nodaDate => this._zdt.Date;


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


        public int ClockHourOfHalfDay => _zdt.ClockHourOfHalfDay;
        public GaianLocalDate Date => new GaianLocalDate(_zdt.Date);
        public int Hour => _zdt.Hour;
        public GaianLocalDateTime LocalDateTime => new GaianLocalDateTime(_zdt.LocalDateTime);
        public int Millisecond => _zdt.Millisecond;
        public int Minute => _zdt.Minute;
        public long NanosecondOfDay => _zdt.NanosecondOfDay;
        public int NanosecondOfSecond => _zdt.NanosecondOfSecond;
        public Offset Offset => _zdt.Offset;
        public int Second => _zdt.Second;
        public long TickOfDay => _zdt.TickOfDay;
        public int TickOfSecond => _zdt.TickOfSecond;
        public LocalTime TimeOfDay => _zdt.TimeOfDay;
        public DateTimeZone Zone => _zdt.Zone;

        // ========= Static helpers (mirror) =========
        public static GaianZonedDateTime Add(GaianZonedDateTime zonedDateTime, Duration duration) => new GaianZonedDateTime(zonedDateTime._zdt.Plus(duration));      // L36-41
        public static XmlQualifiedName AddSchema(XmlSchemaSet xmlSchemaSet) => throw new NotImplementedException();                           // L41-42
        public static GaianZonedDateTime FromDateTimeOffset(DateTimeOffset dateTimeOffset)
        {
            return new GaianZonedDateTime(ZonedDateTime.FromDateTimeOffset(dateTimeOffset));
            throw new NotImplementedException();            // L47-48
        }

        public static GaianZonedDateTime Subtract(GaianZonedDateTime zonedDateTime, Duration duration) => new GaianZonedDateTime(zonedDateTime._zdt.Minus(duration)); // L84-88
        public static Duration Subtract(GaianZonedDateTime end, GaianZonedDateTime start) => end._zdt.Minus(start._zdt);             // L89-91

        // ========= Instance methods (mirror) =========
        public void Deconstruct(out GaianLocalDateTime localDateTime, out DateTimeZone dateTimeZone, out Offset offset) => throw new NotImplementedException(); // L42-43 (Gaian-adapted)

        public bool Equals(GaianZonedDateTime other) => _zdt.Equals(other._zdt);
        public override bool Equals(object? obj) => obj is GaianZonedDateTime other && Equals(other);
        public override int GetHashCode() => _zdt.GetHashCode();

        public ZoneInterval GetZoneInterval() => _zdt.GetZoneInterval();
        public bool IsDaylightSavingTime() => _zdt.IsDaylightSavingTime();

        public GaianZonedDateTime Minus(Duration duration) => new GaianZonedDateTime(_zdt.Minus(duration));      // L56-60
        public Duration Minus(GaianZonedDateTime other) => _zdt.Minus(other._zdt);         // L61-63
        public GaianZonedDateTime Plus(Duration duration) => new GaianZonedDateTime(_zdt.Plus(duration));       // L64-68

        public GaianZonedDateTime PlusHours(int hours) => new GaianZonedDateTime(_zdt.PlusHours(hours));
        public GaianZonedDateTime PlusMilliseconds(long milliseconds) => new GaianZonedDateTime(_zdt.PlusMilliseconds(milliseconds));
        public GaianZonedDateTime PlusMinutes(int minutes) => new GaianZonedDateTime(_zdt.PlusMinutes(minutes));
        public GaianZonedDateTime PlusNanoseconds(long nanoseconds) => new GaianZonedDateTime(_zdt.PlusNanoseconds(nanoseconds));
        public GaianZonedDateTime PlusSeconds(long seconds) => new GaianZonedDateTime(_zdt.PlusSeconds(seconds));
        public GaianZonedDateTime PlusTicks(long ticks) => new GaianZonedDateTime(_zdt.PlusTicks(ticks));

        public DateTimeOffset ToDateTimeOffset() => _zdt.ToDateTimeOffset();
        public DateTime ToDateTimeUnspecified() => _zdt.ToDateTimeUnspecified();
        public DateTime ToDateTimeUtc() => _zdt.ToDateTimeUtc();
        public Instant ToInstant() => _zdt.ToInstant();
        public OffsetDateTime ToOffsetDateTime() => _zdt.ToOffsetDateTime();


        public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

        public string ToString(string? patternText, IFormatProvider? formatProvider)
        {
            var culture = (formatProvider as CultureInfo) ?? CultureInfo.CurrentCulture;

            if (string.IsNullOrEmpty(patternText) ||
                string.Equals(patternText, "G", StringComparison.OrdinalIgnoreCase))
            {
                var gdate = new GaianLocalDate(_zdt.Date).ToString(null, culture);
                var time = _zdt.TimeOfDay.ToString("HH':'mm", culture);
                var off = _zdt.Offset.ToString("g", culture);
                return $"{gdate} {time} [{_zdt.Zone.Id}] {off}";
            }

            // Provide resolver, zone provider, and template value:
            var resolver = Resolvers.LenientResolver;                 // or your custom resolver
            var zoneProv = DateTimeZoneProviders.Tzdb;
            var template = new LocalDateTime(2000, 1, 1, 0, 0)
                              .InZoneLeniently(zoneProv["UTC"]);      // reasonable template

            var pattern = ZonedDateTimePattern.Create(patternText, culture, resolver, zoneProv, template);
            return pattern.Format(_zdt);
        }


        public GaianZonedDateTime WithCalendar(CalendarSystem calendar) => throw new NotImplementedException(); // L110-111
        public GaianZonedDateTime WithZone(DateTimeZone targetZone) => throw new NotImplementedException();     // L111-112

        // ========= Operators (mirror) =========
        public static GaianZonedDateTime operator +(GaianZonedDateTime zonedDateTime, Duration duration) => new GaianZonedDateTime(zonedDateTime._zdt.Plus(duration)); // L112-117
        public static bool operator ==(GaianZonedDateTime left, GaianZonedDateTime right) => left._zdt == right._zdt;
        public static bool operator !=(GaianZonedDateTime left, GaianZonedDateTime right) => left._zdt != right._zdt;
        public static GaianZonedDateTime operator -(GaianZonedDateTime zonedDateTime, Duration duration) => new GaianZonedDateTime(zonedDateTime._zdt.Minus(duration)); // L121-125
        public static Duration operator -(GaianZonedDateTime end, GaianZonedDateTime start) => end._zdt.Minus(start._zdt);             // L126-128

        // ========= XML serialization (explicit) =========
        XmlSchema? IXmlSerializable.GetSchema() => throw new NotImplementedException();         // L129-133 (3.2.x change)
        void IXmlSerializable.ReadXml(XmlReader reader) => throw new NotImplementedException(); // L130-131
        void IXmlSerializable.WriteXml(XmlWriter writer) => throw new NotImplementedException();// L131-132

        // ========= Bridges to raw Noda types (helpers for your impl) =========
        public static GaianZonedDateTime FromNoda(ZonedDateTime zdt) => new GaianZonedDateTime(zdt);
        public ZonedDateTime ToNoda() => _zdt;

        // Optional: raw deconstruct (for interop)
        public void Deconstruct(out LocalDateTime localDateTime, out DateTimeZone zone, out Offset offset) => throw new NotImplementedException();
    }
}
