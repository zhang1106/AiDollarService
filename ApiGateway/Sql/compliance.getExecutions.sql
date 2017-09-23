declare @orderId varchar(50) = '2017022209UE'

select e.shares, e.price, e.timeexecution, e.exchange 
from [bamtca].[executions] e (nolock)
inner join [bamtca].[placements] p (nolock) on e.placementId = p.placementId
where p.orderid = @orderId
order by timeExecution desc