using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ExchangeMarket
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
        /// <exception cref="CurrencyCombinationNotSupportedAtExchange">Throws when the exchange rate is not available in LykkeExchange</exception>
        public LykkeExchangeRate GetExchangeRate(string fromCurrency, string toCurrency)
        {
            var currencyCombination = GetCurrencyCombination(fromCurrency.ToString(), toCurrency.ToString());
            var exchangeRate = FetchExchangeRate(currencyCombination);

            if (exchangeRate != null)
                return new LykkeExchangeRate(fromCurrency, toCurrency, exchangeRate.ask, exchangeRate.bid);

            throw new CurrencyCombinationNotSupportedAtExchange(LykkeExchangeName, fromCurrency, toCurrency);
        }

        private LykkeMarketExchangeRate FetchExchangeRate(string currencyCombination)
        {
            var urlResponse = ThirdPartyRestCallsUtility.Get(Url + currencyCombination);

            var exchangeRate = ExtractExchangeRate(urlResponse);

            return exchangeRate;
        }

        private string GetCurrencyCombination(string fromCurrency, string toCurrency) => $"{fromCurrency.ToUpper()}{toCurrency.ToUpper()}";

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

            if (tradingHistory == null)
            {
                throw new CurrencyCombinationNotSupportedAtExchange(LykkeExchangeName, fromCurrency, toCurrency);
            }

            var tradeHistories = new List<LykkeHistory>();

            if (tradingHistory != null && tradingHistory.Count > 0)
            {
                foreach (var trade in tradingHistory)
                {
                    tradeHistories.Add(new LykkeHistory(fromCurrency, toCurrency, trade.Volume, trade.Price,
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
                throw new CurrencyCombinationNotSupportedAtExchange(LykkeExchangeName, fromCurrency, toCurrency);
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
        /// Returns all available balances in the exchange wallet
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public LykkeWalletBalance[] GetBalances(string apiKey)
        {
            var url = TransactionApiUrl + "Wallets";
            var headers = new Dictionary<string, string>();
            headers.Add("api-key", apiKey);

            var urlResponse = ThirdPartyRestCallsUtility.Get(url, headers);
            if (urlResponse != null)
            {
                LykkeWalletBalance[] walletBalances = JsonConvert.DeserializeObject<LykkeWalletBalance[]>(urlResponse, this.JsonSerializerSettings);
                return walletBalances;
            }

            return null;
        }

        /// <summary>
        /// Returns the balance of the given assetId for the wallet backed by apiKey
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public LykkeWalletBalance GetBalance(string apiKey, string assetId)
        {
            LykkeWalletBalance[] walletBalances = GetBalances(apiKey);
            if (walletBalances != null && walletBalances.Length > 0)
            {
                foreach (LykkeWalletBalance walletBalance in walletBalances)
                {
                    if (walletBalance.AssetId.ToLower().Equals(assetId.ToLower()))
                    {
                        return walletBalance;
                    }
                }
            }

            return null;
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

            var orderAction = tradeType == LykkeTradeType.BUY ? "Buy" : "Sell";

            var exchangeRate = FetchExchangeRate(currencyCombination);

            if (exchangeRate == null)
            {
                throw new CurrencyCombinationNotSupportedAtExchange(LykkeExchangeName, fromCurrency, toCurrency);
            }

            var url = TransactionApiUrl + "Orders/market";
            var headers = new Dictionary<string, string>();
            headers.Add("api-key", apiKey);

            var postData = new Dictionary<string, object>();
            postData.Add("AssetPairId", currencyCombination);
            postData.Add("Asset", fromCurrency.ToString());
            postData.Add("OrderAction", orderAction);
            postData.Add("Volume", value);

            var postDataString = JsonConvert.SerializeObject(postData);

            try
            {
                LykkeWalletBalance preExecutionBalance = GetBalance(apiKey, toCurrency.ToString());
                var urlResponse = ThirdPartyRestCallsUtility.Post(url, postDataString, headers);
                LykkeWalletBalance postExecutionBalance = GetBalance(apiKey, toCurrency.ToString());
                if (urlResponse != null)
                {
                    var marketResult = JsonConvert.DeserializeObject<MarketResult>(urlResponse, this.JsonSerializerSettings);
                    var price = marketResult.Result;
                    var tradeResult = (postExecutionBalance.Balance - preExecutionBalance.Balance);
                    return new LykkeMoney(tradeResult, toCurrency);
                }
                else
                {
                    throw new MarketOrderFailedException(LykkeExchangeName, fromCurrency.ToString(), toCurrency.ToString());
                }
            }
            catch (Exception e)
            {
                throw new MarketOrderFailedException(LykkeExchangeName, fromCurrency.ToString(), toCurrency.ToString());
            }
        }
    }

    /// <summary>
    /// Lykke Trade History to consume trading history of given Asset Pair
    /// </summary>

    public class LykkeTradeHistory
    {
        public DateTime DateTime;
        public string Id;
        public string State;
        public decimal Amount;
        public string Asset;
        public string AssetPair;
        public decimal Price;
        public Fee Fee;
    }

    /// <summary>
    /// Internal models to consume Lykke API results
    /// </summary>

    internal class MarketResult
    {
        public decimal Result;
    }

    public class Fee
    {
        public decimal Amount;
        public Type Type;
    }

    public enum Type
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

    public class LykkeWalletBalance
    {
        public string AssetId;
        public decimal Balance;
        public decimal Reserved;
    }
}
