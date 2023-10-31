using Homework.Entities.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Text;

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

                _logger.LogInformation($"{GetControllerInfo(context.RouteData)} started. Action arguments: {GetActionArgumnetsForLogging(context)}.");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (_filterLoggingSettings.Value.EnableLogging) {
                _logger.LogInformation($"{GetControllerInfo(context.RouteData)} completed.");
            }
        }

        private string GetControllerInfo(RouteData routeData)
        {
            return $"'{routeData.Values["controller"]}/{routeData.Values["action"]}'";
        }

        private string GetActionArgumnetsForLogging(ActionExecutingContext context)
        {
            string actionArguments = $"{string.Join(";", context.ActionArguments.Select(x => $"{x.Key} = {x.Value}"))}";

            if (string.IsNullOrEmpty(actionArguments)) {
                actionArguments = "No action arguments";
            }

            return actionArguments;
        }
    }
}
