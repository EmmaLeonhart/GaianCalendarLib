# Gaian Calendar System

A custom calendar system built as NodaTime wrapper types for .NET, featuring 14 astrological month names, a 4-week month structure based on ISO week numbering, and a year offset of +10,000. Includes iCal export, a web date picker, a date-range CSV generator, and MediaWiki module exports.

---

## Table of Contents

1. [Calendar Overview](#calendar-overview)
2. [Project Structure](#project-structure)
3. [Requirements & Building](#requirements--building)
4. [Running the Main App](#running-the-main-app)
5. [Core Types](#core-types)
   - [GaianLocalDate](#gaianlocaldате)
   - [GaianLocalDateTime](#gaianlocaldatetime)
   - [GaianOffsetDateTime](#gaianoffsetdatetime)
   - [GaianZonedDateTime](#gaianzoneddatetime)
   - [GaianMonth](#gaianmonth)
   - [GaianPeriod](#gaianperiod)
6. [Date Formatting Reference](#date-formatting-reference)
7. [Parsing](#parsing)
8. [iCal Export](#ical-export)
9. [GaianDateRangeGenerator Tool](#gaian-date-range-generator-tool)
10. [Implementation Status](#implementation-status)
11. [Design Notes](#design-notes)

---

## Calendar Overview

### Month Structure

The Gaian calendar divides a year into weeks first, then groups weeks into months:

| # | Month Name  | ISO Weeks | Days |
|---|-------------|-----------|------|
| 1 | Sagittarius | 1–4       | 28   |
| 2 | Capricorn   | 5–8       | 28   |
| 3 | Aquarius    | 9–12      | 28   |
| 4 | Pisces      | 13–16     | 28   |
| 5 | Aries       | 17–20     | 28   |
| 6 | Taurus      | 21–24     | 28   |
| 7 | Gemini      | 25–28     | 28   |
| 8 | Cancer      | 29–32     | 28   |
| 9 | Leo         | 33–36     | 28   |
|10 | Virgo       | 37–40     | 28   |
|11 | Libra       | 41–44     | 28   |
|12 | Scorpio     | 45–48     | 28   |
|13 | Ophiuchus   | 49–52     | 28   |
|14 | **Horus**   | 53        | **7** (intercalary, leap years only) |

- **Regular year**: 13 months × 28 days = **364 days**
- **Leap year** (53 ISO weeks): 13 months + Horus = **371 days**
- A "Gaian leap year" occurs whenever the ISO week-year has 53 weeks (roughly every 5–6 years)

### Year Numbering

```
Gaian year = ISO week-year + 10,000
```

Examples:
- 2024 CE → **12024**
- 1 CE → **10001**
- 1 BCE → **9999**
- 3000 BCE → **7000**

### Weekday Symbols

Each weekday maps to a classical planetary symbol used in the `"W"` format pattern:

| Day       | Symbol | Planet  |
|-----------|--------|---------|
| Monday    | ☽      | Moon    |
| Tuesday   | ♂      | Mars    |
| Wednesday | ☿      | Mercury |
| Thursday  | ♃      | Jupiter |
| Friday    | ♀      | Venus   |
| Saturday  | ♄      | Saturn  |
| Sunday    | ☉      | Sun     |

### Month Symbols

Each month maps to its astrological Unicode symbol used in the `"MMM*"` format pattern:

♐ ♑ ♒ ♓ ♈ ♉ ♊ ♋ ♌ ♍ ♎ ♏ ⛎ 𓅃

---

## Project Structure

```
GaianCalendarLib/
│
├── GaianNodaTimeWrappers/            # Main library + demo app (.NET 8)
│   ├── GaianNodaTimeWrappers.csproj
│   ├── Program.cs                    # Demo: formatting, parsing, round-trip tests
│   ├── GaianTools.cs                 # Core conversion algorithms
│   ├── GaianDateFormat.cs            # Custom format pattern engine
│   ├── GaianLocalDate.cs             # Date wrapper (no time/zone)
│   ├── GaianLocalDateTime.cs         # Date + time wrapper
│   ├── GaianOffsetDateTime.cs        # Date + time + UTC offset wrapper
│   ├── GaianZonedDateTime.cs         # Date + time + timezone wrapper
│   ├── GaianMonth.cs                 # Month value type (1–14)
│   ├── GaianPeriod.cs                # Period/duration stub
│   ├── GaianCalendarExporter*.cs     # iCal exporters (several variants)
│   └── gaian_calendar_20XX.ics(.gz)  # Pre-generated iCal files (2020–2030)
│
├── GaianDateRangeGenerator/          # CLI tool for CSV/year-page generation
│   ├── GaianDateRangeGenerator.csproj
│   ├── Program.cs
│   └── *.csv                         # Pre-generated date range CSVs
│
├── GaianNodaTimeWrappers.sln         # Solution file
└── README.md
```

---

## Requirements & Building

- **.NET 8.0 SDK** or later
- NuGet packages (restored automatically):
  - `NodaTime` 3.2.2
  - `NodaTime.Serialization.JsonNet` 3.2.0
  - `NodaTime.Serialization.SystemTextJson` 1.3.0
  - `Ical.Net` 5.0.7

```bash
# Restore and build both projects
dotnet build GaianNodaTimeWrappers.sln
```

---

## Running the Main App

```bash
cd GaianNodaTimeWrappers

# Run the formatting/parsing demo
dotnet run

# Generate iCal files (one per year, 2020–2030)
dotnet run ical
```

Sample demo output:
```
=== Gaian Calendar Advanced Formatting Demo ===

=== Date Formatting Patterns ===
Default:        Aquarius 15, 12026
"MMMM d, yyyy": Aquarius 15, 12026
"MMM d, yy":    Aqu 15, 26
"M/d/yyyy":     3/15/12026
"ddd":          15th
"dddd":         Fifteenth
"MMM*":         ♒
"W":            ☽
"WWW":          Mon
"WWWW":         Monday
"DDD":          099

=== Parsing Demo ===
Parsed 'Aquarius 15, 12025' -> Aquarius 15, 12025
Parsed '3/15/12025' -> Aquarius 15, 12025
Parsed '12025-03-15' -> Aquarius 15, 12025
Parsed 'Aqu 15, 12025' -> Aquarius 15, 12025
```

---

## Core Types

All Gaian types are `readonly struct`s that wrap their NodaTime counterparts. They implement the same interfaces as the NodaTime equivalents (IEquatable, IComparable, IFormattable, IXmlSerializable, generic math operators) and support implicit conversion to and from the underlying NodaTime type.

### GaianLocalDate

A date without time or timezone, expressed in Gaian calendar terms.

```csharp
// Construction
var date = new GaianLocalDate(12025, 3, 15);    // Aquarius 15, 12025
var today = GaianLocalDate.Today;
var fromNoda = (GaianLocalDate)someLocalDate;   // implicit conversion

// Construction from ISO week components
var fromWeek = GaianLocalDate.FromWeekYearWeekAndDay(2025, 10, IsoDayOfWeek.Monday);

// From .NET / Julian
var fromDt = GaianLocalDate.FromDateTime(DateTime.Now);
var fromJd  = GaianLocalDate.FromJulianDay(2460000.5);

// Formatting
date.ToString()                         // "Aquarius 15, 12025"
date.ToString("MMMM d, yyyy", null)     // "Aquarius 15, 12025"
date.ToString("MMM* dd", null)          // "♒ 15"
date.ToString("W WWWW", null)           // "☽ Monday"
date.ToString("DDD", null)              // "099"

// Parsing
GaianLocalDate.Parse("Aquarius 15, 12025")
GaianLocalDate.Parse("Aqu 15, 12025")
GaianLocalDate.Parse("3/15/12025")
GaianLocalDate.Parse("12025-03-15")
GaianLocalDate.TryParse(input, out var result)

// Arithmetic
date.PlusDays(7)
date.PlusWeeks(2)
date.Next(IsoDayOfWeek.Friday)
date.Previous(IsoDayOfWeek.Monday)

// Conversion
date.ToDateTimeUnspecified()   // DateTime
date.ToDateOnly()              // DateOnly
date.ToJulianDay()             // double
(LocalDate)date                // NodaTime LocalDate (implicit)

// Combining with time
date.At(new LocalTime(14, 30))     // → GaianLocalDateTime
date.AtMidnight()                  // → GaianLocalDateTime

// Properties
date.Year       // int (Gaian year, e.g. 12025)
date.Month      // GaianMonth
date.Day        // int (1–28)
date.DayOfWeek  // IsoDayOfWeek
date.DayOfYear  // int (1–364 or 1–371)
```

**Implemented operators:** `==`, `!=`, `<`, `<=`, `>`, `>=`

### GaianLocalDateTime

Combines a Gaian date with a time-of-day.

```csharp
// Construction
var dt = new GaianLocalDateTime(12025, 3, 15, 14, 30);
var dt = new GaianLocalDateTime(12025, 3, 15, 14, 30, 45);
var dt = new GaianLocalDateTime(12025, 3, 15, 14, 30, 45, 123); // with ms
var now = GaianLocalDateTime.Now;

// Formatting
dt.ToString()                              // "Aquarius 15, 12025 14:30:45"
dt.ToString("MMMM d, yyyy HH:mm", null)   // "Aquarius 15, 12025 14:30"
dt.ToString("MMM* d h:mm tt", null)       // "♒ 15 2:30 PM"

// Time arithmetic
dt.PlusHours(3)
dt.PlusMinutes(45)
dt.PlusSeconds(30)
dt.PlusMilliseconds(500)
dt.PlusNanoseconds(1000)
dt.PlusTicks(100)
dt.PlusDays(1)
dt.PlusWeeks(2)
dt.Next(IsoDayOfWeek.Friday)
dt.Previous(IsoDayOfWeek.Monday)

// Timezone conversion
dt.InUtc()
dt.InZoneLeniently(DateTimeZoneProviders.Tzdb["America/New_York"])
dt.InZoneStrictly(zone)
dt.InZone(zone, resolver)
dt.WithOffset(Offset.FromHours(5))           // → GaianOffsetDateTime

// Properties
dt.Date        // GaianLocalDate
dt.TimeOfDay   // LocalTime
dt.Hour, dt.Minute, dt.Second, dt.Millisecond
dt.NanosecondOfDay, dt.TickOfDay
dt.Year, dt.Month, dt.Day, dt.DayOfWeek, dt.DayOfYear
```

### GaianOffsetDateTime

A Gaian date-time with a fixed UTC offset (no DST awareness).

```csharp
var odt = new GaianOffsetDateTime(12025, 3, 15, 14, 30, Offset.FromHours(5));
var odt = new GaianOffsetDateTime(12025, 3, 15, 14, 30, 45, Offset.FromHours(-8));

odt.PlusHours(2)
odt.ToDateTimeOffset()   // DateTimeOffset
odt.ToInstant()          // NodaTime Instant
```

### GaianZonedDateTime

A Gaian date-time in a named timezone with full DST support.

```csharp
var zdt = new GaianZonedDateTime(instant, DateTimeZoneProviders.Tzdb["America/New_York"]);
var zdt = new GaianZonedDateTime(12025, 3, 15, 14, 30, zone);

zdt.ToString()   // "Aquarius 15, 12025 14:30 [America/New_York] -05"
zdt.IsDaylightSavingTime()
zdt.WithZone(newZone)
zdt.ToInstant()
```

### GaianMonth

A value type representing a Gaian month (1–14).

```csharp
var month = new GaianMonth(3);       // Aquarius
var month = (GaianMonth)3;           // implicit from int

month.ToString()            // "Aquarius"
month.ToString("G", null)   // "Aquarius" (full name)
month.ToString("N", null)   // "3" (number)
month.ToString("NN", null)  // "03" (zero-padded)

GaianMonth.Parse("Aquarius")
GaianMonth.Parse("3")
GaianMonth.TryParse("Aqu", out var m)

month.Next()       // GaianMonth(4) = Pisces
month.Previous()   // GaianMonth(2) = Capricorn
```

### GaianPeriod

A skeleton wrapper around NodaTime's `Period`. All members currently throw `NotImplementedException` — placeholder for future month/year-based arithmetic.

---

## Date Formatting Reference

### Date Patterns

| Pattern | Description                        | Example          |
|---------|------------------------------------|------------------|
| `MMMM`  | Full month name                    | `Aquarius`       |
| `MMM`   | Abbreviated month name (3 chars)   | `Aqu`            |
| `MMM*`  | Astrological month symbol          | `♒`              |
| `MM`    | Month number, zero-padded          | `03`             |
| `M`     | Month number                       | `3`              |
| `dddd`  | Day of month in words              | `Fifteenth`      |
| `ddd`   | Day of month with ordinal suffix   | `15th`           |
| `dd`    | Day of month, zero-padded          | `15`             |
| `d`     | Day of month                       | `15`             |
| `WWWW`  | Full weekday name                  | `Monday`         |
| `WWW`   | Abbreviated weekday (3 chars)      | `Mon`            |
| `WW`    | Super-short weekday (2 chars)      | `Mo`             |
| `W`     | Planetary weekday symbol           | `☽`              |
| `yyyy`  | Full Gaian year                    | `12025`          |
| `yy`    | Last two digits of Gaian year      | `25`             |
| `DDD`   | Day of year, zero-padded to 3      | `099`            |

### Time Patterns (GaianLocalDateTime only)

| Pattern | Description              | Example |
|---------|--------------------------|---------|
| `HH`    | 24-hour, zero-padded     | `14`    |
| `H`     | 24-hour                  | `14`    |
| `hh`    | 12-hour, zero-padded     | `02`    |
| `h`     | 12-hour                  | `2`     |
| `mm`    | Minute, zero-padded      | `05`    |
| `ss`    | Second, zero-padded      | `07`    |
| `fff`   | Milliseconds             | `345`   |
| `tt`    | AM/PM designator         | `PM`    |

### Examples

```csharp
var date = new GaianLocalDate(12025, 3, 15);  // Aquarius 15, 12025 (a Monday)

date.ToString("MMMM d, yyyy", null)           // "Aquarius 15, 12025"
date.ToString("MMM d, yy", null)              // "Aqu 15, 25"
date.ToString("M/d/yyyy", null)               // "3/15/12025"
date.ToString("ddd", null)                    // "15th"
date.ToString("dddd", null)                   // "Fifteenth"
date.ToString("MMM*", null)                   // "♒"
date.ToString("W WWWW, MMMM ddd, yyyy", null) // "☽ Monday, Aquarius 15th, 12025"

var dt = new GaianLocalDateTime(12025, 3, 15, 14, 30, 0);
dt.ToString("MMMM d, yyyy HH:mm", null)       // "Aquarius 15, 12025 14:30"
dt.ToString("MMM* d h:mm tt", null)           // "♒ 15 2:30 PM"
```

---

## Parsing

All date types support parsing from three input formats:

```csharp
// Named format (full or abbreviated month name)
GaianLocalDate.Parse("Aquarius 15, 12025")
GaianLocalDate.Parse("Aqu 15, 12025")

// Numeric format: month/day/year
GaianLocalDate.Parse("3/15/12025")

// ISO format: year-month-day
GaianLocalDate.Parse("12025-03-15")

// Safe parsing
GaianLocalDate.TryParse(input, out var result)

// DateTime parsing
GaianLocalDateTime.Parse("12025-03-15 14:30:00")
GaianLocalDateTime.TryParse(input, out var result)
```

---

## iCal Export

Annotates every calendar day with its Gaian date, compatible with Google Calendar, Apple Calendar, and Outlook.

```bash
cd GaianNodaTimeWrappers
dotnet run ical
```

Produces 11 files for years 2020–2030 (`gaian_calendar_YYYY.ics` + `.gz`). Pre-generated files are included in the repository.

**Importing into Google Calendar:** Settings → Import & Export → import each `.ics` file separately.

---

## GaianDateRangeGenerator Tool

CLI tool for generating year summaries and CSV date tables.

```bash
cd GaianDateRangeGenerator

# Show info for a specific Gaian year
dotnet run 12024

# Generate a CSV of year metadata
dotnet run csv <start> <end> [filename]
dotnet run csv 3 12100
```

### Year Page Output

```
=== Gaian Year 12024 ===
Corresponds to: 2024 CE
Duration: 2023-12-25 to 2024-12-29
Total days: 371 (intercalary year)
Intercalary month (Horus): Yes

Month breakdown:
 1. Sagittarius  | 2023-12-25 to 2024-01-21
 2. Capricorn    | 2024-01-22 to 2024-02-18
...
14. Horus        | 2024-12-23 to 2024-12-29 (intercalary)
```

### CSV Format

```csv
GaianYear,StartDate,GregorianLeapYear,GaianLeapYear,DaysInYear
12024,"December 25, 2023",true,true,371
12025,"December 30, 2024",false,false,364
```

---

## Implementation Status

### GaianLocalDate

| Feature | Status |
|---------|--------|
| Construction from (year, month, day) | ✅ |
| `Today`, `FromDateTime()`, `FromDateOnly()` | ✅ |
| `FromWeekYearWeekAndDay()` | ✅ |
| `FromJulianDay()` / `ToJulianDay()` | ✅ |
| Formatting and parsing | ✅ |
| `PlusDays()`, `PlusWeeks()`, `Next()`, `Previous()` | ✅ |
| Comparison operators | ✅ |
| `At()`, `AtMidnight()`, `WithOffset()` | ✅ |
| `PlusMonths()`, `PlusYears()`, `Plus(Period)` | ❌ |
| XML serialization | ❌ |

### GaianLocalDateTime

| Feature | Status |
|---------|--------|
| Construction, `Now`, `FromDateTime()` | ✅ |
| Formatting and parsing | ✅ |
| All time-unit Plus methods | ✅ |
| `InUtc()`, `InZone*()`, `WithOffset()` | ✅ |
| `Max()`, `Min()`, comparison operators | ✅ |
| `PlusMonths()`, `PlusYears()`, `Plus(Period)` | ❌ |

### GaianOffsetDateTime / GaianZonedDateTime

| Feature | Status |
|---------|--------|
| Construction and duration arithmetic | ✅ |
| Conversion methods and formatting | ✅ |
| Period arithmetic | ❌ |

### GaianMonth

| Feature | Status |
|---------|--------|
| All formatting, parsing, navigation, operators | ✅ |

### GaianPeriod

All members are stubs. Exists as an API placeholder.

---

## Design Notes

### ISO Week-Year Math

```
weekYear   = IsoRules.GetWeekYear(date)
weekOfYear = IsoRules.GetWeekOfWeekYear(date)   // 1..52 or 1..53
dayOfWeek  = (int)date.DayOfWeek                // Mon=1 .. Sun=7

month      = (weekOfYear - 1) / 4 + 1           // 1..14
dayOfMonth = (weekOfYear - 1) % 4 * 7 + dayOfWeek  // 1..28
gaianYear  = weekYear + 10000
```

Reverse (Gaian → NodaTime):
```
weekOfYear = (month - 1) * 4 + (day - 1) / 7 + 1
dayOfWeek  = (day - 1) % 7 + 1
date       = IsoRules.GetLocalDate(isoWeekYear, weekOfYear, dayOfWeek)
```

### Relationship to NodaTime

Every Gaian type stores a single NodaTime value internally and delegates all arithmetic and timezone operations to it. The Gaian layer adds:
1. Alternative string representation (Gaian month names, year offset)
2. Construction from Gaian (year, month, day) components
3. Custom format pattern engine (`GaianDateFormat`)

Gaian types are fully interoperable with NodaTime via implicit conversions.
