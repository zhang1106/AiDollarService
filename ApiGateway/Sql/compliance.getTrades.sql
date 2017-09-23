Declare @startDate datetime, @endDate datetime, @symbol varchar(50)
Select @startDate = '2017-1-1', @endDate='2017-2-23', @symbol='IBM%'

SELECT 
       [tradedate]
	  ,Fund = Fundcode
	  ,Strategy = [StrategyCode]
	  ,Trader = [EB_T_TRADER]
	  ,Side = [EventType]
      ,[Quantity]
	  ,BamSymbol = [InvestmentCode]
	  ,Broker = [BrokerCode]
	  ,TradeId = isnull([EA_T_TradeID],[UserTranID1])
FROM  bamcorelite.txn.TxnAll (nolock)
WHERE InvestmentCode like @symbol
And TxnStatus <> 'Deleted'
And EventType in ('SellShort', 'Sell', 'CoverShort', 'Buy')
And TradeDate >= @startdate
And TradeDate <= @endDate