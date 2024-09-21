using System;

namespace Utility
{
    class Format
    {
        public static string Suffix(ulong num)
        {
            var digits = (int)Math.Log10(num);
            var order = digits / 3;
            var prefix = num / Math.Pow(1000, order);
            var fmt = Convert.ToDouble(String.Format("{0:G3}", prefix)).ToString("R0");
            return fmt + order switch
            {
                1 => "K",
                2 => "M",
                3 => "B",
                4 => "T",
                5 => "Qa",
                6 => "Qi",
                _ => ""
            };
        }
    }
}
