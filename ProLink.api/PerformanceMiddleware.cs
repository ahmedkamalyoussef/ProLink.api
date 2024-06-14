using System.Diagnostics;

namespace ProLink.api
{
    public class PerformanceMiddleware
    {
        private readonly ILogger<PerformanceMiddleware> _logger;
        private readonly RequestDelegate _next;
        public PerformanceMiddleware(ILogger<PerformanceMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            const int performanceTimeLog = 500;
            var sw = new Stopwatch();
            sw.Start();
            await _next(context);
            sw.Stop();
            if(sw.ElapsedMilliseconds > performanceTimeLog)
            {

                _logger.LogWarning("request {method} {path} took about {elapsed} ms",
                context.Request?.Method,
                context.Request.Path.Value,
                sw.ElapsedMilliseconds);
            }

        }
    }
}
