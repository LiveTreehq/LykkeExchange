using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LykkeExchange
{
    public class LykkeExchange
    {
        private const string LykkeExchangeName = "Lykke Exchange";

        // Get exchange rates
        internal static string Url = "https://public-api.lykke.com/api/AssetPairs/rate/";

        // Get Trading histories
        internal static string PublicTradingHistoryUrl = "https://public-api.lykke.com/api/Trades/";

        // Perform trade.
        internal static string TransactionApiUrl = "https://hft-api.lykke.com/api/";

        internal JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings();

        public LykkeExchange()
        {
            this.JsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
        }

        /// <summary>
        /// Get exchange rate between <paramref name="fromCurrency"/> to <paramref name="toCurrency"/>
        /// </summary>
        /// <param name="fromCurrency">Currecny to exchange from</param>
        /// <param name="toCurrency">Currecny to exchange for</param>
        /// <returns></returns>
        /// <exception cref="CurrencyNotAvailableAtExchangeException">Throws when the exchange rate is not available in LykkeExchange</exception>
        public LykkeExchangeRate GetExchangeRate(string fromCurrency, string toCurrency)
        {
            var currencyCombination = GetCurrencyCombination(fromCurrency, toCurrency);
            var exchangeRate = FetchExchangeRate(currencyCombination);

            var reversed = false;
            if (exchangeRate == null)
            {
                reversed = true;
                currencyCombination = GetReverseCurrencyCombination(fromCurrency, toCurrency);
                exchangeRate = FetchExchangeRate(currencyCombination);
            }

            if (exchangeRate == null)
            {
                throw new CurrencyNotAvailableAtExchangeException(LykkeExchangeName, fromCurrency, toCurrency);
            }

            return new LykkeExchangeRate(fromCurrency, toCurrency, reversed ? exchangeRate.bid : exchangeRate.ask, reversed ? exchangeRate.ask : exchangeRate.bid);
        }

        private LykkeMarketExchangeRate FetchExchangeRate(string currencyCombination)
        {
            var urlResponse = ThirdPartyRestCallsUtility.Get(Url + currencyCombination);

            var exchangeRate = ExtractExchangeRate(urlResponse);
            
            return exchangeRate;
        }

        private string GetCurrencyCombination(string fromCurrency, string toCurrency) => $"{toCurrency}{fromCurrency}";
        
        private string GetReverseCurrencyCombination(string fromCurrency, string toCurrency) => $"{fromCurrency}{toCurrency}";
        
        /// <summary>
        /// Get trading history between <paramref name="fromCurrency"/> and <paramref name="toCurrency"/> from LykkeExchange
        /// </summary>
        /// <param name="fromCurrency">Currecny to exchange from</param>
        /// <param name="toCurrency">Currecny to exchange for</param>
        /// <param name="skip">Number of recent trading histories to skip</param>
        /// <param name="count">Number of recent histories to collect after skipping <paramref name="skip"/></param>
        /// <returns>List of <see cref="LykkeHistory"/></returns>
        public List<LykkeHistory> GetTradingHistory(string fromCurrency, string toCurrency, int skip, int count)
        {
            var currencyCombination = GetCurrencyCombination(fromCurrency, toCurrency);
            var tradingHistory = FetchTradingHistory(skip, count, currencyCombination);

            var reversed = false;
            if (tradingHistory == null)
            {
                reversed = true;
                currencyCombination = GetReverseCurrencyCombination(fromCurrency, toCurrency);
                tradingHistory = FetchTradingHistory(skip, count, currencyCombination);
            }

            var tradeHistories = new List<LykkeHistory>();

            if (tradingHistory != null && tradingHistory.Count > 0)
            {
                foreach (var trade in tradingHistory)
                {
                    tradeHistories.Add(new LykkeHistory(reversed ? toCurrency : fromCurrency, reversed ? fromCurrency : toCurrency, trade.Volume, trade.Price,
                        trade.Action == "Buy" ? LykkeTradeType.BUY : LykkeTradeType.SELL, trade.DateTime));
                }
            }

            return tradeHistories;
        }

        private static List<LykkeTrade> FetchTradingHistory(int skip, int count, string currencyCombination)
        {
            var urlToRequest = PublicTradingHistoryUrl + currencyCombination + "?skip=" + skip;
            if (count > 0)
            {
                urlToRequest = urlToRequest + "&take=" + count;
            }

            string urlResponse;

            try
            {
                urlResponse = ThirdPartyRestCallsUtility.Get(urlToRequest);
            }
            catch (Exception)
            {
                urlResponse = null;
            }

            var tradingHistory = ExtractExchangeTradingHistory(urlResponse);
            return tradingHistory;
        }

        private static List<LykkeTrade> ExtractExchangeTradingHistory(string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;

            var tradingHistory = JsonConvert.DeserializeObject<List<LykkeTrade>>(data);
            return tradingHistory;
        }

        /**
         * Below method is only when we get all the data and need to convert and keep them with us.
         * https://public-api.lykke.com/api/AssetPairs/rate with out currency combination
         */
        private Dictionary<string, LykkeMarketExchangeRate> ExtractExchangeRates(string data)
        {
            var lykkeExchangeRates = JsonConvert.DeserializeObject<LykkeMarketExchangeRate[]>(data);
            return lykkeExchangeRates.ToDictionary(exchangeRate => exchangeRate.id);
        }

        private LykkeMarketExchangeRate ExtractExchangeRate(string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;
            var exchangeRate = JsonConvert.DeserializeObject<LykkeMarketExchangeRate>(data);
            return exchangeRate;
        }

        /// <summary>
        /// Get trading history between <paramref name="fromCurrency"/> and <paramref name="toCurrency"/> from LykkeExchange
        /// </summary>
        /// <param name="apiKey">API key</param>
        /// <param name="fromCurrency">Currency to exchange from</param>
        /// <param name="toCurrency">Currecny to exchange to</param>
        /// <param name="skip">Number of recent trading histories to skip</param>
        /// <param name="take">Number of recent histories to collect after skipping <paramref name="skip"/></param>
        /// <returns><see cref="LykkeTradeHistory"/></returns>
        public LykkeTradeHistory[] GetWalletTradeInformation(string apiKey, string fromCurrency, string toCurrency, int skip, int take)
        {
            var currencyCombination = GetCurrencyCombination(fromCurrency, toCurrency);
            var exchangeRate = FetchExchangeRate(currencyCombination);
            if (exchangeRate == null)
            {
                currencyCombination = GetReverseCurrencyCombination(fromCurrency, toCurrency);
                exchangeRate = FetchExchangeRate(currencyCombination);
                if (exchangeRate == null)
                {
                    throw new CurrencyNotAvailableAtExchangeException(LykkeExchangeName, fromCurrency, toCurrency);
                }
            }

            var url = TransactionApiUrl + "History/trades?assetPairId=" + currencyCombination + "&skip=" + skip + "&take=" + take;

            var headers = new Dictionary<string, string>();
            headers.Add("api-key", apiKey);

            var urlResponse = ThirdPartyRestCallsUtility.Get(url, headers);
            try
            {
                var lykkeExchangeRates = JsonConvert.DeserializeObject<LykkeTradeHistory[]>(urlResponse);
                return lykkeExchangeRates;
            }
            catch (Exception)
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
            var currencyCombination = GetCurrencyCombination(fromCurrency, toCurrency);
            var url = TransactionApiUrl + "Orders/market";

            var orderAction = tradeType == LykkeTradeType.BUY ? "Buy" : "Sell";

            var exchangeRate = FetchExchangeRate(currencyCombination);
            var reversed = false;
            if (exchangeRate == null)
            {
                reversed = true;
                currencyCombination = GetReverseCurrencyCombination(fromCurrency, toCurrency);

                exchangeRate = FetchExchangeRate(currencyCombination);
                
                orderAction = tradeType == LykkeTradeType.BUY ? "Sell" : "Buy";
                if (exchangeRate == null)
                {
                    throw new CurrencyNotAvailableAtExchangeException(LykkeExchangeName, fromCurrency, toCurrency);
                }

                // actual value is getting converted to reverse conversion as only reverse combination is supported
                value = value * (tradeType == LykkeTradeType.BUY ? exchangeRate.ask : exchangeRate.bid);
            }

            var headers = new Dictionary<string, string>();
            headers.Add("api-key", apiKey);

            var postData = new Dictionary<string, object>();
            postData.Add("AssetPairId", currencyCombination);
            postData.Add("Asset", reversed ? toCurrency : fromCurrency);
            postData.Add("OrderAction", orderAction);
            postData.Add("Volume", value);
            postData.Add("executeOrders", false);

            var postDataString = JsonConvert.SerializeObject(postData);
            var urlResponse = ThirdPartyRestCallsUtility.Post(url, postDataString, headers);
            try
            {
                if (urlResponse != null)
                {
                    var marketResult = JsonConvert.DeserializeObject<MarketResult>(urlResponse, this.JsonSerializerSettings);
                    var resultValue = marketResult.Result;
                    // if reversed returning local converted value assuming result is more or less same as that of our input volume
                    return reversed ? new LykkeMoney(value, toCurrency) : new LykkeMoney(resultValue, toCurrency); 
                }
                else
                {
                    throw new MarketOrderFailedException(LykkeExchangeName, fromCurrency, toCurrency);
                }
            }
            catch (Exception e)
            {
                throw new MarketOrderFailedException(LykkeExchangeName, fromCurrency, toCurrency);
            }
        }
    }
    
    /// <summary>
    /// Lykke Trade History to consume trading history of given Asset Pair
    /// </summary>

    public class LykkeTradeHistory
    {
        DateTime _dateTime;
        string _id;
        string _state;
        decimal _amount;
        string _asset;
        string _assetPair;
        decimal _price;
        Fee _fee;
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
        decimal _amount;
        Type _type;
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
        public DateTime DateTime;
        public decimal Volume;
        public string Action;
        public decimal Price;
    }
}
