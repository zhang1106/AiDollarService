using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Bam.Oms.Compliance;
using Bam.Oms.Compliance.Services;
using Bam.Oms.Data.Compliance;

namespace Bam.Compliance.ApiGateway.Http.Controller
{
    public class RuleController : ApiController
    {
        private readonly IRuleSvc _ruleSvc;

        public RuleController(IRuleSvc ruleSvc)
        {
            _ruleSvc = ruleSvc;
        }
        
        [AllowAnonymous]
        public IList<Rule<CompliancePosition>> GetAll(int policyId)
        {
            return _ruleSvc.GetAll(policyId);
        }

        [AllowAnonymous]
        public IList<Rule<CompliancePosition>> GetRules(int policyId, string name)
        {
            return _ruleSvc.GetAll(policyId).Where(r=>r.Name.ToUpper().Contains(name.ToUpper())).ToList();
        }

    }
}
