using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using Gaian;
using NodaTime;
using Xunit;

namespace GaianNodaTimeWrappers.Tests;

public class GaianSerializationTests
{
    // ===== XML round-trip =====

    [Fact]
    public void Xml_GaianLocalDate_RoundTrip()
    {
        var original = new GaianLocalDate(12025, 3, 15);
        var deserialized = XmlRoundTrip(original);
        Assert.Equal(original, deserialized);
    }

    [Fact]
    public void Xml_GaianLocalDateTime_RoundTrip()
    {
        var original = new GaianLocalDateTime(12025, 3, 15, 14, 30, 0);
        var deserialized = XmlRoundTrip(original);
        Assert.Equal(original, deserialized);
    }

    [Fact]
    public void Xml_GaianOffsetDateTime_RoundTrip()
    {
        var original = new GaianOffsetDateTime(12025, 3, 15, 14, 30, Offset.FromHours(5));
        var deserialized = XmlRoundTrip(original);
        Assert.Equal(original, deserialized);
    }

    // ===== JSON round-trip =====

    [Fact]
    public void Json_GaianLocalDate_RoundTrip()
    {
        var options = new JsonSerializerOptions().AddGaianConverters();
        var original = new GaianLocalDate(12025, 3, 15);
        var json = JsonSerializer.Serialize(original, options);
        var deserialized = JsonSerializer.Deserialize<GaianLocalDate>(json, options);
        Assert.Equal(original, deserialized);
    }

    [Fact]
    public void Json_GaianLocalDateTime_RoundTrip()
    {
        var options = new JsonSerializerOptions().AddGaianConverters();
        var original = new GaianLocalDateTime(12025, 3, 15, 14, 30, 0);
        var json = JsonSerializer.Serialize(original, options);
        var deserialized = JsonSerializer.Deserialize<GaianLocalDateTime>(json, options);
        Assert.Equal(original, deserialized);
    }

    [Fact]
    public void Json_GaianOffsetDateTime_RoundTrip()
    {
        var options = new JsonSerializerOptions().AddGaianConverters();
        var original = new GaianOffsetDateTime(12025, 3, 15, 14, 30, Offset.FromHours(5));
        var json = JsonSerializer.Serialize(original, options);
        var deserialized = JsonSerializer.Deserialize<GaianOffsetDateTime>(json, options);
        Assert.Equal(original, deserialized);
    }

    [Fact]
    public void Json_GaianZonedDateTime_RoundTrip()
    {
        var options = new JsonSerializerOptions().AddGaianConverters();
        var zone = DateTimeZoneProviders.Tzdb["America/New_York"];
        var original = new GaianZonedDateTime(12025, 3, 15, 14, 30, zone);
        var json = JsonSerializer.Serialize(original, options);
        var deserialized = JsonSerializer.Deserialize<GaianZonedDateTime>(json, options);
        Assert.Equal(original.ToInstant(), deserialized.ToInstant());
        Assert.Equal(original.Zone.Id, deserialized.Zone.Id);
    }

    [Fact]
    public void Json_GaianMonth_RoundTrip()
    {
        var options = new JsonSerializerOptions().AddGaianConverters();
        var original = new GaianMonth(3); // Aquarius
        var json = JsonSerializer.Serialize(original, options);
        var deserialized = JsonSerializer.Deserialize<GaianMonth>(json, options);
        Assert.Equal(original.Value, deserialized.Value);
    }

    [Fact]
    public void Json_GaianPeriod_RoundTrip()
    {
        var options = new JsonSerializerOptions().AddGaianConverters();
        var original = GaianPeriod.FromYears(2) + GaianPeriod.FromMonths(3) + GaianPeriod.FromDays(5);
        var json = JsonSerializer.Serialize(original, options);
        var deserialized = JsonSerializer.Deserialize<GaianPeriod>(json, options);
        Assert.NotNull(deserialized);
        Assert.Equal(original.Years, deserialized!.Years);
        Assert.Equal(original.Months, deserialized.Months);
        Assert.Equal(original.Days, deserialized.Days);
    }

    // ===== Helpers =====

    private static T XmlRoundTrip<T>(T value)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var sw = new StringWriter();
        serializer.Serialize(sw, value);
        var xml = sw.ToString();
        using var sr = new StringReader(xml);
        return (T)serializer.Deserialize(sr)!;
    }
}
