using System;

namespace ExchangeMarket.LykkeExchange
{
    /// <summary>
    /// Utility functions for Lykke exchange
    /// </summary>
    internal static class ExchangeUtility
    {
        /// <summary>
        /// Encode DateTime to long.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long GetTimestamp(DateTime dateTime)
        {
            return (long)((dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static long GetTimestamp(TimeSpan timeSpan)
        {
            DateTime dateTime = new DateTime(timeSpan.Ticks);
            return GetTimestamp(dateTime);
        }

        /// <summary>
        /// Decode long to DateTime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(long timestamp)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp).ToLocalTime();
            return dt;
        }


        /// <summary>
        /// Returns true if first value is samller
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool compareDataTime(long d1, long d2)
        {
            return DateTime.Compare(GetDateTime(d1), GetDateTime(d2)) < 0;
        }
    }
}
