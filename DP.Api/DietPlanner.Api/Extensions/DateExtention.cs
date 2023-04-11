using System;
using System.Globalization;

namespace DietPlanner.Api.Extensions
{
    public static class DateExtention
    {
        public static string Normalize(this DateTime date)
        {
            if(DateTime.TryParseExact(date.ToString(), "d/M/yyyy h:mm:ss tt", 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.AssumeUniversal, out DateTime result)){
                return result.ToShortDateString();
            }
            return string.Empty;
        }
    }
}
