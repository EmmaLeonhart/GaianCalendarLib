using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NodaTime;

namespace Gaian
{
    /// <summary>
    /// A calendar-aware span of time in the Gaian calendar system, mirroring NodaTime.Period (3.2.x).
    /// Stores Gaian years and Gaian months as distinct fields; all sub-monthly units delegate to
    /// a NodaTime <see cref="Period"/>. One Gaian month = 4 ISO weeks (28 days).
    /// Gaian year arithmetic uses ISO week-year semantics.
    /// </summary>
    public sealed class GaianPeriod :
        IEquatable<GaianPeriod?>,
        IAdditionOperators<GaianPeriod, GaianPeriod, GaianPeriod>,
        ISubtractionOperators<GaianPeriod, GaianPeriod, GaianPeriod>,
        IUnaryNegationOperators<GaianPeriod, GaianPeriod>,
        IUnaryPlusOperators<GaianPeriod, GaianPeriod>
    {
        // Gaian-specific date components
        private readonly int _years;
        private readonly int _months;

        // NodaTime Period carries weeks, days, and all time units
        private readonly Period _period;

        // ===== Private constructor =====
        private GaianPeriod(int years, int months, Period period)
        {
            _years = years;
            _months = months;
            _period = period;
        }

        // ===== Static constants =====

        /// <summary>The additive identity (all zero fields).</summary>
        public static GaianPeriod AdditiveIdentity => Zero;

        /// <summary>A period with all fields set to zero.</summary>
        public static GaianPeriod Zero => new GaianPeriod(0, 0, Period.Zero);

        /// <summary>A period with all fields set to their minimum value.</summary>
        public static GaianPeriod MinValue => new GaianPeriod(int.MinValue, int.MinValue, Period.Zero);

        /// <summary>A period with all fields set to their maximum value.</summary>
        public static GaianPeriod MaxValue => new GaianPeriod(int.MaxValue, int.MaxValue, Period.Zero);

        // ===== Component properties =====

        /// <summary>Gaian years component.</summary>
        public int Years => _years;

        /// <summary>Gaian months component (each = 4 ISO weeks / 28 days).</summary>
        public int Months => _months;

        /// <summary>ISO weeks component.</summary>
        public int Weeks => _period.Weeks;

        /// <summary>Days component.</summary>
        public int Days => _period.Days;

        /// <inheritdoc />
        public long Hours => _period.Hours;

        /// <inheritdoc />
        public long Minutes => _period.Minutes;

        /// <inheritdoc />
        public long Seconds => _period.Seconds;

        /// <inheritdoc />
        public long Milliseconds => _period.Milliseconds;

        /// <inheritdoc />
        public long Ticks => _period.Ticks;

        /// <inheritdoc />
        public long Nanoseconds => _period.Nanoseconds;

        /// <summary>Returns true if this period has any date component (years, months, weeks, or days).</summary>
        public bool HasDateComponent =>
            _years != 0 || _months != 0 || _period.HasDateComponent;

        /// <summary>Returns true if this period has any time component.</summary>
        public bool HasTimeComponent => _period.HasTimeComponent;

        // ===== Equality comparer =====

        /// <summary>An equality comparer that normalizes periods before comparing.</summary>
        public static IEqualityComparer<GaianPeriod?> NormalizingEqualityComparer =>
            new NormalizingComparer();

        private sealed class NormalizingComparer : IEqualityComparer<GaianPeriod?>
        {
            public bool Equals(GaianPeriod? x, GaianPeriod? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                return x.Normalize().Equals(y.Normalize());
            }

            public int GetHashCode(GaianPeriod? obj) => obj?.Normalize().GetHashCode() ?? 0;
        }

        // ===== Static factories =====

        /// <summary>Creates a period of the given number of Gaian years.</summary>
        public static GaianPeriod FromYears(int years) => new GaianPeriod(years, 0, Period.Zero);

        /// <summary>Creates a period of the given number of Gaian months (each = 4 ISO weeks).</summary>
        public static GaianPeriod FromMonths(int months) => new GaianPeriod(0, months, Period.Zero);

        /// <summary>Creates a period of the given number of ISO weeks.</summary>
        public static GaianPeriod FromWeeks(int weeks) => new GaianPeriod(0, 0, Period.FromWeeks(weeks));

        /// <summary>Creates a period of the given number of days.</summary>
        public static GaianPeriod FromDays(int days) => new GaianPeriod(0, 0, Period.FromDays(days));

        /// <summary>Creates a period of the given number of hours.</summary>
        public static GaianPeriod FromHours(long hours) => new GaianPeriod(0, 0, Period.FromHours(hours));

        /// <summary>Creates a period of the given number of minutes.</summary>
        public static GaianPeriod FromMinutes(long minutes) => new GaianPeriod(0, 0, Period.FromMinutes(minutes));

        /// <summary>Creates a period of the given number of seconds.</summary>
        public static GaianPeriod FromSeconds(long seconds) => new GaianPeriod(0, 0, Period.FromSeconds(seconds));

        /// <summary>Creates a period of the given number of milliseconds.</summary>
        public static GaianPeriod FromMilliseconds(long milliseconds) => new GaianPeriod(0, 0, Period.FromMilliseconds(milliseconds));

        /// <summary>Creates a period of the given number of ticks.</summary>
        public static GaianPeriod FromTicks(long ticks) => new GaianPeriod(0, 0, Period.FromTicks(ticks));

        /// <summary>Creates a period of the given number of nanoseconds.</summary>
        public static GaianPeriod FromNanoseconds(long nanoseconds) => new GaianPeriod(0, 0, Period.FromNanoseconds(nanoseconds));

        // ===== Static arithmetic =====

        /// <summary>Adds two GaianPeriods together.</summary>
        public static GaianPeriod Add(GaianPeriod left, GaianPeriod right) =>
            new GaianPeriod(
                left._years + right._years,
                left._months + right._months,
                left._period + right._period);

        /// <summary>Subtracts one GaianPeriod from another.</summary>
        public static GaianPeriod Subtract(GaianPeriod minuend, GaianPeriod subtrahend) =>
            new GaianPeriod(
                minuend._years - subtrahend._years,
                minuend._months - subtrahend._months,
                minuend._period - subtrahend._period);

        /// <summary>
        /// Returns the period between two Gaian dates, expressed in Gaian years, months, and days.
        /// Uses day-based calculation: 1 year ≈ 364 days, 1 month = 28 days.
        /// </summary>
        public static GaianPeriod Between(GaianLocalDate start, GaianLocalDate end)
        {
            int totalDays = (int)Period.Between((LocalDate)start, (LocalDate)end, PeriodUnits.Days).Days;
            return FromTotalDays(totalDays);
        }

        /// <summary>Returns the period between two Gaian date-times (date component only).</summary>
        public static GaianPeriod Between(GaianLocalDateTime start, GaianLocalDateTime end) =>
            Between(start.Date, end.Date);

        /// <summary>Returns the period between two Gaian date-times (date component only; units ignored).</summary>
        public static GaianPeriod Between(GaianLocalDateTime start, GaianLocalDateTime end, PeriodUnits units) =>
            Between(start, end);

        /// <summary>Returns the period between two LocalTimes, delegating to NodaTime.</summary>
        public static GaianPeriod Between(LocalTime start, LocalTime end) =>
            new GaianPeriod(0, 0, Period.Between(start, end));

        /// <summary>Returns the period between two LocalTimes with the given units, delegating to NodaTime.</summary>
        public static GaianPeriod Between(LocalTime start, LocalTime end, PeriodUnits units) =>
            new GaianPeriod(0, 0, Period.Between(start, end, units));

        /// <summary>Returns the exact number of days between two Gaian dates.</summary>
        public static int DaysBetween(GaianLocalDate start, GaianLocalDate end) =>
            (int)Period.Between((LocalDate)start, (LocalDate)end, PeriodUnits.Days).Days;

        /// <summary>
        /// Creates a comparer that orders GaianPeriods by the date they produce when applied to
        /// <paramref name="baseDateTime"/>.
        /// </summary>
        public static IComparer<GaianPeriod?> CreateComparer(GaianLocalDateTime baseDateTime) =>
            Comparer<GaianPeriod?>.Create((a, b) =>
            {
                if (a is null && b is null) return 0;
                if (a is null) return -1;
                if (b is null) return 1;
                var dateA = a.ApplyTo(baseDateTime.Date);
                var dateB = b.ApplyTo(baseDateTime.Date);
                return ((LocalDate)dateA).CompareTo((LocalDate)dateB);
            });

        // ===== Instance methods =====

        /// <summary>Returns a normalized copy of this period (normalizes the sub-period; Gaian years/months unchanged).</summary>
        public GaianPeriod Normalize() => new GaianPeriod(_years, _months, _period.Normalize());

        /// <summary>
        /// Converts this period to a <see cref="Duration"/>. Only valid if the Gaian years and months
        /// fields are both zero; throws <see cref="InvalidOperationException"/> otherwise.
        /// </summary>
        public Duration ToDuration()
        {
            if (_years != 0 || _months != 0)
                throw new InvalidOperationException(
                    "Cannot convert a GaianPeriod with year or month components to a Duration. " +
                    "Convert the years/months to weeks or days first.");
            return _period.ToDuration();
        }

        /// <summary>
        /// Returns a <see cref="PeriodBuilder"/> for the sub-period (weeks, days, time units).
        /// Note: the Gaian years and months are NOT included in the builder.
        /// </summary>
        public PeriodBuilder ToBuilder() => _period.ToBuilder();

        /// <summary>Returns a string representation of this period in an ISO-8601-inspired format.</summary>
        public override string ToString()
        {
            if (_years == 0 && _months == 0)
                return _period.ToString();

            var sb = new StringBuilder("P");
            if (_years != 0) sb.Append(_years).Append("GY");
            if (_months != 0) sb.Append(_months).Append("GM");
            // Append NodaTime period components (strip the leading "P")
            var subStr = _period.ToString();
            if (subStr.Length > 1)
                sb.Append(subStr.Substring(1));
            return sb.ToString();
        }

        // ===== Equality =====

        /// <inheritdoc />
        public bool Equals(GaianPeriod? other)
        {
            if (other is null) return false;
            return _years == other._years && _months == other._months && _period.Equals(other._period);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) => Equals(obj as GaianPeriod);

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(_years, _months, _period);

        // ===== Operators =====

        /// <inheritdoc />
        public static GaianPeriod operator +(GaianPeriod left, GaianPeriod right) => Add(left, right);

        /// <inheritdoc />
        public static GaianPeriod operator -(GaianPeriod minuend, GaianPeriod subtrahend) => Subtract(minuend, subtrahend);

        /// <summary>Negates all components of this period.</summary>
        public static GaianPeriod operator -(GaianPeriod period) =>
            new GaianPeriod(-period._years, -period._months, NegatePeriod(period._period));

        /// <summary>Returns the period unchanged.</summary>
        public static GaianPeriod operator +(GaianPeriod period) => period;

        // ===== Bridges =====

        /// <summary>Wraps a NodaTime <see cref="Period"/> as a GaianPeriod (years/months treated as ISO units).</summary>
        public static GaianPeriod FromNoda(Period p)
        {
            var pb = new PeriodBuilder
            {
                Weeks = p.Weeks,
                Days = p.Days,
                Hours = p.Hours,
                Minutes = p.Minutes,
                Seconds = p.Seconds,
                Milliseconds = p.Milliseconds,
                Ticks = p.Ticks,
                Nanoseconds = p.Nanoseconds
            };
            // ISO months → Gaian months (approximate: treat as same)
            // ISO years → Gaian years (approximate: treat as same)
            return new GaianPeriod(p.Years, p.Months, pb.Build());
        }

        /// <summary>
        /// Converts this GaianPeriod to a NodaTime <see cref="Period"/>.
        /// Gaian months are expressed as 4 ISO weeks each; Gaian years are kept as ISO years (approximate).
        /// </summary>
        public Period ToNoda()
        {
            var pb = new PeriodBuilder
            {
                Years = _years,
                Months = 0,
                Weeks = _period.Weeks + _months * 4,
                Days = _period.Days,
                Hours = _period.Hours,
                Minutes = _period.Minutes,
                Seconds = _period.Seconds,
                Milliseconds = _period.Milliseconds,
                Ticks = _period.Ticks,
                Nanoseconds = _period.Nanoseconds
            };
            return pb.Build();
        }

        // ===== Internal application =====

        /// <summary>Applies this period to a GaianLocalDate.</summary>
        internal GaianLocalDate ApplyTo(GaianLocalDate date)
        {
            GaianLocalDate result = date;
            if (_years != 0) result = result.PlusYears(_years);
            if (_months != 0) result = new GaianLocalDate(((LocalDate)result).PlusWeeks(_months * 4));
            if (_period != Period.Zero) result = new GaianLocalDate(((LocalDate)result) + _period);
            return result;
        }

        /// <summary>Applies this period to a GaianLocalDateTime.</summary>
        internal GaianLocalDateTime ApplyTo(GaianLocalDateTime dt)
        {
            var newDate = ApplyTo(dt.Date);
            return new GaianLocalDateTime(newDate.Value.At(dt.TimeOfDay));
        }

        // ===== Private helpers =====

        private static GaianPeriod FromTotalDays(int totalDays)
        {
            int sign = totalDays >= 0 ? 1 : -1;
            int absDays = Math.Abs(totalDays);

            int years = absDays / 364;
            int remainder = absDays % 364;
            int months = remainder / 28;
            int days = remainder % 28;

            return new GaianPeriod(years * sign, months * sign, Period.FromDays(days * sign));
        }

        private static Period NegatePeriod(Period p)
        {
            var pb = new PeriodBuilder
            {
                Weeks = -p.Weeks,
                Days = -p.Days,
                Hours = -p.Hours,
                Minutes = -p.Minutes,
                Seconds = -p.Seconds,
                Milliseconds = -p.Milliseconds,
                Ticks = -p.Ticks,
                Nanoseconds = -p.Nanoseconds
            };
            return pb.Build();
        }
    }
}
