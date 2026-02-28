# NodaTime.GaianCalendar – TODO

**Package:** `NodaTime.GaianCalendar`
**Current version:** `0.1.0-preview`
**Target:** `0.1.0` stable once all high-priority stubs are filled

---

## ✅ Done

### Core utilities
- [x] `GaianTools` – date conversion, parsing (named/numeric/ISO), Julian day support
- [x] `GaianDateFormat` – custom format pattern engine (`M`, `d`, `W`, `y`, `D`, `H`, `h`, `m`, `s`, `f`, `t` tokens, astrological symbols)
- [x] `GaianMonth` – complete: construction, formatting (G/N/NN), parsing, navigation, all operators

### GaianLocalDate
- [x] Construction from `(year, month, day)` with leap-year validation
- [x] `Today`, `FromDateTime()`, `FromDateOnly()`
- [x] `FromWeekYearWeekAndDay()`
- [x] `FromJulianDay()` / `ToJulianDay()`
- [x] `ToString()` and `ToString(format, culture)`
- [x] `Parse()` / `TryParse()` (named, numeric, ISO)
- [x] `PlusDays()`, `PlusWeeks()`
- [x] `Next(IsoDayOfWeek)`, `Previous(IsoDayOfWeek)`
- [x] `At(LocalTime)`, `AtMidnight()`, `WithOffset()`
- [x] `ToDateTimeUnspecified()`, `ToDateOnly()`
- [x] Comparison operators (`==`, `!=`, `<`, `<=`, `>`, `>=`), `IComparable`
- [x] Implicit conversions to/from `LocalDate`
- [x] `IEquatable`, `IFormattable`

### GaianLocalDateTime
- [x] Construction from Gaian components (all overloads: h/m, h/m/s, h/m/s/ms)
- [x] `Now`, `FromDateTime()`, `FromWeekYearWeekAndDay()`
- [x] `ToString()` and `ToString(format, culture)`
- [x] `Parse()` / `TryParse()`
- [x] `PlusDays()`, `PlusWeeks()`, `PlusHours()`, `PlusMinutes()`, `PlusSeconds()`, `PlusMilliseconds()`, `PlusNanoseconds()`, `PlusTicks()`
- [x] `InUtc()`, `InZoneLeniently()`, `InZoneStrictly()`, `InZone(zone, resolver)`, `WithOffset()`
- [x] `Next()`, `Previous()`
- [x] `Max()`, `Min()`
- [x] Comparison operators, `IComparable`
- [x] Implicit conversions to/from `LocalDateTime`

### GaianOffsetDateTime
- [x] Construction from Gaian components + `Offset`
- [x] Duration arithmetic: `Plus()`, `Minus()`, all `Plus*()` variants
- [x] `ToInstant()`, `ToDateTimeOffset()`, `ToOffsetDate()`, `ToOffsetTime()`
- [x] `ToString()` and `ToString(format, culture)`
- [x] `operator +`, `operator -`, `==`, `!=`

### GaianZonedDateTime
- [x] Construction from `Instant`+zone and from Gaian components+zone
- [x] Duration arithmetic: `Plus()`, `Minus()`, all `Plus*()` variants
- [x] `ToInstant()`, `ToDateTimeOffset()`, `ToOffsetDateTime()`
- [x] `IsDaylightSavingTime()`, `GetZoneInterval()`, `WithZone()`, `FromDateTimeOffset()`
- [x] `ToString()` and `ToString(format, culture)`
- [x] `operator +`, `operator -`, `==`, `!=`

### Extras
- [x] iCal exporter (Google/Apple Calendar compatible, 2020–2030 pre-generated)
- [x] `GaianDateRangeGenerator` CLI (year info pages, CSV output)
- [x] `.csproj` NuGet metadata (package ID, version, author, description, tags, README, symbols)

---

## ✅ Implemented (previously stubs)

### High priority — Period arithmetic (all done)

- [x] **`GaianPeriod`** — full implementation wrapping NodaTime `Period`
- [x] **`GaianLocalDate.PlusMonths(int)`** — n Gaian months = n × 4 weeks
- [x] **`GaianLocalDate.PlusYears(int)`** — roll-forward semantics for Horus → Sagittarius
- [x] **`GaianLocalDate.Plus(Period)` / `Minus(Period)`** — delegate to `LocalDate.Plus/Minus`
- [x] **`GaianLocalDate` Period operators** — `operator+(date, Period)`, `operator-(date, Period)`, `operator-(lhs, rhs)` → Period
- [x] **`GaianLocalDate.Add()/Subtract()` statics** — delegate to Plus/Minus
- [x] **`GaianLocalDateTime.PlusMonths(int)`**
- [x] **`GaianLocalDateTime.PlusYears(int)`**
- [x] **`GaianLocalDateTime.Plus(Period)` / `Minus(Period)` / operators**

### Medium priority (all done)

