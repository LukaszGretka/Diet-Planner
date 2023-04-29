using System;
using System.Globalization;

namespace DietPlanner.Api.Extensions
{
    public static class DateExtention
    {
        public static string ToDatabaseDateFormat(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");       
        }
    }
}
