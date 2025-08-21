using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using NodaTime;
using Gaian;

namespace GaianTime
{
    public sealed class GaianZonedDateTime : IEquatable<GaianZonedDateTime>, IComparable<GaianZonedDateTime>, IComparable
    {
        private readonly ZonedDateTime _inner;

        public GaianZonedDateTime(ZonedDateTime inner)
        {
            _inner = inner;
        }

        // =====================
        // Properties
        // =====================

        public LocalDateTime LocalDateTime => throw new NotImplementedException();
        public GaianLocalDate Date => throw new NotImplementedException();
        public LocalTime TimeOfDay => throw new NotImplementedException();
        public Offset Offset => throw new NotImplementedException();
        public DateTimeZone Zone => throw new NotImplementedException();
        public Instant ToInstant() => throw new NotImplementedException();
        public long Ticks => throw new NotImplementedException();
        public int NanosecondOfDay => throw new NotImplementedException();
        public int NanosecondOfSecond => throw new NotImplementedException();

        // Calendar fields
        public int Year => throw new NotImplementedException();
        public int Month => throw new NotImplementedException();
        public int Day => throw new NotImplementedException();
        public int Hour => throw new NotImplementedException();
        public int Minute => throw new NotImplementedException();
        public int Second => throw new NotImplementedException();
        public IsoDayOfWeek DayOfWeek => throw new NotImplementedException();
        public int DayOfYear => throw new NotImplementedException();

        // =====================
        // Methods
        // =====================

        public GaianZonedDateTime WithZone(DateTimeZone zone) => throw new NotImplementedException();
        public GaianZonedDateTime WithEarlierOffsetAtOverlap() => throw new NotImplementedException();
        public GaianZonedDateTime WithLaterOffsetAtOverlap() => throw new NotImplementedException();

        public GaianZonedDateTime Plus(Duration duration) => throw new NotImplementedException();
        public GaianZonedDateTime Minus(Duration duration) => throw new NotImplementedException();
        public GaianZonedDateTime Plus(Period period) => throw new NotImplementedException();
        public GaianZonedDateTime Minus(Period period) => throw new NotImplementedException();

        public Duration Minus(GaianZonedDateTime other) => throw new NotImplementedException();

        public ZonedDateTime WithZoneSameLocal(DateTimeZone zone) => throw new NotImplementedException();
        public ZonedDateTime WithZoneSameInstant(DateTimeZone zone) => throw new NotImplementedException();

        // Conversion helpers
        public OffsetDateTime ToOffsetDateTime() => throw new NotImplementedException();
        public LocalDateTime ToLocalDateTime() => throw new NotImplementedException();

        // =====================
        // Operators
        // =====================

        public static Duration operator -(GaianZonedDateTime left, GaianZonedDateTime right) => throw new NotImplementedException();
        public static bool operator ==(GaianZonedDateTime left, GaianZonedDateTime right) => throw new NotImplementedException();
        public static bool operator !=(GaianZonedDateTime left, GaianZonedDateTime right) => throw new NotImplementedException();
        public static bool operator <(GaianZonedDateTime left, GaianZonedDateTime right) => throw new NotImplementedException();
        public static bool operator <=(GaianZonedDateTime left, GaianZonedDateTime right) => throw new NotImplementedException();
        public static bool operator >(GaianZonedDateTime left, GaianZonedDateTime right) => throw new NotImplementedException();
        public static bool operator >=(GaianZonedDateTime left, GaianZonedDateTime right) => throw new NotImplementedException();

        // =====================
        // Equality / Comparison
        // =====================

        public bool Equals(GaianZonedDateTime other) => throw new NotImplementedException();
        public override bool Equals(object obj) => throw new NotImplementedException();
        public override int GetHashCode() => throw new NotImplementedException();
        public int CompareTo(GaianZonedDateTime other) => throw new NotImplementedException();
        public int CompareTo(object obj) => throw new NotImplementedException();

        // =====================
        // Formatting
        // =====================

        public override string ToString() => throw new NotImplementedException();
        public string ToString(string patternText, IFormatProvider formatProvider) => throw new NotImplementedException();
    }
}
