using System.Collections.Generic;
using Bam.Compliance.Infrastructure.Hosting;

namespace Bam.Compliance.ApiGateway
{
    public class ApiGatewayService : IService
    {
        private readonly IService _httpService;
        
        public ApiGatewayService(IService httpService) 
            
        {
            _httpService = httpService;
        }

        public void Start()
        {
            _httpService.Start();
        }

        public void Stop()
        {
            _httpService.Stop();
        }

        public string Name => "API";
    }
}
