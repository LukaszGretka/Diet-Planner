using DietPlanner.Api.Extensions;
using System;
using Xunit;

namespace DietPlanner.Api.UnitTests.Extensions
{
    public class DateExtentionTests
    {
        [Fact]
        public void Normalize_WhenUsDateCulture_ReturnExpectedDateFormat()
        {
            var date = new DateTime(2023, 04, 29, 17, 10, 0);

            var result = DateExtentions.ToDatabaseDateFormat(date);

            Assert.Equal("2023-04-29", result);
        }
    }
}
