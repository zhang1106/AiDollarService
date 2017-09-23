using System.Linq;
using System.Web.Http;
using Bam.Compliance.ApiGateway.Http;
using Swashbuckle.Application;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Bam.Compliance.ApiGateway.Http
{
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "Bam.Compliance.ApiGateway");
                        
                        c.ApiKey("Bearer")
                            .Description("Filling bearer token here")
                            .Name("Authorization")
                            .In("header");

                        c.ResolveConflictingActions(apiDescriptions => 
                            apiDescriptions.FirstOrDefault(r => r.ParameterDescriptions.Count > 0));
                    })
                .EnableSwaggerUi(c =>
                    {
                        c.DisableValidator();
                        c.EnableApiKeySupport("Authorization", "header");
                    });
        }
    }
}
