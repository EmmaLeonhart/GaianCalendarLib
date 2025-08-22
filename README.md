# Gaian Calendar System

A custom calendar system built as NodaTime wrappers featuring astrological month names and a 4-week month structure based on ISO week numbering.

## Main Objects

The Gaian Calendar provides four primary wrapper classes that mirror NodaTime's date/time types while presenting dates in the Gaian calendar format:

### GaianLocalDate
Represents a date without time or timezone information in the Gaian calendar.

**What you can do:**
- Create dates using Gaian components: `new GaianLocalDate(12025, 3, 15)` (Aquarius 15, 12025)
- Convert to/from regular .NET DateTime and NodaTime LocalDate
- Format dates with custom patterns (e.g., `"MMMM d, yyyy"` → "Aquarius 15, 12025")
- Parse from multiple formats: named ("Aquarius 15, 12025"), numeric ("3/15/12025"), or ISO ("12025-03-15")
- Perform day-based arithmetic: `date.PlusDays(7)`, `date.PlusWeeks(2)`
- Navigate to next/previous weekdays: `date.Next(IsoDayOfWeek.Friday)`
- Convert to Julian day numbers for astronomical calculations

### GaianLocalDateTime  
Combines a Gaian date with time-of-day information.

**What you can do:**
- Create with date and time: `new GaianLocalDateTime(12025, 3, 15, 14, 30, 45)`
- All date operations from GaianLocalDate plus time manipulation
- Time-based arithmetic: `PlusHours()`, `PlusMinutes()`, `PlusSeconds()`, `PlusMilliseconds()`
- Format with time patterns: `"MMMM d, yyyy HH:mm"` → "Aquarius 15, 12025 14:30"
- Convert to timezone-aware types (GaianZonedDateTime, GaianOffsetDateTime)
- Parse date-time combinations

### GaianOffsetDateTime
A Gaian date-time with a fixed UTC offset (like "+05:30" or "-08:00").

**What you can do:**
- Create with specific offset: `new GaianOffsetDateTime(year, month, day, hour, minute, offset)`
- All datetime operations plus offset-aware conversions
- Convert to .NET DateTimeOffset for interoperability
- Handle time zones with fixed offsets (no DST transitions)

### GaianZonedDateTime
A Gaian date-time in a specific timezone that handles DST transitions.

**What you can do:**
- Create in specific timezone: `gaianDateTime.InZone(DateTimeZone.ForId("America/New_York"))`
- All datetime operations with full timezone support
- Handle daylight saving time transitions automatically
- Convert between timezones while maintaining Gaian date formatting

## Calendar System

**Month Structure:** 13 months of 28 days each (4 weeks), plus 1 intercalary month (Horus) in leap years
**Month Names:** Sagittarius, Capricorn, Aquarius, Pisces, Aries, Taurus, Gemini, Cancer, Leo, Virgo, Libra, Scorpio, Ophiuchus, Horus
**Year Numbering:** Gaian year = ISO week-year + 10,000 (e.g., 2025 → 12025)

## Parsing Capabilities

All date types support parsing from multiple input formats:

```csharp
// Named format with full month names
GaianLocalDate.Parse("Aquarius 15, 12025")

// Abbreviated month names  
GaianLocalDate.Parse("Aqu 15, 12025")

// Numeric format (month/day/year)
GaianLocalDate.Parse("3/15/12025")

// ISO format (year-month-day)
GaianLocalDate.Parse("12025-03-15")

// Safe parsing with TryParse
if (GaianLocalDate.TryParse(input, out var result))
{
    Console.WriteLine($"Parsed: {result}");
}
```

## Advanced Formatting

Comprehensive format patterns inspired by traditional date formatting:

