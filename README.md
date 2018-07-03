# LykkeExchange

Features:

1) Fetches exchange rates between two currencies if Lykke Supports them
2) Fetches trading history for two currencies if Lykke Supports them
3) Places market order using our wallet apiKey
4) Fetch trading history for two currencies that we made using our wallet apiKey

How to generate the API Key:

Just need to follow the steps given in https://www.lykke.com/cp/lykke-trading-api

How to call the functions:

Initialize LykkeExchange
LykkeExchange lykkeExchange = new LykkeExchange();

1) To fetch Exchange Rates for any assetPair
 - LykkeExchangeRate exchangeRate = lykkeExchange.GetExchangeRate("ETH", "BTC");

 - What does LykkeExchangeRate have:
    - decimal Buy;
    - string FromCurrency;
    - decimal Sell;
    - string ToCurrency;
    
- What all exceptions does it throw:
    - CurrencyNotAvailableAtExchangeException indicating that one or both currencies are not supported by LykkeExchange
 
2) To fetch Trading History
- List<LykkeHistory> tradingHistory = lykkeExchange.GetTradingHistory("ETH", "BTC", 0, 100);
    - 0 is basically Skip index
    - 100 is basically Count index (How many entries to return from given Skip index)
    
- What does LykkeHistory have:
    - decimal Amount;
    - string FromCurrency;
    - string GasCurrency;
    - decimal Price;
    - long Timestamp;
    - string ToCurrency;
    - LykkeTradeType TradeType; // Enum having SELL, BUY
        
- What all exceptions does it throw:
    - CurrencyNotAvailableAtExchangeException indicating that one or both currencies are not supported by LykkeExchange

3) Actually placing Market Order:
- LykkeMoney money = lykkeExchange.MarketOrder(<API KEY>, "ETH", "BTC", LykkeTradeType.BUY, 1);
    - This call can be used for both BUY and SELL call.
    - 1 is the amount to be traded.

- What does LykkeMoney have:
    - decimal Amount // How much amount the trade yielded to
    - string Currency // which was the currency it traded for
    
- What all exceptions does it throw:
    - MarketOrderFailedException indicating that market order returned null from Lykke Exchange. It could be because you do not have sufficient wallet balance or any other reasons.
    - CurrencyNotAvailableAtExchangeException indicating that one or both currencies are not supported by LykkeExchange
    
4) Fetching trade history of the transactions we did for an asset pair
- LykkeTradeHistory[] walletTradeHistory = lykkeExchange.GetWalletTradeInformation(<API KEY>, "ETH", "BTC", 0, 10);
    - 0 is basically Skip index
    - 100 is basically Count index (How many entries to return from given Skip index)

- What does LykkeTradeHistory have:
    - DateTime DateTime;
    - string Id;
    - string State;
    - decimal Amount;
    - string Asset;
    - string AssetPair;
    - decimal Price;
    - Fee Fee;
    
- What all exceptions does it throw:
    - CurrencyNotAvailableAtExchangeException indicating that one or both currencies are not supported by LykkeExchange
