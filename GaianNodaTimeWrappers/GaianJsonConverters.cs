using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NodaTime;
using NodaTime.Text;

namespace Gaian
{
    /// <summary>
    /// System.Text.Json converters for Gaian calendar types.
    /// All types serialize as ISO 8601 strings for maximum interoperability.
    /// Register via <see cref="GaianJsonSerializerOptions.AddGaianConverters"/>.
    /// </summary>
    public static class GaianJsonSerializerOptions
    {
        /// <summary>Adds all Gaian JSON converters to the given options.</summary>
        public static JsonSerializerOptions AddGaianConverters(this JsonSerializerOptions options)
        {
            options.Converters.Add(new GaianLocalDateJsonConverter());
            options.Converters.Add(new GaianLocalDateTimeJsonConverter());
            options.Converters.Add(new GaianOffsetDateTimeJsonConverter());
            options.Converters.Add(new GaianZonedDateTimeJsonConverter());
            options.Converters.Add(new GaianMonthJsonConverter());
            options.Converters.Add(new GaianPeriodJsonConverter());
            return options;
        }
    }

    /// <summary>Converts <see cref="GaianLocalDate"/> to/from ISO 8601 date string (e.g. "2026-02-28").</summary>
    public sealed class GaianLocalDateJsonConverter : JsonConverter<GaianLocalDate>
    {
        public override GaianLocalDate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var text = reader.GetString() ?? throw new JsonException("Expected a string for GaianLocalDate.");
            return new GaianLocalDate(LocalDatePattern.Iso.Parse(text).GetValueOrThrow());
        }

        public override void Write(Utf8JsonWriter writer, GaianLocalDate value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(LocalDatePattern.Iso.Format(value.Value));
        }
    }

    /// <summary>Converts <see cref="GaianLocalDateTime"/> to/from ISO 8601 date-time string (e.g. "2026-02-28T14:30:00").</summary>
    public sealed class GaianLocalDateTimeJsonConverter : JsonConverter<GaianLocalDateTime>
    {
        public override GaianLocalDateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var text = reader.GetString() ?? throw new JsonException("Expected a string for GaianLocalDateTime.");
            return new GaianLocalDateTime(LocalDateTimePattern.GeneralIso.Parse(text).GetValueOrThrow());
        }

        public override void Write(Utf8JsonWriter writer, GaianLocalDateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(LocalDateTimePattern.GeneralIso.Format(value.Value));
        }
    }

    /// <summary>Converts <see cref="GaianOffsetDateTime"/> to/from ISO 8601 offset date-time string (e.g. "2026-02-28T14:30:00+05:30").</summary>
    public sealed class GaianOffsetDateTimeJsonConverter : JsonConverter<GaianOffsetDateTime>
    {
        public override GaianOffsetDateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var text = reader.GetString() ?? throw new JsonException("Expected a string for GaianOffsetDateTime.");
            return new GaianOffsetDateTime(OffsetDateTimePattern.GeneralIso.Parse(text).GetValueOrThrow());
        }

        public override void Write(Utf8JsonWriter writer, GaianOffsetDateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(OffsetDateTimePattern.GeneralIso.Format(value.ToNoda()));
        }
    }

    /// <summary>
    /// Converts <see cref="GaianZonedDateTime"/> to/from a string containing the ISO 8601 offset
    /// date-time followed by the IANA zone ID (e.g. "2026-02-28T14:30:00+05:30 America/New_York").
    /// </summary>
    public sealed class GaianZonedDateTimeJsonConverter : JsonConverter<GaianZonedDateTime>
    {
        public override GaianZonedDateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var text = reader.GetString() ?? throw new JsonException("Expected a string for GaianZonedDateTime.");
            var spaceIndex = text.LastIndexOf(' ');
            if (spaceIndex < 0)
                throw new JsonException($"Invalid GaianZonedDateTime format: '{text}'. Expected 'offset-datetime zoneId'.");
            var odtText = text.Substring(0, spaceIndex);
            var zoneId = text.Substring(spaceIndex + 1);
            var odt = OffsetDateTimePattern.GeneralIso.Parse(odtText).GetValueOrThrow();
            var zone = DateTimeZoneProviders.Tzdb[zoneId];
            return new GaianZonedDateTime(odt.InZone(zone));
        }

        public override void Write(Utf8JsonWriter writer, GaianZonedDateTime value, JsonSerializerOptions options)
        {
            var zdt = value.ToNoda();
            var odtText = OffsetDateTimePattern.GeneralIso.Format(zdt.ToOffsetDateTime());
            writer.WriteStringValue($"{odtText} {zdt.Zone.Id}");
        }
    }

    /// <summary>Converts <see cref="GaianMonth"/> to/from its numeric value (1–14).</summary>
    public sealed class GaianMonthJsonConverter : JsonConverter<GaianMonth>
    {
        public override GaianMonth Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetInt32();
            return new GaianMonth(value);
        }

        public override void Write(Utf8JsonWriter writer, GaianMonth value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.Value);
        }
    }

    /// <summary>Converts <see cref="GaianPeriod"/> to/from its ToString() representation.</summary>
    public sealed class GaianPeriodJsonConverter : JsonConverter<GaianPeriod>
    {
        public override GaianPeriod Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var text = reader.GetString() ?? throw new JsonException("Expected a string for GaianPeriod.");
            // Parse the NodaTime sub-period, then extract Gaian components
            // Format: "P3GY2GMW1D" or just the NodaTime period format
            return ParseGaianPeriod(text);
        }

        public override void Write(Utf8JsonWriter writer, GaianPeriod value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }

        private static GaianPeriod ParseGaianPeriod(string text)
        {
            if (string.IsNullOrEmpty(text) || text == "P0D")
                return GaianPeriod.Zero;

            int gaianYears = 0, gaianMonths = 0;
            var remaining = text;

            if (!remaining.StartsWith("P"))
                throw new JsonException($"Invalid GaianPeriod format: '{text}'.");

            remaining = remaining.Substring(1); // strip "P"

            // Extract Gaian years (e.g. "3GY")
            var gyIndex = remaining.IndexOf("GY");
            if (gyIndex >= 0)
            {
                gaianYears = int.Parse(remaining.Substring(0, gyIndex));
                remaining = remaining.Substring(gyIndex + 2);
            }

            // Extract Gaian months (e.g. "2GM")
            var gmIndex = remaining.IndexOf("GM");
            if (gmIndex >= 0)
            {
                gaianMonths = int.Parse(remaining.Substring(0, gmIndex));
                remaining = remaining.Substring(gmIndex + 2);
            }

            // Parse remaining as NodaTime period
            Period subPeriod = Period.Zero;
            if (remaining.Length > 0)
            {
                var nodaText = "P" + remaining;
                var parseResult = PeriodPattern.NormalizingIso.Parse(nodaText);
                subPeriod = parseResult.GetValueOrThrow();
            }

            // Reconstruct: years via FromYears, months via FromMonths, then add sub-period
            var result = GaianPeriod.FromYears(gaianYears)
                       + GaianPeriod.FromMonths(gaianMonths)
                       + GaianPeriod.FromNoda(subPeriod);
            return result;
        }
    }
}
