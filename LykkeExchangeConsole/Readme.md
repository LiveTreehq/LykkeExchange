# LykkeExchange Command line API interface

## Building Console project

LykkeExchangeConsole is the project that generates the executable for interacting with LykkeExchange API.

Just build the project and use the LykkeExchangeConsole.exe for command line interface.

## Usage
Note: Unix users will need to have mono installed to run the executable. 

##### Help information

`LykkeExchangeConsole.exe` or `LykkeExchangeConsole.exe --help`

#### Version Information

`LykkeExchangeConsole.exe --version`

#### API interface
Currently the supported API calls include
* GetExchangeRate :  Get the exchange rate between two currencies
* GetTradingHistory : Get trading history between two currencies
* GetWalletTradeInformation : Get the wallet trade history
* GetBalances : Get wallet balances
* MarketOrder : Place a market order

Use argument key `-api` to specify the api.

**Examples:**

`LykkeExchangeConsole.exe -api GetExchangeRate`

> **Enter fromCurrency. Currency to trade from**
> 
> _USD_
> 
> **Enter toCurrency. Currency to trade to**
> 
> _BTC_
> 
> 		Sell Price 0.0001497005988023952095808383
> 
> 		Buy Price 0.0001496212636952083191816555


`LykkeExchangeConsole.exe -api GetTradingHistory`

> **Enter fromCurrency. Currency to trade from**
> 
> _ETH_
> 
> **Enter toCurrency. Currency to trade to**
> 
> _BTC_
> 
> **Enter number of histories to skip** (skips given number of most recent histories)
> 
> _10_
> 
> **Enter number of histories to collect** (collects the most recent histories after skipping)
> 
> _2_
> 
> 		Trade Amount 0.59998
> 		Trade Price 0.07163
> 		Trade Type SELL
> 		Trade Time Monday, July 16, 2018


> 		Trade Amount 0.56928
> 		Trade Price 0.07161
> 		Trade Type SELL
>		Trade Time Monday, July 16, 2018


`LykkeExchangeConsole.exe -api GetBalances`

> **Enter API key:** _**********************_
> 
> 		Balance 0.0001
> 		Reserved 0.000001
> 		AssetId BTC
> 
> 		Balance 0.043
> 		Reserved 0.0006
> 		AssetId ETH

`LykkeExchangeConsole.exe -api GetWalletTradeInformation`

> **Enter fromCurrency. Currency to trade from**
> 
> _ETH_
> 
> **Enter toCurrency. Currency to trade to**
> 
> _BTC_
> 
> **Enter number of histories to skip** (skips given number of most recent histories from wallet)
> 
> _10_
> 
> **Enter number of histories to collect** (collects the most recent histories from wallet after skipping)
> 
> _2_
> 
> **Enter API key:** _**********************_
>
>
>		Trade Amount 4.8797
>		Trade Price 325.31842
>		Trade DateTime Saturday, July 14, 2018
>		Trade state Finished
>		Trade id XXXXXXXXXXXXXXXXXXXXXXXXXXXX
>		Trade asset GBP
>		Trade asset pair ETHGBP
>
>
>		Trade Amount 0.015
>		Trade Price 335.01765
>		Trade DateTime Saturday, July 14, 2018
>		Trade state Finished
>		Trade id XXXXXXXXXXXXXXXXXXXXXXXXXXXX
>		Trade asset ETH
>		Trade asset pair ETHGBP
>
>
>		Trade Amount -5.0253
>		Trade Price 335.01765
>		Trade DateTime Saturday, July 14, 2018
>		Trade state Finished
>		Trade id XXXXXXXXXXXXXXXXXXXXXXXXXXXXX
>		Trade asset GBP
>		Trade asset pair ETHGBP

`LykkeExchangeConsole.exe -api MarketOrder`

Example: Buy 0.001 ETH suing GBP.

>	__Enter API key: ***************************************__
>	
>	**Enter Trade volume**
>		
>	_0.001_
>	
>	__Enter fromCurrency. Currency to trade from__
>	
>	_ETH_
>		
>	__Enter toCurrency. Currency to trade to__
>	
>	_GBP_
>	
>	__Enter Trade type. BUY or SELL__
>	
>	_BUY_
>	
>		Amount -0.3540 (This is the GBP that is debited)
>		Currency GBP

Before Market order, Balance:

    Balance 0.04535788
    Reserved 0.0
    AssetId ETH


    Balance 34.4049
    Reserved 0.0
    AssetId GBP

After Market order, Balance:

    Balance 0.04635788
    Reserved 0.0
    AssetId ETH


    Balance 34.0509
    Reserved 0.0
    AssetId GBP
