using Gaian;
using NodaTime;
using NodaTime.Calendars;
using Xunit;

namespace GaianNodaTimeWrappers.Tests;

public class GaianLocalDateTests
{
    // ===== Round-trip: every day in 2020–2030 =====

    [Theory]
    [MemberData(nameof(AllDays2020To2030))]
    public void RoundTrip_EveryDay_2020_2030(LocalDate isoDate)
    {
        // ISO → Gaian → ISO should be identity
        GaianLocalDate gaian = new GaianLocalDate(isoDate);
        LocalDate roundTripped = gaian.Value;
        Assert.Equal(isoDate, roundTripped);
    }

    [Theory]
    [MemberData(nameof(AllDays2020To2030))]
    public void RoundTrip_YearMonthDay_2020_2030(LocalDate isoDate)
    {
        // ISO → Gaian components → reconstruct → ISO should be identity
        GaianLocalDate gaian = new GaianLocalDate(isoDate);
        var reconstructed = new GaianLocalDate(gaian.Year, gaian.Month.Value, gaian.Day);
        Assert.Equal(isoDate, reconstructed.Value);
    }

    public static IEnumerable<object[]> AllDays2020To2030()
    {
        var start = new LocalDate(2020, 1, 1);
        var end = new LocalDate(2030, 12, 31);
        for (var d = start; d <= end; d = d.PlusDays(1))
        {
            yield return new object[] { d };
        }
    }

    // ===== Leap year boundary (week 53) =====

    [Fact]
    public void LeapYear_2020_HasWeek53()
    {
        // 2020 is an ISO week-year with 53 weeks
        Assert.Equal(53, WeekYearRules.Iso.GetWeeksInWeekYear(2020));
    }

    [Fact]
    public void LeapYear_Horus_Month14_IsValid()
    {
        // Gaian year 12020 (ISO 2020) has 53 weeks → month 14 (Horus) is valid
        var horusDate = new GaianLocalDate(12020, 14, 1);
        Assert.Equal(14, horusDate.Month.Value);
        Assert.Equal("Horus", horusDate.Month.ToString());
    }

    [Fact]
    public void NonLeapYear_Month14_Throws()
    {
        // Gaian year 12021 (ISO 2021) has only 52 weeks → month 14 should throw
        Assert.Throws<ArgumentOutOfRangeException>(() => new GaianLocalDate(12021, 14, 1));
    }

    [Fact]
    public void PlusYears_FromHorus_RollsForward()
    {
        // Horus 2, 12020 + 1 year → 12021 has no week 53 → rolls forward to Sagittarius of 12022
        var horus = new GaianLocalDate(12020, 14, 2);
        var result = horus.PlusYears(1);
        // Should skip 12021 (no week 53) and land in 12022 Sagittarius
        Assert.Equal(12022, result.Year);
        Assert.Equal(1, result.Month.Value); // Sagittarius
    }

    [Fact]
    public void PlusYears_FromRegularMonth_PreservesMonthAndDay()
    {
        // Aquarius 15, 12025 + 1 year → same Gaian date in 12026
        var date = new GaianLocalDate(12025, 3, 15);
        var result = date.PlusYears(1);
        Assert.Equal(12026, result.Year);
        Assert.Equal(3, result.Month.Value);  // Aquarius
        Assert.Equal(15, result.Day);
    }

    // ===== PlusMonths edge cases =====

    [Fact]
    public void PlusMonths_AddsExactly28Days()
    {
        var date = new GaianLocalDate(12025, 1, 1);
        var result = date.PlusMonths(1);
        var diff = Period.Between(date.Value, result.Value, PeriodUnits.Days);
        Assert.Equal(28, diff.Days);
    }

    [Fact]
    public void PlusMonths_13_WrapsToNextYear()
    {
        // Adding 13 months (13 × 28 = 364 days) should land roughly 1 Gaian year later
        var date = new GaianLocalDate(12025, 1, 1);
        var result = date.PlusMonths(13);
        var diff = Period.Between(date.Value, result.Value, PeriodUnits.Days);
        Assert.Equal(364, diff.Days);
    }

    [Fact]
    public void PlusMonths_Negative_SubtractsCorrectly()
    {
        var date = new GaianLocalDate(12025, 3, 15);
        var result = date.PlusMonths(-2);
        Assert.Equal(1, result.Month.Value); // Sagittarius (3 - 2 = 1)
    }

    // ===== Parsing all three input formats =====

    [Fact]
    public void Parse_NamedFormat()
    {
        var result = GaianLocalDate.Parse("Aquarius 15, 12025");
        Assert.Equal(12025, result.Year);
        Assert.Equal(3, result.Month.Value);
        Assert.Equal(15, result.Day);
    }

    [Fact]
    public void Parse_NumericFormat()
    {
        var result = GaianLocalDate.Parse("3/15/12025");
        Assert.Equal(12025, result.Year);
        Assert.Equal(3, result.Month.Value);
        Assert.Equal(15, result.Day);
    }

    [Fact]
    public void Parse_IsoFormat()
    {
        var result = GaianLocalDate.Parse("12025-03-15");
        Assert.Equal(12025, result.Year);
        Assert.Equal(3, result.Month.Value);
        Assert.Equal(15, result.Day);
    }

    [Fact]
    public void Parse_Invalid_Throws()
    {
        Assert.Throws<FormatException>(() => GaianLocalDate.Parse("not a date"));
    }

    // ===== Deconstruct =====

    [Fact]
    public void Deconstruct_ReturnsCorrectComponents()
    {
        var date = new GaianLocalDate(12025, 3, 15);
        var (year, month, day) = date;
        Assert.Equal(12025, year);
        Assert.Equal(3, month);
        Assert.Equal(15, day);
    }

    // ===== Operators =====

    [Fact]
    public void Comparison_Operators()
    {
        var earlier = new GaianLocalDate(12025, 1, 1);
        var later = new GaianLocalDate(12025, 2, 1);
        Assert.True(earlier < later);
        Assert.True(later > earlier);
        Assert.True(earlier <= later);
        Assert.True(later >= earlier);
        Assert.True(earlier != later);
        Assert.False(earlier == later);
    }

    [Fact]
    public void MinMax_ReturnsCorrectDate()
    {
        var a = new GaianLocalDate(12025, 1, 1);
        var b = new GaianLocalDate(12025, 6, 15);
        Assert.Equal(a, GaianLocalDate.Min(a, b));
        Assert.Equal(b, GaianLocalDate.Max(a, b));
    }
}
