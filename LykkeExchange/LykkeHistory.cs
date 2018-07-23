using System;

namespace ExchangeMarket
{ 
    /// <summary>
    /// Type of trade
    /// </summary>
    public enum LykkeTradeType
    {
        SELL, //ASK
        BUY   //BID
    }

    /// <summary>
    /// 
    /// </summary>
    public class LykkeHistory
    {
        public string FromCurrency;
        public string ToCurrency;
        public decimal Amount;
        public decimal Price;
        public LykkeTradeType TradeType; // SELL or BUY
        public DateTime DateTime;

        /// <summary>
        /// Creates a LykkeHistory object with given arguments.
        /// </summary>
        /// <param name="FromCurrency">Trade from currency</param>
        /// <param name="ToCurrency">Trade to currency</param>
        /// <param name="Amount">Trade Volume</param>
        /// <param name="Price">Trading price</param>
        /// <param name="TradeType">Type of the trade. <see cref="LykkeTradeType"/></param>
        /// <param name="DateTime">Time Stamp of the trade</param>
        public LykkeHistory(string FromCurrency, string ToCurrency, decimal Amount, decimal Price, LykkeTradeType TradeType, DateTime DateTime)
        {
            this.FromCurrency = FromCurrency;
            this.ToCurrency = ToCurrency;
            this.Amount = Amount;
            this.Price = Price;
            this.TradeType = TradeType;
            this.DateTime = DateTime;
        }

        /// <summary>
        /// Custom comparator for LykkeHistory.
        /// </summary>
        /// <param name="obj">Object to compare with</param>
        /// <returns>
        /// true if all the parameters are equivalent.
        /// </returns>
        public override bool Equals(object obj)
        {
            var tradingHistory = obj as LykkeHistory;

            if (tradingHistory == null)
                return false;

            return tradingHistory.FromCurrency == this.FromCurrency && tradingHistory.ToCurrency == this.ToCurrency
                    && tradingHistory.Amount == this.Amount && tradingHistory.Price == this.Price &&
                    tradingHistory.TradeType == this.TradeType && tradingHistory.DateTime == this.DateTime;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
