using NodaTime;
using NodaTime.Calendars;
using System.ComponentModel;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Gaian
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //DateTime dateTime = new DateTime(13, 13, 13);
            int y = 12026;
            int m = 13;
            int d = 1;
            Offset offset = new Offset();
            GaianOffsetDateTime dt = new GaianOffsetDateTime(y, m, d, 1, 1, offset);
            Console.WriteLine(dt.ToString());
            string s = dt.ToDateTimeOffset().ToString();
            Console.WriteLine(s);
        }


        //private static string GetMonth(int month)
        //{
        //    switch (month)
        //    {
        //        case 1: return "Sagittarius";
        //        case 2: return "Capricorn";
        //        case 3: return "Aquarius";
        //        case 4: return "Pisces";
        //        case 5: return "Aries";
        //        case 6: return "Taurus";
        //        case 7: return "Gemini";
        //        case 8: return "Cancer";
        //        case 9: return "Leo";
        //        case 10: return "Virgo";
        //        case 11: return "Libra";
        //        case 12: return "Scorpio";
        //        case 13: return "Ophiuchus";
        //        case 14: return "Horus";
        //        default: throw new ArgumentOutOfRangeException(nameof(month), "Month must be 1–12.");
        //    }
        //}

    }
}
