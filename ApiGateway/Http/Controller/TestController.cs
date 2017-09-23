using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using AiDollar.Infrastructure.Logger;

namespace AiDollar.ApiGateway.Http.Controller
{
    public class TestController : ApiController
    {
        public TestController()
        {
            Logger.Log(LogLevel.Information, "Test Controller started");
        }
        public ILogger Logger { get; set; }

        [AllowAnonymous]
        [HttpGet]
        public string Get(string hello)
        {
            return hello;
        }
    }
}
