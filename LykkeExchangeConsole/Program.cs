using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Reflection;
using ExchangeMarket;

namespace LykkeExchangeConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Options options = new Options();
            if (!ArgumentParser.Parse(args, ref options)) return;

            LykkeExchange lykkeExchange = new LykkeExchange();

            switch (options.methodName)
            {
                case "GetExchangeRate":
                    GetExchangeRateOptions ex = new GetExchangeRateOptions();

                    ex.fromCurrency = getFromCurrency();
                    ex.toCurrency = getToCurrency();
                    var excRate = lykkeExchange.GetExchangeRate(ex.fromCurrency, ex.toCurrency);
                    Console.WriteLine($"Sell Price {excRate.Sell}");
                    Console.WriteLine($"Buy Price {excRate.Buy}");
                    break;
                case "GetTradingHistory":
                    GetTradingHistoryOptions th = new GetTradingHistoryOptions();

                    th.fromCurrency = getFromCurrency();
                    th.toCurrency = getToCurrency();
                    th.skip = getSkip();
                    th.count = getCount();

                    var histories = lykkeExchange.GetTradingHistory(th.fromCurrency, th.toCurrency, (int)th.skip, (int)th.count);

                    foreach(var history in histories) {
                        DisplayTradeHistory(history);
                    }
                    break;
                case "GetWalletTradeInformation":
                    GetWalletTradeInformationOptions wt = new GetWalletTradeInformationOptions();

                    wt.fromCurrency = getFromCurrency();
                    wt.toCurrency = getToCurrency();
                    wt.skip = getSkip();
                    wt.take = getCount();
                    wt.apiKey = getAPIKey();
                    var apiKey = new System.Net.NetworkCredential(string.Empty, wt.apiKey).Password;

                    var walletHistories =lykkeExchange.GetWalletTradeInformation(apiKey, wt.fromCurrency, wt.toCurrency, (int)wt.skip, (int)wt.take);

                    foreach (var history in walletHistories) {
                        DisplayWalletTradeHistory(history);
                    }

                    break;
                case "GetBalances":
                    GetBalancesOptions b = new GetBalancesOptions();
                    b.apikey = getAPIKey();
                    apiKey = new System.Net.NetworkCredential(string.Empty, b.apikey).Password;
                    var balances = lykkeExchange.GetBalances(apiKey);

                    foreach(var balance in balances) {
                        DisplayWalletBalance(balance);
                    }

                    break;
                case "MarketOrder":
                    MarketOrderOptions m = new MarketOrderOptions();
                    m.apiKey = getAPIKey();
                    apiKey = new System.Net.NetworkCredential(string.Empty, m.apiKey).Password;
                    m.value = getTradeVolume();
                    m.fromCurrency = getFromCurrency();
                    m.toCurrency = getToCurrency();
                    LykkeTradeType tradeType;
                    Enum.TryParse<LykkeTradeType>(getTradeType(), out tradeType);
                    m.tradeType = tradeType;

                    var money = lykkeExchange.MarketOrder(apiKey, m.fromCurrency, m.toCurrency, m.tradeType, m.value);

                    Console.WriteLine($"Amount {money.Amount}");
                    Console.WriteLine($"Currency {money.Currency}");

                    break;
                default:
                    throw new Exception("API call is not supported");
            }
        }

        private static void DisplayWalletBalance(LykkeWalletBalance balance)
        {
            Console.WriteLine($"\nBalance {balance.Balance}");
            Console.WriteLine($"Reserved {balance.Reserved}");
            Console.WriteLine($"AssetId {balance.AssetId}\n");
        }

        private static void DisplayTradeHistory(LykkeHistory history)
        {
            Console.WriteLine($"\nTrade Amount {history.Amount}");
            Console.WriteLine($"Trade Price {history.Price}");
            Console.WriteLine($"Trade Type {history.TradeType.ToString()}");
            Console.WriteLine($"Trade Time {history.DateTime.ToLongDateString()}\n");
        }

        private static void DisplayWalletTradeHistory(LykkeTradeHistory history) {
            Console.WriteLine($"\nTrade Amount {history.Amount}");
            Console.WriteLine($"Trade Price {history.Price}");
            Console.WriteLine($"Trade DateTime {history.DateTime.ToLongDateString()}");
            Console.WriteLine($"Trade state {history.State}");
            Console.WriteLine($"Trade id {history.Id}");
            Console.WriteLine($"Trade asset {history.Asset}");
            Console.WriteLine($"Trade asset pair {history.AssetPair}\n");
        }

        private static string getFromCurrency()
        {
            Console.WriteLine("Enter fromCurrency. Currency to trade from");
            return Console.ReadLine();
        }

        private static string getToCurrency()
        {
            Console.WriteLine("Enter toCurrency. Currency to trade to");
            return Console.ReadLine();
        }

        private static SecureString getAPIKey()
        {
            SecureString securePwd = new SecureString();
            ConsoleKeyInfo key;

            Console.Write("Enter API key: ");
            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace)
                {
                    securePwd.AppendChar(key.KeyChar);
                    Console.Write("*");
                }
                else
                {
                    if (securePwd.Length > 0)
                    {
                        securePwd.RemoveAt(securePwd.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();

            return securePwd;
        }

        private static uint getSkip() {
            Console.WriteLine("Enter number of histories to skip");
            return UInt32.Parse(Console.ReadLine());
        }

        private static uint getCount()
        {
            Console.WriteLine("Enter number of histories to collect");
            return UInt32.Parse(Console.ReadLine());
        }

        private static decimal getTradeVolume() {
            Console.WriteLine("Enter Trade volume");
            return Decimal.Parse(Console.ReadLine());
        }

        private static string getTradeType() {
            Console.WriteLine("Enter Trade type. BUY or SELL");
            var tradeType = Console.ReadLine();
            if (tradeType != "BUY" && tradeType != "SELL")
                throw new Exception("Trade type is not supported");
            return tradeType;
        }
    }

    class ArgumentParser
    {

        public ArgumentParser()
        {
            throw new Exception("Helper class. Do not instantiate");
        }

        public static bool Parse(string[] args, ref Options options)
        {
            if (args.Length == 0) DisplayHelpInfo();
            else if (args[0] == "--help") DisplayHelpInfo();
            else if (args[0] == "--version") DisplayVersion();
            else if (args.Length > 1 && args[0] == "-api")
            {
                options.methodName = args[1];
                return true;
            }
            else
                throw new Exception("Argument is not supported.");
            return false;
        }

        private static void DisplayVersion()
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        private static void DisplayHelpInfo()
        {
            string helpInfo = "API calls utility for LykkeExchange" +
                "\n" +
                "\n\tSupported API calls:\n" +
                "\n\tGetExchangeRate: Get the exchange rate between two currencies" +
                "\n\tGetTradingHistory: Get trading history between two currencies" +
                "\n\tGetWalletTradeInformation: Get the wallet trade history" +
                "\n\tGetBalances: Get wallet balances" +
                "\n\tMarketOrder: Place a market order" +
                "\n\n" +
                "Use -api to specify the api call" +
                "\nUse --help for help information and --version for version info";
            Console.WriteLine(helpInfo);
        }
    }

    class Options
    {
        public Options() {}
        public string methodName { get; set; }
    }

    class GetExchangeRateOptions : Options
    {
        public string fromCurrency { get; set; }
        public string toCurrency { get; set; }
    }

    class GetTradingHistoryOptions : Options
    {
        public string fromCurrency { get; set; }
        public string toCurrency { get; set; }

        private uint _skip = 0;
        public uint skip
        {
            get { return this._skip; }
            set { this._skip = value; }
        }

        private uint _count = 0;
        public uint count
        {
            get
            {
                return this._count;
            }
            set
            {
                this._count = value;
            }
        }
    }

    class GetWalletTradeInformationOptions : Options
    {
        public SecureString apiKey { get; set; }
        public string fromCurrency { get; set; }
        public string toCurrency { get; set; }
        private uint _skip = 0;
        public uint skip
        {
            get { return this._skip; }
            set { this._skip = value; }
        }

        private uint _take = 0;
        public uint take
        {
            get
            {
                return this._take;
            }
            set
            {
                this._take = value;
            }
        }
    }

    class GetBalancesOptions : Options
    {
        public SecureString apikey { get; set; }
    }

    class MarketOrderOptions : Options
    {
        public SecureString apiKey { get; set; }
        public string fromCurrency { get; set; }
        public string toCurrency { get; set; }
        public LykkeTradeType tradeType { get; set; }
        public decimal value { get; set; }
    }
}
