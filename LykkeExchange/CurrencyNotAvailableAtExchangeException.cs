﻿using System;

namespace ExchangeMarket.LykkeExchange
{
    internal class CurrencyNotAvailableAtExchangeException : Exception
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
        public CurrencyNotAvailableAtExchangeException(string ExchangeName, string FromCurrency, string ToCurrency) : base($"{ExchangeName} does not have conversion rates between {FromCurrency} and {ToCurrency}")
        {
            this.ExchangeName = ExchangeName;
            this.FromCurrency = FromCurrency;
            this.ToCurrency = ToCurrency;
        }
    }
}
