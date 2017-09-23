using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Compliance.ApiGateway.Models;
using Bam.Compliance.Infrastructure.Database;

namespace Bam.Compliance.ApiGateway.Services
{
    public class ExecutionRetriever : IExecutionRetriever
    {
        private const string Query = @"select TradeId=p.OrderId, Quantity=e.shares, e.Price, ExecutionTime = e.timeexecution, e.Exchange 
                    from [bamtca].[executions] e (nolock)
                    inner join [bamtca].[placements] p (nolock) on e.placementId = p.placementId
                    where p.orderid = '{0}'
                    union
                    select TradeId=p.OrderId, Quantity=e.shares, e.Price, ExecutionTime = e.timeexecution, e.Exchange 
                    from [aqtftca].[executions] e (nolock)
                    inner join [aqtftca].[placements] p (nolock) on e.placementId = p.placementId
                    where p.orderid = '{0}'
                    order by timeExecution desc";

        private readonly string _connectionString;

        public ExecutionRetriever(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Execution> GetExecutions(string tradeId)
        {
            var query = string.Format(Query, tradeId);
            using (var connection = new SqlConnection(_connectionString))
            {
                var dbOps = new DbOperation(connection);
                return dbOps.Select<Execution>(query);
            }
        }
    }

}
