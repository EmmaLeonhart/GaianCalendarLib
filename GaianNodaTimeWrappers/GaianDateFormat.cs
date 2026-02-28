using System;
using System.Globalization;
using System.Text;
using NodaTime;

namespace Gaian
{
    /// <summary>
    /// Advanced formatting for Gaian calendar dates and times.
    /// Inspired by StarDate's formatting system but simplified for Gaian calendar.
    /// </summary>
    public static class GaianDateFormat
    {
        /*
         Custom format patterns for Gaian calendar:
         
         Patterns   Description                           Example
         =========  ===================================== ========
            "M"     Month number w/o leading zero         3
            "MM"    Month number with leading zero        03
            "MMM"   Short month name                      Aqu
            "MMMM"  Full month name                       Aquarius
            "MMM*"  Month symbol                          ♒ (if symbols defined)
            
            "d"     Day w/o leading zero                  5
            "dd"    Day with leading zero                 05
            "ddd"   Ordinal day                          5th
            "dddd"  Ordinal day full                     Fifth
            
            "W"     Weekday symbol                        ☽
            "WW"    Super short weekday                   Mo
            "WWW"   Abbreviated weekday                   Mon
            "WWWW"  Full weekday name                     Monday
            
            "y"     Two digit year w/o leading zero       25
            "yy"    Two digit year with leading zero      25
            "yyyy"  Full year                            12025
            "yyyyy" Five digit year                      12025
            
            "DDD"   Day of year (zero-padded)            071
            
            Time patterns (for datetime):
            "h"     Hour (12-hour) w/o leading zero       3
            "hh"    Hour (12-hour) with leading zero      03
            "H"     Hour (24-hour) w/o leading zero       15
            "HH"    Hour (24-hour) with leading zero      15
            "m"     Minute w/o leading zero               5
            "mm"    Minute with leading zero              05
            "s"     Second w/o leading zero               7
            "ss"    Second with leading zero              07
            "f"     Tenths of second                      3
            "ff"    Hundredths of second                  34
            "fff"   Milliseconds                          345
            "t"     First char of AM/PM                   A
            "tt"    AM/PM designator                      AM
         */

        private static readonly string[] MonthSymbols = 
        {
            "♐", "♑", "♒", "♓", "♈", "♉", "♊", "♋", 
            "♌", "♍", "♎", "♏", "⛎", "𓅃"  // Horus symbol for month 14
        };

        private static readonly string[] OrdinalSuffixes = 
        {
            "th", "st", "nd", "rd", "th", "th", "th", "th", "th", "th",
            "th", "th", "th", "th", "th", "th", "th", "th", "th", "th",
            "th", "st", "nd", "rd", "th", "th", "th", "th"
        };

        private static readonly string[] NumberWords = 
        {
            "Zeroth", "First", "Second", "Third", "Fourth", "Fifth", "Sixth", "Seventh",
            "Eighth", "Ninth", "Tenth", "Eleventh", "Twelfth", "Thirteenth", "Fourteenth",
            "Fifteenth", "Sixteenth", "Seventeenth", "Eighteenth", "Nineteenth", "Twentieth",
            "Twenty-First", "Twenty-Second", "Twenty-Third", "Twenty-Fourth", "Twenty-Fifth",
            "Twenty-Sixth", "Twenty-Seventh", "Twenty-Eighth"
        };

        public static string Format(LocalDate date, string format, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(format))
                return GaianTools.GaiaDateString(date);

            var gaianMonth = GaianTools.GetMonth(date);
            var gaianDay = GaianTools.GetDay(date);
            var gaianYear = GaianTools.GetYear(date);
            var gaianDayOfYear = GaianTools.GetDayOfYear(date);
            var dayOfWeek = date.DayOfWeek;
            var monthName = gaianMonth.ToString("G", culture);
            var monthAbbr = monthName.Length >= 3 ? monthName.Substring(0, 3) : monthName;

            // Token table: ordered longest-first so greedy matching works correctly.
            // Scanning the format string left-to-right ensures replacement values never
            // accidentally match other tokens (fixes the "Friday → Frida26" bug).
            var tokens = new (string Token, string Value)[]
            {
                ("MMMMM", monthName),
                ("MMMM",  monthName),
                ("MMM*",  MonthSymbols[gaianMonth.Value - 1]),
                ("MMM",   monthAbbr),
                ("MM",    gaianMonth.ToString("NN", culture)),
                ("M",     gaianMonth.ToString("N", culture)),
                ("dddd",  NumberWords[Math.Min(gaianDay, NumberWords.Length - 1)]),
                ("ddd",   gaianDay + OrdinalSuffixes[Math.Min(gaianDay, OrdinalSuffixes.Length - 1)]),
                ("dd",    gaianDay.ToString("00", culture)),
                ("d",     gaianDay.ToString("0", culture)),
                ("WWWW",  dayOfWeek.ToString()),
                ("WWW",   dayOfWeek.ToString().Substring(0, 3)),
                ("WW",    dayOfWeek.ToString().Substring(0, 2)),
                ("W",     GetDaySymbol(dayOfWeek)),
                ("yyyyy", gaianYear.ToString("00000", culture)),
                ("yyyy",  gaianYear.ToString("0000", culture)),
                ("yy",    (gaianYear % 100).ToString("00", culture)),
                ("y",     (gaianYear % 100).ToString("0", culture)),
                ("DDD",   gaianDayOfYear.ToString("000", culture)),
            };

