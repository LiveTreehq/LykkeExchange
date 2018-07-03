using System;

namespace ExchangeMarket.LykkeExchange
{
    internal class MarketOrderFailedException : Exception
    {
        string FromCurrency;
        string ToCurrency;
        string ExchangeName;

        /// <summary>
        /// Exception to handle unavailable conversion rates between currencies.
        /// </summary>
        /// <param name="ExchangeName"></param>
        /// <param name="FromCurrency"></param>
        /// <param name="ToCurrency"></param>
        public MarketOrderFailedException(string ExchangeName, string FromCurrency, string ToCurrency) : base($"Market order between {FromCurrency} and {ToCurrency} failed in {ExchangeName}")
        {
            this.ExchangeName = ExchangeName;
            this.FromCurrency = FromCurrency;
            this.ToCurrency = ToCurrency;
        }
    }
}
