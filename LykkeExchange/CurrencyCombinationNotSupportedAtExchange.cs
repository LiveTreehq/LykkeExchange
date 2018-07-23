using System;

namespace ExchangeMarket
{
    internal class CurrencyCombinationNotSupportedAtExchange : Exception
    {
        private string _fromCurrency;
        private string _toCurrency;
        private string _exchangeName;

        /// <summary>
        /// Exception to handle unavailable conversion rates between currencies.
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="fromCurrency"></param>
        /// <param name="toCurrency"></param>
        public CurrencyCombinationNotSupportedAtExchange(string exchangeName, string fromCurrency, string toCurrency) : base($"{exchangeName} does not have conversion rates between {fromCurrency} and {toCurrency}")
        {
            this._exchangeName = exchangeName;
            this._fromCurrency = fromCurrency;
            this._toCurrency = toCurrency;
        }
    }
}
