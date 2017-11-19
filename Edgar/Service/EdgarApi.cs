 using System.Collections.Generic;
using System.Linq;
using AiDollar.Edgar.Model;
using AiDollar.Edgar.Service.Model;
using AiDollar.Infrastructure.Database;

namespace AiDollar.Edgar.Service
{
    public class EdgarApi:IEdgarApi
    {
        private readonly string _connectionString;
        private readonly string _database;

        public EdgarApi(string connectionString, string database)
        {
            _connectionString = connectionString;
            _database = database;
        }
        public IList<Portfolio> GetPortfolios(string cik)
        {
            var db = new MongoDbOperation(_connectionString, _database);
            var qry = "{'Cik':'"+cik+"'}";
            var ports = db.Select<Portfolio>(qry).ToList();
            return ports;
        }

        public IList<Security> GetSecurities()
        {
            var db = new MongoDbOperation(_connectionString, _database);
            var securities = db.Select<Security>("{}").ToList();
            return securities;
        }

        public IList<Guru> GetGurus()
        {
            var db = new MongoDbOperation(_connectionString, _database);
            var gurus = db.Select<Guru>("{}").ToList();
            return gurus;
        }
    }
}
