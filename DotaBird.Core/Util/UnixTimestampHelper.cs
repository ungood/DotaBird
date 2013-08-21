using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotaBird.Core.Util
{
    public static class UnixTimestampHelper
    {
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static double ToUnixTimestamp(this DateTime dt)
        {
            return (dt - epoch).TotalSeconds;
        }

        public static DateTime FromUnixTimestamp(double timestamp)
        {
            return epoch.AddSeconds(timestamp);
        }
    }
}
