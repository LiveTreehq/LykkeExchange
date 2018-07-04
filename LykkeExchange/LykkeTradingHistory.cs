using System;

namespace LykkeExchange
{ 
    /// <summary>
    /// Type of trade
    /// </summary>
    public enum LykkeTradeType
    {
        Sell, //ASK
        Buy   //BID
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
        public string GasCurrency;

        /// <summary>
        /// Creates a LykkeHistory object with given arguments.
        /// </summary>
        /// <param name="fromCurrency">Trade from currency</param>
        /// <param name="toCurrency">Trade to currency</param>
        /// <param name="amount">Trade Volume</param>
        /// <param name="price">Trading price</param>
        /// <param name="tradeType">Type of the trade. <see cref="LykkeTradeType"/></param>
        /// <param name="dateTime">Time Stamp of the trade</param>
        /// <param name="gasCurrency"></param>
        public LykkeHistory(string fromCurrency, string toCurrency, decimal amount, decimal price, LykkeTradeType tradeType, DateTime dateTime, string gasCurrency)
        {
            this.FromCurrency = fromCurrency;
            this.ToCurrency = toCurrency;
            this.Amount = amount;
            this.Price = price;
            this.TradeType = tradeType;
            this.DateTime = dateTime;
            this.GasCurrency = gasCurrency;
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
                    tradingHistory.TradeType == this.TradeType && tradingHistory.DateTime == this.DateTime
                    && tradingHistory.GasCurrency == this.GasCurrency;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