- [x] `GaianLocalDate.Deconstruct(out year, out month, out day)`
- [x] `GaianLocalDate.With(Func<GaianLocalDate, GaianLocalDate>)`
- [x] `GaianLocalDate.Min(x, y)` / `Max(x, y)`
- [x] `GaianLocalDate.Era`
- [x] `GaianLocalDate.AtStartOfDayInZone(DateTimeZone)`
- [x] `GaianLocalDate` `operator+(date, LocalTime)`
- [x] `GaianLocalDateTime.Deconstruct()`
- [x] `GaianLocalDateTime.With(Func<LocalDate, LocalDate>)` / `With(Func<LocalTime, LocalTime>)`
- [x] `GaianOffsetDateTime.With()`, `WithCalendar()`, `WithOffset()`
- [x] `GaianZonedDateTime.Deconstruct()`, `WithCalendar()`

### Low priority / future work

- [ ] XML serialization (`IXmlSerializable`) on all types
- [ ] Unit test project (`GaianNodaTimeWrappers.Tests`)
  - Round-trip: every day in 2020–2030 → GaianLocalDate → back
  - Leap year boundary (week 53 clamping)
  - PlusMonths/PlusYears edge cases
  - Parsing all three input formats
  - GaianPeriod.Between vs manual calculation
- [ ] JSON serialization (NodaTime.Serialization refs already in .csproj)
- [ ] Clean up duplicate `using` directives in several files
- [ ] Remove commented-out dead code in Program.cs

---

## 📦 Publishing

- [x] `.csproj` NuGet metadata configured
- [ ] Update version: `0.1.0-preview` → `0.1.0` once stubs complete
- [ ] Create GitHub repo: `Emma-Leonhart/GaianCalendarLib` (referenced in .csproj)
- [ ] Tag `v0.1.0-preview` in git
- [ ] `dotnet pack GaianNodaTimeWrappers -c Release`
- [ ] `dotnet nuget push *.nupkg -s https://api.nuget.org/v3/index.json --api-key <KEY>`
- [ ] Create GitHub release

---

## ❓ Open Questions / Uncertainty

### What even is NodaTime Period?

**Short answer:** `Period` is NodaTime's version of "a span of time expressed in calendar units" — like "3 months and 5 days" — as opposed to a fixed duration in milliseconds.

**Key distinction from `Duration`:**
- `Duration` = fixed number of nanoseconds (like a `TimeSpan`). Easy to add to any instant.
- `Period` = "3 months + 2 days" — the actual elapsed time depends on *which* month you're counting from (February + 1 month ≠ August + 1 month in terms of days).

**Why this matters for Gaian calendar:**
- Gaian months are *always* exactly 28 days (unlike ISO months which are 28–31 days), so `GaianPeriod.FromMonths(1)` is unambiguous — it always means 28 days.
- Gaian years have 364 *or* 371 days depending on whether the ISO week-year has 52 or 53 weeks. So `PlusYears(1)` navigates by ISO week-year, not by counting 365 days.
- `GaianPeriod` wraps a NodaTime `Period` for the sub-monthly units (weeks, days, hours, minutes, seconds, etc.) and adds its own `Years` and `Months` fields with Gaian semantics.

**When to use what:**
```
date + NodaTime.Period.FromDays(7)   → adds exactly 7 days (ISO)
date.PlusDays(7)                     → same thing (delegates to NodaTime)
date + NodaTime.Period.FromMonths(1) → adds 1 ISO calendar month (28-31 days)
date.PlusMonths(1)                   → adds 28 days (1 Gaian month, always)
date.PlusYears(1)                    → advances by 1 ISO week-year (with week-53 clamping)
GaianPeriod.Between(a, b)            → difference in Gaian years/months/days (approx)
```

**Still uncertain:** Whether `GaianPeriod` is actually useful to end users, or whether just having clean `PlusMonths`/`PlusYears` methods is enough. The whole `GaianPeriod` type exists to mirror NodaTime's API surface — but it may be over-engineering for this use case.

---

## 🏗️ Architecture Notes

### Why wrap NodaTime?
The Gaian calendar doesn't fit NodaTime's `CalendarSystem` extension model cleanly (ISO week-year boundaries don't align with gregorian month boundaries). Wrapper types are simpler and keep full NodaTime interop via implicit conversions.

### Month/Year arithmetic design
- **`PlusMonths(n)`** = `PlusWeeks(n * 4)` — a Gaian month is always exactly 28 days
- **`PlusYears(n)`** = advance the ISO week-year component; if the date is in Horus (week 53) and the target year has no week 53, roll forward to week 1 of the year *after* the target (skip the non-leap year). Example: Horus 2, 12026 + 1 year = Sagittarius 2, 12028 (12027 is skipped because it has no week 53)
- **`GaianPeriod`** stores Gaian years and months as separate int fields; all sub-monthly units delegate to `NodaTime.Period`
- **`operator +(GaianLocalDate, Period)`** uses ISO arithmetic (NodaTime semantics), not Gaian month arithmetic — this is intentional and consistent with how NodaTime handles calendar-specific vs generic arithmetic

### Leap years
A Gaian leap year is any ISO week-year with 53 weeks (roughly every 5–6 years: 2015, 2020, 2026, 2032…). Check with:
```csharp
WeekYearRules.Iso.GetWeeksInWeekYear(isoYear) == 53
```
