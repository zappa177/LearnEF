using Microsoft.AspNetCore.Mvc.Filters;

namespace LearnWebUI.Filters.ActionFilter
{
    public class PersonListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonListActionFilter> _logger;

        public PersonListActionFilter(ILogger<PersonListActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //add after logic here  
            _logger.LogInformation("OnActionExecuted method of PersonListActionFilter");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //add before logic here
            _logger.LogInformation("OnActionExecuting method of PersonListActionFilter");


        }
    }
}
