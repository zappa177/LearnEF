using Microsoft.AspNetCore.Mvc.Filters;

namespace LearnWebUI.Filters.ActionFilter
{
    public class ResponseHeaderActionFilter : IActionFilter
    {
        private readonly ILogger<ResponseHeaderActionFilter> _logger;
        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
