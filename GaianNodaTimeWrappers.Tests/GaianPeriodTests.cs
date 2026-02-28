using Gaian;
using NodaTime;
using Xunit;

namespace GaianNodaTimeWrappers.Tests;

public class GaianPeriodTests
{
    [Fact]
    public void Between_SameDate_ReturnsZero()
    {
        var date = new GaianLocalDate(12025, 3, 15);
        var period = GaianPeriod.Between(date, date);
        Assert.Equal(0, period.Years);
        Assert.Equal(0, period.Months);
        Assert.Equal(0, period.Days);
    }

    [Fact]
    public void Between_ExactlyOneMonth_Returns1Month()
    {
        var start = new GaianLocalDate(12025, 1, 1);
        var end = start.PlusMonths(1);
        var period = GaianPeriod.Between(start, end);
        Assert.Equal(0, period.Years);
        Assert.Equal(1, period.Months);
        Assert.Equal(0, period.Days);
    }

    [Fact]
    public void Between_ExactlyOneYear_Returns1Year()
    {
        var start = new GaianLocalDate(12025, 1, 1);
        var end = start.PlusDays(364);
        var period = GaianPeriod.Between(start, end);
        Assert.Equal(1, period.Years);
        Assert.Equal(0, period.Months);
        Assert.Equal(0, period.Days);
    }

    [Fact]
    public void Between_Mixed_ReturnsCorrectComponents()
    {
        var start = new GaianLocalDate(12025, 1, 1);
        // 1 year + 2 months + 5 days = 364 + 56 + 5 = 425 days
        var end = start.PlusDays(425);
        var period = GaianPeriod.Between(start, end);
        Assert.Equal(1, period.Years);
        Assert.Equal(2, period.Months);
        Assert.Equal(5, period.Days);
    }

    [Fact]
    public void DaysBetween_ReturnsExactDays()
    {
        var start = new GaianLocalDate(12025, 1, 1);
        var end = start.PlusDays(100);
        Assert.Equal(100, GaianPeriod.DaysBetween(start, end));
    }

    [Fact]
    public void FromMonths_ToNoda_ConvertsToWeeks()
    {
        var period = GaianPeriod.FromMonths(3);
        var noda = period.ToNoda();
        Assert.Equal(12, noda.Weeks); // 3 months × 4 weeks
    }

    [Fact]
    public void Add_TwoPeriods()
    {
        var a = GaianPeriod.FromYears(1);
        var b = GaianPeriod.FromMonths(3);
        var result = a + b;
        Assert.Equal(1, result.Years);
        Assert.Equal(3, result.Months);
    }

    [Fact]
    public void Negate_Period()
    {
        var period = GaianPeriod.FromYears(2) + GaianPeriod.FromMonths(3);
        var negated = -period;
        Assert.Equal(-2, negated.Years);
        Assert.Equal(-3, negated.Months);
    }

    [Fact]
    public void Zero_HasNoComponents()
    {
        var zero = GaianPeriod.Zero;
        Assert.Equal(0, zero.Years);
        Assert.Equal(0, zero.Months);
        Assert.Equal(0, zero.Days);
        Assert.False(zero.HasDateComponent);
        Assert.False(zero.HasTimeComponent);
    }

    [Fact]
    public void HasDateComponent_TrueForYears()
    {
        Assert.True(GaianPeriod.FromYears(1).HasDateComponent);
    }

    [Fact]
    public void HasTimeComponent_TrueForHours()
    {
        Assert.True(GaianPeriod.FromHours(5).HasTimeComponent);
    }

    [Fact]
    public void Equality()
    {
        var a = GaianPeriod.FromMonths(2);
        var b = GaianPeriod.FromMonths(2);
        Assert.Equal(a, b);
    }

    [Fact]
    public void ToDuration_ThrowsWithYearOrMonthComponents()
    {
        Assert.Throws<InvalidOperationException>(() => GaianPeriod.FromYears(1).ToDuration());
        Assert.Throws<InvalidOperationException>(() => GaianPeriod.FromMonths(1).ToDuration());
    }

    [Fact]
    public void ToDuration_WorksForDaysOnly()
    {
        var period = GaianPeriod.FromDays(7);
        var duration = period.ToDuration();
        Assert.Equal(Duration.FromDays(7), duration);
    }
}
