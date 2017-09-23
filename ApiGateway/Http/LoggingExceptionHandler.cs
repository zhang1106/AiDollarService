using System.Web.Http.ExceptionHandling;
using Bam.Compliance.Infrastructure.Logger;

namespace Bam.Compliance.ApiGateway.Http
{
    public class LoggingExceptionHandler : ExceptionHandler
    {
        private readonly ILogger _logger;

        public LoggingExceptionHandler(ILogger logger)
        {
            _logger = logger;
        }

        public override void Handle(ExceptionHandlerContext context)
        {
            _logger.LogError(context.Request?.RequestUri?.ToString(), context.Exception);
            base.Handle(context);
        }
    }
}
