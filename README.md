# LykkeExchange
c# API wrappers for [Lykke Trading API](https://www.lykke.com/cp/lykke-trading-api)

# Features
-  Fetch Exchange Rate between two currencies
-  Get Trading History between two currencies
-  Place Market Order on Lykke Exchange with API key.
-  Fetch wallet Trading History between two currencies, we made using API key.

# API key generation
API key is required for for wallet access and making API calls to Lykke Exchange
See [API key generation](https://auth.lykke.com/signin?ReturnUrl=%2Fconnect%2Fauthorize%3Fclient_id%3D2c6c77be-efd3-44b7-a381-940c90e52985%26scope%3Dprofile%2520email%2520address%26response_type%3Dtoken%26redirect_uri%3Dhttps%253A%252F%252Fwallet.lykke.com%252Fauth%26nonce%3DxXxvmySWwe9MDMZeIpU8%26state%3DrYnw6uFcMmXCz3Ytuihk) for to get your personal API key.

# Installation
Clone or download the repository and build the project. LykkeExchange.dll is gererated inside bin/[Debug|Release] of the root directory which can be referrenced in your projects.

# Usage
The module can be imported with the below statement

# License

Standar [MIT License](https://opensource.org/licenses/MIT)

Copyright 2018 LIVETREE

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

```csharp
using ExchangeMarket.LykkeExchange
```

#### API Documentation
##### Classes:
* ```csharp 
    public class LykkeExchange
    ```
    > Main entry point to Lykke Exchange

    ##### Methods:
    * `LykkeExchange()`
        > Default constructor.
    
    * [LykkeExchangeRate]() GetExchangeRate(string FromCurrency, string ToCurrency)
        > Get exchnage rate between FromCurrency to ToCurrency.
    
        Returns [LykkeExchangeRate]()
    
    * `List<[LykkeHistory]()> GetTradingHistory(string FromCurrency, string ToCurrency, int Skip, int Count)`
        > Get trading histories between FromCurrency and ToCurrency from LykkeExchange skipping specified Skip number of recent Trading histories and fetching the next Count number of trading histories.
        
        Returns a List of [LykkeHistory]()
    
    * `public [LykkeTradeHistory]()[] GetWalletTradeInformation(string apiKey, string FromCurrency, string ToCurrency, int skip, int take)`
    
        >Get wallet trading histories between FromCurrency and ToCurrency from LykkeExchange skipping specified Skip number of recent Trading histories and fetching the next Count number of trading histories.with the wallet apikey.
        
         Returns List of [LykkeTradeHistory]()
    
    * `[LykkeMoney]() MarketOrder(string apiKey, string FromCurrency, string ToCurrency, [LykkeTradeType]() tradeType, decimal value)`
    
        > Execute a market order in lykke exchange. Buy or Sell a volume of an asset for another asset with the wallet apikey. 
        
        > **Arguments**:
            1. apikey : wallet api key
            2. FromCurrency : currency to trade from
            3. ToCurrency : currency to trade for
            4. tradeType : type of trade (Buy or Sell)
            5. value : Transaction volume.
    
        Returns [LykkeMoney]()
    
* ```csharp
    public class LykkeTradeHistory
    ```
    
    > Class to Store Wallet trade information.
    
    **Members:**<br/>
        * `DateTime DateTime`: Date time of trade<br/>
        * `string Id` : Trade ID<br/>
        * `string State` : current state of trade<br/>
        * `decimal Amount` : Trade Volume<br/>
        * `string Asset` : Trade Asset<br/>
        * `string AssetPair` : Trade Asset Pair<br/>
        * `decimal Price`: Trade Price<br/>
* ```csharp
    public enum LykkeTradeType
    {
        SELL, //ASK
        BUY   //BID
    }
    ```
* ```csharp
    public class LykkeHistory
    ```
    
    **Members:**<br/>
        * `string FromCurrency` : From asset<br/>
        * `string ToCurrency` : To asset<br/>
        * `decimal Amount` : Trade Amount<br/>
        * `decimal Price` : Trade Price<br/>
        * `LykkeTradeType TradeType` : SELL or BUY<br/>
        * `DateTime DateTime` : Date and time of trade<br/>
    
    **Methods:**
    ```csharp
        public LykkeHistory(string FromCurrency, string ToCurrency, decimal Amount, decimal Price, LykkeTradeType TradeType, DateTime DateTime)
    ```
        
    > Default Constructor


* ```csharp
    public class LykkeExchangeRate
    ```
    
    **Members:**<br/>
        ***`string FromCurrency` : exchange from asset<br/>
        ***`string ToCurrency` : exchange to asset<br/>
        *** `decimal Sell` : seller price<br/>
        *** `decimal Buy` : buyer price<br/>
    **Methods:**
    ```csharp
    public LykkeExchangeRate(string FromCurrency, string ToCurrency, decimal Sell, decimal Buy)
    ```
    
    > Default Constructor


* ```csharp
    public struct LykkeMoney : IEquatable<LykkeMoney>
    ```
 
    **Members:**<br/>
    * `decimal amount` : Amount<br/>
    * `string Currency` : Asset<br/>
    
    **Methods**
    ```csharp
    public LykkeMoney(decimal amount, string currency)
    ```
    > Default Constructor
       
