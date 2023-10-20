using Homework.Entities;
using Newtonsoft.Json;

namespace Homework.MIddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try {
                await _next(context);
            }catch(Exception ex) {
                string routeWhereExceptionOccured = context.Request.Path;

                var path = JsonConvert.SerializeObject(routeWhereExceptionOccured);
                var result = new ErrorViewModel {
                    Path = path
                };
                if (ex is AggregateException aggregateExc) {
                    var messages = aggregateExc.InnerExceptions.Select(x => x.Message).ToList();
                } else {
                    result.ErrorMessages = new List<string> { ex.Message };
                }

                string messageJson = JsonConvert.SerializeObject(result);
                context.Items["ErrorMessagesJson"] = messageJson;

                await HandleErrorAsync(context);
            }
        }

        private static async Task HandleErrorAsync(HttpContext context)
        {
            string messageJson = context.Items["ErrorMessagesJson"] as string;
            string redirectUrl = $"Home/ErrorMiddleware?messageJson={messageJson}";

            
            context.Response.Redirect(redirectUrl);
        }
    }
}