```csharp
var date = new GaianLocalDate(12025, 3, 15);

// Month patterns
date.ToString("MMMM")    // "Aquarius" (full name)
date.ToString("MMM")     // "Aqu" (abbreviated)
date.ToString("MMM*")    // "♒" (astrological symbol)
date.ToString("MM")      // "03" (zero-padded number)
date.ToString("M")       // "3" (number)

// Day patterns  
date.ToString("dd")      // "15" (zero-padded)
date.ToString("d")       // "15" (number)
date.ToString("ddd")     // "15th" (ordinal)
date.ToString("dddd")    // "Fifteenth" (word form)

// Weekday patterns
date.ToString("WWWW")    // "Monday" (full name)
date.ToString("WWW")     // "Mon" (abbreviated)
date.ToString("W")       // "☽" (planetary symbol)

// Year patterns
date.ToString("yyyy")    // "12025" (full year)
date.ToString("yy")      // "25" (two digits)

// Combined patterns
date.ToString("MMMM d, yyyy")           // "Aquarius 15, 12025"
date.ToString("MMM* dd, yy")            // "♒ 15, 25"
date.ToString("W WWW d")                // "☽ Mon 15"
```

## Common Methods

All four wrapper classes share these capabilities:

### Conversion & Interoperability
- Implicit conversion to/from corresponding NodaTime types
- `FromDateTime()` - Create from .NET DateTime
- `ToDateTimeUnspecified()` - Convert to .NET DateTime

### Comparison & Equality
- Full comparison support (`==`, `!=`, `<`, `>`, `<=`, `>=`)
- `Equals()` and `GetHashCode()` implementation
- `CompareTo()` for sorting

### Formatting & Parsing
- `ToString()` - Default Gaian format
- `ToString(pattern, culture)` - Custom formatting with culture support
- `Parse()` and `TryParse()` - Multiple input format support

### Basic Arithmetic
- `PlusDays()`, `PlusWeeks()` - Duration-based arithmetic
- `Next()`, `Previous()` - Weekday navigation

## Class-Specific Methods

### GaianLocalDate Only
- `At(LocalTime)` - Combine with time to create GaianLocalDateTime
- `AtMidnight()` - Create datetime at 00:00
- `ToJulianDay()` - Convert to Julian day number
- `FromJulianDay()` - Create from Julian day number

### GaianLocalDateTime Only  
- `PlusHours()`, `PlusMinutes()`, `PlusSeconds()`, `PlusMilliseconds()`, `PlusTicks()`, `PlusNanoseconds()`
- `InUtc()` - Convert to UTC timezone
- `InZone()`, `InZoneLeniently()`, `InZoneStrictly()` - Convert to specific timezone
- `WithOffset()` - Add UTC offset

### GaianOffsetDateTime Only
- `ToDateTimeOffset()` - Convert to .NET DateTimeOffset
- Offset-aware arithmetic and comparisons

### GaianZonedDateTime Only
- Full timezone transition handling
- DST-aware arithmetic

## Current Limitations & TODOs

⚠️ **Important:** The current implementation is limited to dates within NodaTime's supported range. Future versions will implement support for dates outside this range to fully utilize the Gaian calendar's extended year numbering system.

**Not Yet Implemented:**
- Period-based arithmetic (months/years) - only duration-based arithmetic works
- Some advanced NodaTime features (custom calendars, some static methods)
- Dates outside NodaTime's range (future enhancement planned)

**Fully Working:**
- All formatting and parsing operations
- Duration-based arithmetic (days, hours, minutes, etc.)
- Timezone conversions
- Comparison and equality operations
- Julian day conversions

## Example Usage

```csharp
// Create a Gaian date
var date = new GaianLocalDate(12025, 3, 15); // Aquarius 15, 12025

// Format in different ways
Console.WriteLine(date);                              // "Aquarius 15, 12025"
Console.WriteLine(date.ToString("MMM* d, yy"));      // "♒ 15, 25"
Console.WriteLine(date.ToString("WWWW, MMMM d"));    // "Monday, Aquarius 15"

// Parse from different formats
var parsed1 = GaianLocalDate.Parse("Aquarius 15, 12025");
var parsed2 = GaianLocalDate.Parse("3/15/12025");
var parsed3 = GaianLocalDate.Parse("12025-03-15");

// Date arithmetic
var nextWeek = date.PlusWeeks(1);
var nextFriday = date.Next(IsoDayOfWeek.Friday);

// Create datetime and add time
var dateTime = date.At(new LocalTime(14, 30));
var futureTime = dateTime.PlusHours(3).PlusMinutes(45);

// Timezone operations
var utc = dateTime.InUtc();
var eastern = dateTime.InZone(DateTimeZone.ForId("America/New_York"));
```