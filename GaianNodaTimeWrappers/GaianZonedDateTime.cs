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

        // ========= Constructors (mirror Noda) =========
        public GaianZonedDateTime(Instant instant, DateTimeZone zone) => throw new NotImplementedException();                // docs L8
        public GaianZonedDateTime(Instant instant, DateTimeZone zone, CalendarSystem calendar) => throw new NotImplementedException(); // docs L9
        public GaianZonedDateTime(GaianLocalDateTime localDateTime, DateTimeZone zone, Offset offset) => throw new NotImplementedException(); // adapted from docs L10-L11

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
        //I am not including  => throw new NotImplementedException();


        // ========= Core properties (mirror names/semantics) =========
        //public CalendarSystem Calendar => throw new NotImplementedException();                 // L12
        public int ClockHourOfHalfDay => throw new NotImplementedException();                  // L12-13
        public GaianLocalDate Date => throw new NotImplementedException();                          // L14 (Gaian-adapted)
        //public int Day => throw new NotImplementedException();                                 // L15
        //public IsoDayOfWeek DayOfWeek => throw new NotImplementedException();                  // L16
        //public int DayOfYear => throw new NotImplementedException();                           // L16
        //public Era Era => throw new NotImplementedException();                                 // L17
        public int Hour => _zdt.Hour;                             // L17-18
        public GaianLocalDateTime LocalDateTime => new GaianLocalDateTime(_zdt.LocalDateTime);//throw new NotImplementedException();        // L18-19 (Gaian-adapted)
        public int Millisecond => _zdt.Millisecond;                         // L19-20
        public int Minute => _zdt.Minute;// throw new NotImplementedException();                              // L21
        //public int Month => throw new NotImplementedException();                               // L21
        public long NanosecondOfDay => _zdt.NanosecondOfDay;                    // L22-25
        public int NanosecondOfSecond => _zdt.NanosecondOfSecond;                  // L26
        public Offset Offset => _zdt.Offset;                           // L27
        public int Second => _zdt.Second; //throw new NotImplementedException();                              // L28
        public long TickOfDay => _zdt.TickOfDay; //throw new NotImplementedException();                          // L29-31
        public int TickOfSecond => _zdt.TickOfSecond; //throw new NotImplementedException();                        // L32
        public LocalTime TimeOfDay => _zdt.TimeOfDay; //throw new NotImplementedException();                     // L33
        //public int Year => throw new NotImplementedException();                                // L34
        ////I am not including  => throw new NotImplementedException();                           // L35
        public DateTimeZone Zone => throw new NotImplementedException();                       // L35-36

        // ========= Static helpers (mirror) =========
        public static GaianZonedDateTime Add(GaianZonedDateTime zonedDateTime, Duration duration) => throw new NotImplementedException();      // L36-41
        public static XmlQualifiedName AddSchema(XmlSchemaSet xmlSchemaSet) => throw new NotImplementedException();                           // L41-42
        public static GaianZonedDateTime FromDateTimeOffset(DateTimeOffset dateTimeOffset)
        {
            return new GaianZonedDateTime(ZonedDateTime.FromDateTimeOffset(dateTimeOffset));
            throw new NotImplementedException();            // L47-48
        }

        public static GaianZonedDateTime Subtract(GaianZonedDateTime zonedDateTime, Duration duration) => throw new NotImplementedException(); // L84-88
        public static Duration Subtract(GaianZonedDateTime end, GaianZonedDateTime start) => throw new NotImplementedException();             // L89-91

        // ========= Instance methods (mirror) =========
        public void Deconstruct(out GaianLocalDateTime localDateTime, out DateTimeZone dateTimeZone, out Offset offset) => throw new NotImplementedException(); // L42-43 (Gaian-adapted)

        public bool Equals(GaianZonedDateTime other) => _zdt.Equals(other._zdt);   // L44
        public override bool Equals(object? obj) => obj is GaianZonedDateTime other && Equals(other);       // L45-46
        public override int GetHashCode() => _zdt.GetHashCode();              // L49

        public ZoneInterval GetZoneInterval() => _zdt.GetZoneInterval();          // L49-50
        public bool IsDaylightSavingTime() => _zdt.IsDaylightSavingTime();             // L51-56

        public GaianZonedDateTime Minus(Duration duration) => throw new NotImplementedException();      // L56-60
        public Duration Minus(GaianZonedDateTime other) => throw new NotImplementedException();         // L61-63
        public GaianZonedDateTime Plus(Duration duration) => throw new NotImplementedException();       // L64-68

        public GaianZonedDateTime PlusHours(int hours) => new GaianZonedDateTime(_zdt.PlusHours(hours));          // L69-71
        public GaianZonedDateTime PlusMilliseconds(long milliseconds) => new GaianZonedDateTime(_zdt.PlusMilliseconds(milliseconds)); // L71-73
        public GaianZonedDateTime PlusMinutes(int minutes) => new GaianZonedDateTime(_zdt.PlusMinutes(minutes));      // L74-76
        public GaianZonedDateTime PlusNanoseconds(long nanoseconds) => new GaianZonedDateTime(_zdt.PlusNanoseconds(nanoseconds)); // L76-78
        public GaianZonedDateTime PlusSeconds(long seconds) => new GaianZonedDateTime(_zdt.PlusSeconds(seconds));     // L79-80
        public GaianZonedDateTime PlusTicks(long ticks) => new GaianZonedDateTime(_zdt.PlusTicks(ticks));         // L81-83

        public DateTimeOffset ToDateTimeOffset() => _zdt.ToDateTimeOffset();        // L92-96
        public DateTime ToDateTimeUnspecified() => _zdt.ToDateTimeUnspecified();         // L97-101
        public DateTime ToDateTimeUtc() => _zdt.ToDateTimeUtc();                 // L101-103
        public Instant ToInstant() => _zdt.ToInstant();                      // L103-104
        public OffsetDateTime ToOffsetDateTime() => _zdt.ToOffsetDateTime();        // L105


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
        public static GaianZonedDateTime operator +(GaianZonedDateTime zonedDateTime, Duration duration) => throw new NotImplementedException(); // L112-117
        public static bool operator ==(GaianZonedDateTime left, GaianZonedDateTime right) => left._zdt == right._zdt;               // L118-119
        public static bool operator !=(GaianZonedDateTime left, GaianZonedDateTime right) => left._zdt != right._zdt;               // L120
        public static GaianZonedDateTime operator -(GaianZonedDateTime zonedDateTime, Duration duration) => throw new NotImplementedException(); // L121-125
        public static Duration operator -(GaianZonedDateTime end, GaianZonedDateTime start) => throw new NotImplementedException();             // L126-128

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
