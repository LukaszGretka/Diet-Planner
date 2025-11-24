using System;

namespace DietPlanner.Api.Extensions
{
    public static class DateExtentions
    {
        public static string ToDatabaseDateFormat(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");       
        }
    }
}
