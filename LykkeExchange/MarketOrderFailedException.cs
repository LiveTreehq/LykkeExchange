using System;

namespace LykkeExchange
{
    internal class MarketOrderFailedException : Exception
    {
        string _fromCurrency;
        string _toCurrency;
        string _exchangeName;

        /// <summary>
        /// Exception to handle unavailable conversion rates between currencies.
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="fromCurrency"></param>
        /// <param name="toCurrency"></param>
        public MarketOrderFailedException(string exchangeName, string fromCurrency, string toCurrency) : base($"Market order between {fromCurrency} and {toCurrency} failed in {exchangeName}")
        {
            this._exchangeName = exchangeName;
            this._fromCurrency = fromCurrency;
            this._toCurrency = toCurrency;
        }
    }
}
