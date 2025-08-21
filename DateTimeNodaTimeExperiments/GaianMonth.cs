namespace Gaian
{
    public readonly struct GaianMonth
    {
        private readonly int _value; // 1–14

        private GaianMonth(int value) => _value = value;

        public int Value => _value;

        // Factory
        public static GaianMonth FromInt(int value)
            => value is >= 1 and <= 14 ? new GaianMonth(value)
                                       : throw new ArgumentOutOfRangeException();

        // Implicit conversions
        public static implicit operator int(GaianMonth m) => m._value;
        public static implicit operator GaianMonth(int i) => FromInt(i);

        public static implicit operator string(GaianMonth m) => GetName(m._value);

        // Custom ToString
        public override string ToString() => GetName(_value);

        public static string GetName(int month)
        {
            // 4-week months; week 53 becomes month 14 ("Horus")
            switch (month)
            {
                case 1: return "Sagittarius";
                case 2: return "Capricorn";
                case 3: return "Aquarius";
                case 4: return "Pisces";
                case 5: return "Aries";
                case 6: return "Taurus";
                case 7: return "Gemini";
                case 8: return "Cancer";
                case 9: return "Leo";
                case 10: return "Virgo";
                case 11: return "Libra";
                case 12: return "Scorpio";
                case 13: return "Ophiuchus";  // weeks 49–52
                case 14: return "Horus";      // week 53 (intercalary)
                default: throw new ArgumentOutOfRangeException(nameof(month), "Month must be 1–14.");
            }
        }
    }

}