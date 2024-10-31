using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Ocorreu um execeção não tratada: Status Code 500");

            context.Result = new ObjectResult("Ocorreu um problema ao tratar a sua solicitacao: Status code 500 ")
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}
