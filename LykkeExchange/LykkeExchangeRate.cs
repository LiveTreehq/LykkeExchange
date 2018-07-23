using System;

namespace ExchangeMarket
{
    /// <summary>
    /// Exchange rate between two currencies
    /// </summary>
    public class LykkeExchangeRate
    {
        public DateTime TimeStamp { get; }

        public string  FromCurrency;
        public string  ToCurrency;
        public decimal Sell;
        public decimal Buy;

        /// <summary>
        /// Creates a LykkeExchangeRate object with given arguments.  
        /// </summary>
        /// <param name="fromCurrency">Currecny to exchange from</param>
        /// <param name="toCurrency">Currency to exchange for</param>
        /// <param name="sell">Exchange ratio which the sellers are willing to spend</param>
        /// <param name="buy">Exchange ratio which the buyers are willing to spend</param>
        public LykkeExchangeRate(string fromCurrency, string toCurrency, decimal sell, decimal buy)
        {
            this.TimeStamp = DateTime.Now;

            this.FromCurrency = fromCurrency;
            this.ToCurrency = toCurrency;
            this.Sell = sell;
            this.Buy = buy;
        }
    }
}
