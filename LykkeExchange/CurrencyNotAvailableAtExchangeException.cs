using System;

namespace ExchangeMarket.LykkeExchange
{
    internal class CurrencyNotAvailableAtExchangeException : Exception
    {
        string fromCurrency;
        string toCurrency;
        string exchangeName;

        /// <summary>
        /// Exception to handle unavailable conversion rates between currencies.
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="fromCurrency"></param>
        /// <param name="toCurrency"></param>
        public CurrencyNotAvailableAtExchangeException(String exchangeName, string fromCurrency, string toCurrency) : base($"{exchangeName} does not have conversion rates between {fromCurrency} and {toCurrency}")
        {
            this.exchangeName = exchangeName;
            this.fromCurrency = fromCurrency;
            this.toCurrency = toCurrency;
        }
    }
}
