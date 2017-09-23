using System.Collections.Generic;
using System.Web.Http;
using Bam.Oms.Compliance.Services;
using Bam.Oms.Data.Compliance;

namespace Bam.Compliance.ApiGateway.Http.Controller
{
    public class FactController : ApiController
    {
        private readonly IFactSvc _factSvc;
        public FactController( IFactSvc factSvc) 
        {
            _factSvc = factSvc;
        }

        [AllowAnonymous]
        public IList<Fact> GetAll()
        {
            return _factSvc.GetAll();
        }
        [AllowAnonymous]
        public Fact Get(int id)
        {
            return _factSvc.Get(id);
        }
        [AllowAnonymous]
        [HttpPost]
        public void Put(Fact fact)
        {
             _factSvc.Save(fact);
        }
    }
}
