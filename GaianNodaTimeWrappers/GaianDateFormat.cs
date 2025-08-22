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

            var result = format;

            // Process patterns in order of specificity (longest first to avoid substring conflicts)
            result = ReplacePattern(result, "MMMMM", gaianMonth.ToString("G", culture));
            result = ReplacePattern(result, "MMMM", gaianMonth.ToString("G", culture));
            result = ReplacePattern(result, "MMM*", MonthSymbols[gaianMonth.Value - 1]);
            result = ReplacePattern(result, "MMM", gaianMonth.ToString("G", culture).Substring(0, Math.Min(3, gaianMonth.ToString("G", culture).Length)));
            result = ReplacePattern(result, "MM", gaianMonth.ToString("NN", culture));
            result = ReplacePattern(result, "M", gaianMonth.ToString("N", culture));

            result = ReplacePattern(result, "dddd", NumberWords[Math.Min(gaianDay, NumberWords.Length - 1)]);
            result = ReplacePattern(result, "ddd", gaianDay + OrdinalSuffixes[Math.Min(gaianDay, OrdinalSuffixes.Length - 1)]);
            result = ReplacePattern(result, "dd", gaianDay.ToString("00", culture));
            result = ReplacePattern(result, "d", gaianDay.ToString("0", culture));

            result = ReplacePattern(result, "WWWW", dayOfWeek.ToString());
            result = ReplacePattern(result, "WWW", dayOfWeek.ToString().Substring(0, 3));
            result = ReplacePattern(result, "WW", dayOfWeek.ToString().Substring(0, 2));
            result = ReplacePattern(result, "W", GetDaySymbol(dayOfWeek));

            result = ReplacePattern(result, "yyyyy", gaianYear.ToString("00000", culture));
            result = ReplacePattern(result, "yyyy", gaianYear.ToString("0000", culture));
            result = ReplacePattern(result, "yy", (gaianYear % 100).ToString("00", culture));
            result = ReplacePattern(result, "y", (gaianYear % 100).ToString("0", culture));

            result = ReplacePattern(result, "DDD", gaianDayOfYear.ToString("000", culture));

            return result;
        }

        public static string Format(LocalDateTime dateTime, string format, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(format))
            {
                var gdate = new GaianLocalDate(dateTime.Date);
                return $"{gdate.ToString()} {dateTime.TimeOfDay:HH:mm}";
            }

            var result = Format(dateTime.Date, format, culture);
            var time = dateTime.TimeOfDay;

            // Time patterns - process in order of specificity (longest first)
            result = ReplacePattern(result, "hh", (time.Hour % 12 == 0 ? 12 : time.Hour % 12).ToString("00", culture));
            result = ReplacePattern(result, "h", (time.Hour % 12 == 0 ? 12 : time.Hour % 12).ToString("0", culture));
            result = ReplacePattern(result, "HH", time.Hour.ToString("00", culture));
            result = ReplacePattern(result, "H", time.Hour.ToString("0", culture));
            result = ReplacePattern(result, "mm", time.Minute.ToString("00", culture));
            result = ReplacePattern(result, "m", time.Minute.ToString("0", culture));
            result = ReplacePattern(result, "ss", time.Second.ToString("00", culture));
            result = ReplacePattern(result, "s", time.Second.ToString("0", culture));
            result = ReplacePattern(result, "fff", time.Millisecond.ToString("000", culture));
            result = ReplacePattern(result, "ff", (time.Millisecond / 10).ToString("00", culture));
            result = ReplacePattern(result, "f", (time.Millisecond / 100).ToString("0", culture));
            result = ReplacePattern(result, "tt", time.Hour >= 12 ? "PM" : "AM");
            result = ReplacePattern(result, "t", time.Hour >= 12 ? "P" : "A");

            return result;
        }

        private static string ReplacePattern(string input, string pattern, string replacement)
        {
            return input.Replace(pattern, replacement);
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