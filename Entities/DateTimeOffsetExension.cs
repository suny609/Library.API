using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Entities
{
    public static class DateTimeOffsetExension
    {
        public static int GetCurrentAge(this DateTimeOffset dateTimeOffset)
        {
            return (DateTimeOffset.Now.Year - dateTimeOffset.Year);
        }
    }
}
