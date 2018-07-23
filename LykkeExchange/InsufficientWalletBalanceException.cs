using System;

namespace ExchangeMarket
{
    internal class InsufficientWalletBalanceException : Exception
    {
        string _fromCurrency;
        string _exchangeName;

        /// <summary>
        /// Exception to handle insufficient balance
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="fromCurrency"></param>
        public InsufficientWalletBalanceException(string exchangeName, string fromCurrency) : base($"Insufficient balance for {fromCurrency} currency in {exchangeName}")
        {
            this._exchangeName = exchangeName;
            this._fromCurrency = fromCurrency;
        }
    }
}
