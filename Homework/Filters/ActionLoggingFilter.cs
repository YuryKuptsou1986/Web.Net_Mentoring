using Homework.Entities.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Homework.Filters
{
    public class ActionLoggingFilter : IActionFilter
    {
        private readonly ILogger<ActionLoggingFilter> _logger;
        private readonly IOptions<FilterLoggingSettings> _filterLoggingSettings;

        public ActionLoggingFilter(ILogger<ActionLoggingFilter> logger, IOptions<FilterLoggingSettings> filterLoggingSettings)
        {
            _logger = logger;
            _filterLoggingSettings = filterLoggingSettings;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (_filterLoggingSettings.Value.EnableLogging) {
                _logger.LogInformation($"'{context.RouteData.Values["controller"]}/{context.RouteData.Values["action"]}' started.");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (_filterLoggingSettings.Value.EnableLogging) {
                _logger.LogInformation($"'{context.RouteData.Values["controller"]}/{context.RouteData.Values["action"]}' completed.");
            }
        }
    }
}