            return ScanAndReplace(format, tokens);
        }

        public static string Format(LocalDateTime dateTime, string format, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(format))
            {
                var gdate = new GaianLocalDate(dateTime.Date);
                return $"{gdate.ToString()} {dateTime.TimeOfDay:HH:mm}";
            }

            var time = dateTime.TimeOfDay;
            var gaianMonth = GaianTools.GetMonth(dateTime.Date);
            var gaianDay = GaianTools.GetDay(dateTime.Date);
            var gaianYear = GaianTools.GetYear(dateTime.Date);
            var gaianDayOfYear = GaianTools.GetDayOfYear(dateTime.Date);
            var dayOfWeek = dateTime.Date.DayOfWeek;
            var monthName = gaianMonth.ToString("G", culture);
            var monthAbbr = monthName.Length >= 3 ? monthName.Substring(0, 3) : monthName;
            int hour12 = time.Hour % 12 == 0 ? 12 : time.Hour % 12;

            var tokens = new (string Token, string Value)[]
            {
                ("MMMMM", monthName),
                ("MMMM",  monthName),
                ("MMM*",  MonthSymbols[gaianMonth.Value - 1]),
                ("MMM",   monthAbbr),
                ("MM",    gaianMonth.ToString("NN", culture)),
                ("M",     gaianMonth.ToString("N", culture)),
                ("dddd",  NumberWords[Math.Min(gaianDay, NumberWords.Length - 1)]),
                ("ddd",   gaianDay + OrdinalSuffixes[Math.Min(gaianDay, OrdinalSuffixes.Length - 1)]),
                ("dd",    gaianDay.ToString("00", culture)),
                ("d",     gaianDay.ToString("0", culture)),
                ("WWWW",  dayOfWeek.ToString()),
                ("WWW",   dayOfWeek.ToString().Substring(0, 3)),
                ("WW",    dayOfWeek.ToString().Substring(0, 2)),
                ("W",     GetDaySymbol(dayOfWeek)),
                ("yyyyy", gaianYear.ToString("00000", culture)),
                ("yyyy",  gaianYear.ToString("0000", culture)),
                ("yy",    (gaianYear % 100).ToString("00", culture)),
                ("y",     (gaianYear % 100).ToString("0", culture)),
                ("DDD",   gaianDayOfYear.ToString("000", culture)),
                // Time tokens
                ("hh",    hour12.ToString("00", culture)),
                ("h",     hour12.ToString("0", culture)),
                ("HH",    time.Hour.ToString("00", culture)),
                ("H",     time.Hour.ToString("0", culture)),
                ("mm",    time.Minute.ToString("00", culture)),
                ("m",     time.Minute.ToString("0", culture)),
                ("ss",    time.Second.ToString("00", culture)),
                ("s",     time.Second.ToString("0", culture)),
                ("fff",   time.Millisecond.ToString("000", culture)),
                ("ff",    (time.Millisecond / 10).ToString("00", culture)),
                ("f",     (time.Millisecond / 100).ToString("0", culture)),
                ("tt",    time.Hour >= 12 ? "PM" : "AM"),
                ("t",     time.Hour >= 12 ? "P" : "A"),
            };

            return ScanAndReplace(format, tokens);
        }

        /// <summary>
        /// Scans <paramref name="format"/> left-to-right, replacing known tokens with their values.
        /// Unrecognized characters are emitted as-is. Replacement values are never re-scanned,
        /// so day/month names that happen to contain token characters are safe.
        /// </summary>
        private static string ScanAndReplace(string format, (string Token, string Value)[] tokens)
        {
            var sb = new System.Text.StringBuilder(format.Length * 2);
            int i = 0;
            while (i < format.Length)
            {
                bool matched = false;
                foreach (var (token, value) in tokens)
                {
                    if (i + token.Length <= format.Length &&
                        string.CompareOrdinal(format, i, token, 0, token.Length) == 0)
                    {
                        sb.Append(value);
                        i += token.Length;
                        matched = true;
                        break;
                    }
                }
                if (!matched)
                {
                    sb.Append(format[i]);
                    i++;
                }
            }
            return sb.ToString();
        }

        private static string GetDaySymbol(IsoDayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                IsoDayOfWeek.Monday => "☽",     // Moon
                IsoDayOfWeek.Tuesday => "♂",    // Mars
                IsoDayOfWeek.Wednesday => "☿",  // Mercury
                IsoDayOfWeek.Thursday => "♃",   // Jupiter
                IsoDayOfWeek.Friday => "♀",     // Venus
                IsoDayOfWeek.Saturday => "♄",   // Saturn
                IsoDayOfWeek.Sunday => "☉",     // Sun
                _ => "?"
            };
        }
    }
}