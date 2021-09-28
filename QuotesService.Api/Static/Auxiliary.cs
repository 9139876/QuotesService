using QuotesService.Api.Enum;
using System;

namespace QuotesService.Api.Static
{
    public static class Auxiliary
    {
        public static string GetDecimalString(decimal d)
        {
            if (d < 1)
            {
                return d.ToString("f6");
            }
            else if (d < 10)
            {
                return d.ToString("f4");
            }
            else
            {
                return d.ToString("f2");
            }
        }

        public static string GetDateString(DateTime dt, TimeFrameEnum tf)
        {
            if ((int)tf == 10)
            {
                return dt.ToString("G");
            }
            else if ((int)tf <= 40)
            {
                return dt.ToString("g");
            }
            else
            {
                return dt.ToString("d");
            }
        }
    }
}
