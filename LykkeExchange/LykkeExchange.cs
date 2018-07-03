using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ExchangeMarket.LykkeExchange
{
    public class LykkeExchange
    {
        // Get exchange rates
        internal static string URL = "https://public-api.lykke.com/api/AssetPairs/rate/";

        // Get Trading histories
        internal static string PublicTradingHistoryUrl = "https://public-api.lykke.com/api/Trades/";

        // Perform trade.
        internal static string TransactionAPIUrl = "https://hft-api.lykke.com/api/";

        internal JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings();

        public LykkeExchange()
        {
            JsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
        }

        /// <summary>
        /// Get exchange rate between <paramref name="FromCurrency"/> to <paramref name="ToCurrency"/>
        /// </summary>
        /// <param name="FromCurrency">Currecny to exchange from</param>
        /// <param name="ToCurrency">Currecny to exchange for</param>
        /// <returns></returns>
        /// <exception cref="CurrencyNotAvailableAtExchangeException">Throws when the exchange rate is not available in LykkeExchange</exception>
        public LykkeExchangeRate GetExchangeRate(string FromCurrency, string ToCurrency)
        {
            string currencyCombination = GetCurrencyCombination(FromCurrency, ToCurrency);

            string urlResponse = ThirdPartyRestCallsUtility.GET(URL + currencyCombination);

            LykkeMarketExchangeRate exchangeRate = ExtractExchangeRate(urlResponse);

            if (exchangeRate == null)
            {
                throw new CurrencyNotAvailableAtExchangeException("Lykke Exchange", FromCurrency, ToCurrency);
            }

            return new LykkeExchangeRate(FromCurrency, ToCurrency, exchangeRate.ask, exchangeRate.bid);
        }

        private string GetCurrencyCombination(string FromCurrency, string ToCurrency)
        {
            return ToCurrency.ToString() + FromCurrency.ToString();
        }

        /// <summary>
        /// Get trading history between <paramref name="FromCurrency"/> and <paramref name="ToCurrency"/> from LykkeExchange
        /// </summary>
        /// <param name="FromCurrency">Currecny to exchange from</param>
        /// <param name="ToCurrency">Currecny to exchange for</param>
        /// <param name="Skip">Number of recent trading histories to skip</param>
        /// <param name="Count">Number of recent histories to collect after skipping <paramref name="Skip"/></param>
        /// <returns>List of <see cref="LykkeHistory"/></returns>
        public List<LykkeHistory> GetTradingHistory(string FromCurrency, string ToCurrency, int Skip, int Count)
        {
            string currencyCombination = GetCurrencyCombination(FromCurrency, ToCurrency);

            string urlToRequest = PublicTradingHistoryUrl + currencyCombination + "?skip=" + Skip;
            if (Count > 0)
            {
                urlToRequest = urlToRequest + "&take=" + Count;
            }

            string urlResponse = ThirdPartyRestCallsUtility.GET(urlToRequest);

            List<LykkeTrade> tradingHistory = ExtractExchangeTradingHistory(urlResponse);

            List<LykkeHistory> tradeHistories = new List<LykkeHistory>();

            if (tradingHistory != null && tradingHistory.Count > 0)
            {
                foreach (LykkeTrade trade in tradingHistory)
                {
                    //TODO: Need to send right gas currency value
                    tradeHistories.Add(new LykkeHistory(FromCurrency, ToCurrency, trade.volume, trade.price,
                        trade.action == "Buy" ? LykkeTradeType.BUY : LykkeTradeType.SELL, ExchangeUtility.GetTimestamp(trade.dateTime), FromCurrency));
                }
            }

            return tradeHistories;
        }

        private List<LykkeTrade> ExtractExchangeTradingHistory(string data)
        {
            if (data == null || data == "")
                return null;

            List<LykkeTrade> tradingHistory = JsonConvert.DeserializeObject<List<LykkeTrade>>(data);
            return tradingHistory;
        }

        /**
         * Below method is only when we get all the data and need to convert and keep them with us.
         * https://public-api.lykke.com/api/AssetPairs/rate with out currency combination
         */
        private Dictionary<string, LykkeMarketExchangeRate> ExtractExchangeRates(string data)
        {
            Dictionary<string, LykkeMarketExchangeRate> exchangeRates = new Dictionary<string, LykkeMarketExchangeRate>();
            LykkeMarketExchangeRate[] lykkeExchangeRates = JsonConvert.DeserializeObject<LykkeMarketExchangeRate[]>(data);
            foreach (LykkeMarketExchangeRate exchangeRate in lykkeExchangeRates)
            {
                exchangeRates.Add(exchangeRate.id, exchangeRate);
            }
            return exchangeRates;
        }

        private LykkeMarketExchangeRate ExtractExchangeRate(string data)
        {
            if (data == null || data == "")
                return null;
            LykkeMarketExchangeRate exchangeRate = JsonConvert.DeserializeObject<LykkeMarketExchangeRate>(data);
            return exchangeRate;
        }

        /// <summary>
        /// Get trading history between <paramref name="fromCurrency"/> and <paramref name="toCurrency"/> from LykkeExchange
        /// </summary>
        /// <param name="apiKey">API key</param>
        /// <param name="fromCurrency">Currecny to exchange from</param>
        /// <param name="toCurrency">Currecny to exchange to</param>
        /// <param name="skip">Number of recent trading histories to skip</param>
        /// <param name="take">Number of recent histories to collect after skipping <paramref name="Skip"/></param>
        /// <returns><see cref="LykkeTradeHistory"/></returns>
        public LykkeTradeHistory[] GetWalletTradeInformation(string apiKey, string fromCurrency, string toCurrency, int skip, int take)
        {
            string CurrencyCombination = GetCurrencyCombination(fromCurrency, toCurrency);
            string url = TransactionAPIUrl + "History/trades?assetPairId=" + CurrencyCombination + "&skip=" + skip + "&take=" + take;

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("api-key", apiKey);

            string urlResponse = ThirdPartyRestCallsUtility.GET(url, headers);
            try
            {
                LykkeTradeHistory[] lykkeExchangeRates = JsonConvert.DeserializeObject<LykkeTradeHistory[]>(urlResponse);
                return lykkeExchangeRates;
            }
            catch (Exception e)
            {
                return new LykkeTradeHistory[0];
            }
        }

        /// <summary>
        /// Execute a market order in lykke exchange. Buy or Sell a volume of an asset for another asset.
        /// </summary>
        /// <param name="apiKey">API key</param>
        /// <param name="fromCurrency">Currecny to exchange from</param>
        /// <param name="toCurrency">Currecny to exchange to</param>
        /// <param name="tradeType">
        /// SELL <paramref name="fromCurrency"/> for <paramref name="toCurrency"/>or BUY <paramref name="fromCurrency"/> for <paramref name="toCurrency"/>
        /// </param>
        /// <param name="value">Volume of trade</param>
        /// <returns>Amount of resultant <paramref name="toCurrency"/> after executing market order.</returns>
        public LykkeMoney MarketOrder(string apiKey, string fromCurrency, string toCurrency, LykkeTradeType tradeType, decimal value)
        {
            // Actual market order implementation
            string CurrencyCombination = GetCurrencyCombination(fromCurrency, toCurrency);
            string url = TransactionAPIUrl + "Orders/market";

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("api-key", apiKey);

            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("AssetPairId", CurrencyCombination);
            postData.Add("Asset", fromCurrency.ToString());
            postData.Add("OrderAction", tradeType == LykkeTradeType.BUY ? "Buy" : "Sell");
            postData.Add("Volume", value);
            postData.Add("executeOrders", false);

            string postDataString = JsonConvert.SerializeObject(postData);
            string urlResponse = ThirdPartyRestCallsUtility.POST(url, postDataString, headers);
            try
            {
                if (urlResponse != null)
                {
                    MarketResult MarketResult = JsonConvert.DeserializeObject<MarketResult>(urlResponse, JsonSerializerSettings);
                    return new LykkeMoney(MarketResult.Result, toCurrency);
                }
                else
                {
                    return new LykkeMoney(-1, toCurrency);
                }
            }
            catch (Exception e)
            {
                return new LykkeMoney(-1, toCurrency);
            }
        }
    }
    
    /// <summary>
    /// Lykke Trade History to consume trading history of given Asset Pair
    /// </summary>

    public class LykkeTradeHistory
    {
        DateTime DateTime;
        string Id;
        string State;
        decimal Amount;
        string Asset;
        string AssetPair;
        decimal Price;
        Fee Fee;
    }

    /// <summary>
    /// Internal models to consume Lykke API results
    /// </summary>

    internal class MarketResult
    {
        public decimal Result;
    }

    internal class Fee
    {
        decimal Amount;
        Type Type;
    }

    internal enum Type
    {
        Unknown,
        Absolute,
        Relative
    }

    internal class LykkeMarketExchangeRate
    {
        public string id;
        public decimal bid;
        public decimal ask;
    }

    internal class LykkeTrade
    {
        //{"id":"aef9b066-ec4b-4754-a93c-061d6e3bb8ed","assetPairId":"ETHBTC","dateTime":"2018-06-13T06:49:21.902Z","volume":0.68534876,"index":4,"price":0.07624,"action":"Buy"}
        public DateTime dateTime;
        public decimal volume;
        public string action;
        public decimal price;
    }
}
