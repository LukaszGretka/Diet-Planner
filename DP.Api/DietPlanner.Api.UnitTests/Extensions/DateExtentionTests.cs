using DietPlanner.Api.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DietPlanner.Api.UnitTests.Extensions
{
    public class DateExtentionTests
    {
        [Fact]
        public void Normalize_WhenUsDateCulture_ReturnExpectedDateFormat()
        {
            var date = new DateTime(2023, 04, 29, 17, 10, 0);

            var result = DateExtention.ToDatabaseDateFormat(date);

            Assert.Equal("2023-04-29", result);
        }
    }
}
