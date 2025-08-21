# Gaian Calendar System - NodaTime Wrapper Experiments

This project implements a custom calendar system called the "Gaian Calendar" built as wrapper types around NodaTime's date/time structures. The system creates an alternative calendar with astrological month names and a modified year numbering system.

## Calendar System Overview

### Month Structure
The Gaian calendar uses a **4-week month system** based on ISO week numbering:
- **13 regular months** of 4 weeks each (28 days)
- **1 intercalary month** ("Horus") for the 53rd week in leap years
- Each month contains exactly 28 days (4 weeks × 7 days)

### Month Names (Astrological)
1. **Sagittarius** (weeks 1-4)
2. **Capricorn** (weeks 5-8) 
3. **Aquarius** (weeks 9-12)
4. **Pisces** (weeks 13-16)
5. **Aries** (weeks 17-20)
6. **Taurus** (weeks 21-24)
7. **Gemini** (weeks 25-28)
8. **Cancer** (weeks 29-32)
9. **Leo** (weeks 33-36)
10. **Virgo** (weeks 37-40)
11. **Libra** (weeks 41-44)
12. **Scorpio** (weeks 45-48)
13. **Ophiuchus** (weeks 49-52)
14. **Horus** (week 53, intercalary)

### Year Numbering
- Gaian years = ISO week-year + 10,000
- Example: 2024 → 12024 in Gaian calendar

### Date Calculation Logic
The system converts regular dates to Gaian format using:
- ISO week-year rules for consistent week boundaries
- Zero-based math: `(weekOfYear - 1) / 4` for month calculation
- Day within month: `(weekInMonth * 7) + dayOfWeek`

## Implementation Structure

### Core Components

#### `GaianTools.cs` (GaianTools:10-127)
Central utility class containing date conversion algorithms:
- `GaiaDateString()`: Converts LocalDate to Gaian format string
- `GetMonth()`, `GetDay()`, `GetYear()`: Extract Gaian calendar components
- Uses ISO week-year rules for consistent calculations

#### `GaianLocalDate.cs` (GaianLocalDate:14-185)
Wrapper around NodaTime's `LocalDate` with Gaian calendar semantics:
- Implements standard date interfaces (IEquatable, IComparable, etc.)
- Provides implicit conversions to/from NodaTime LocalDate
- Most methods throw NotImplementedException (skeleton implementation)
- Custom ToString() returns Gaian-formatted date strings

#### `GaianLocalDateTime.cs` (GaianLocalDateTime:23-284)
Wrapper around NodaTime's `LocalDateTime`:
- Combines Gaian date formatting with standard time representation
- Custom ToString() combines Gaian date with time (e.g., "Aquarius 15, 12024 14:30")
- Skeleton implementation with most methods not yet implemented

#### `GaianMonth.cs` (GaianMonth:5-134)
Dedicated month type supporting both numeric (1-14) and name-based operations:
- Supports parsing month names and numbers
- Localization-ready design with culture-aware formatting
- Range validation (1-14 for the 13+1 month system)

### Example Output
Running the program converts current DateTime to Gaian format:
```
Aquarius 15, 12024 14:30
```

## Current Status
- **Core date conversion logic**: ✅ Implemented
- **Basic wrapper types**: ✅ Created
- **String formatting**: ✅ Working
- **Full API implementation**: ❌ Most methods are stubs
- **Arithmetic operations**: ❌ Not implemented
- **Parsing from strings**: ❌ Limited implementation

This is an experimental/proof-of-concept implementation exploring alternative calendar systems using NodaTime's robust date/time infrastructure.