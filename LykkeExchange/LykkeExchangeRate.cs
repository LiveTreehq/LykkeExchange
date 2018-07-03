using System;

namespace ExchangeMarket.LykkeExchange
{
    /// <summary>
    /// Exchange rate between two currencies
    /// </summary>
    public class LykkeExchangeRate
    {
        public DateTime TimeStamp { get; }

        public string FromCurrency;
        public string ToCurrency;
        public decimal Sell;
        public decimal Buy;

        /// <summary>
        /// Creates a LykkeExchangeRate object with given arguments.  
        /// </summary>
        /// <param name="FromCurrency">Currecny to exchange from</param>
        /// <param name="ToCurrency">Currency to exchange for</param>
        /// <param name="Sell">Exchange ratio which the sellers are willing to spend</param>
        /// <param name="Buy">Exchange ratio which the buyers are willing to spend</param>
        public LykkeExchangeRate(string FromCurrency, string ToCurrency, decimal Sell, decimal Buy)
        {
            this.TimeStamp = DateTime.Now;

            this.FromCurrency = FromCurrency;
            this.ToCurrency = ToCurrency;
            this.Sell = Sell;
            this.Buy = Buy;
        }
    }
}
