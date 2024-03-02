using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers.AspNetCore.RequestHeader.Enrichers
{
    internal class RequestHeaderEnricher : ILogEventEnricher
    {
        private const string PrefixKey = "Serilog_AspNetCoreRequestHeader";
        private readonly string ItemKey;
        private readonly string key;
        private readonly string propertyName;
        private readonly IHttpContextAccessor contextAccessor;

        public RequestHeaderEnricher(
            string _key,
            string _propertyName) : this(_key, _propertyName, new HttpContextAccessor())
        {
        }

        internal RequestHeaderEnricher(
            string _key,
            string _propertyName,
            IHttpContextAccessor _contextAccessor)
        {
            key = _key;
            propertyName = _propertyName;
            contextAccessor = _contextAccessor;
            ItemKey = $"{PrefixKey}_{key}";
        }
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var httpContext = contextAccessor.HttpContext;
            if (httpContext == null)
            { return; }

            if (httpContext.Items[ItemKey] is LogEventProperty logEventProperty)
            {
                logEvent.AddPropertyIfAbsent(logEventProperty);
                return;
            }

            var header = httpContext.Request.Headers[key].ToString();
            var EventIdProperty = new LogEventProperty(propertyName, new ScalarValue(header));
            logEvent.AddOrUpdateProperty(EventIdProperty);

            httpContext.Items.Add(ItemKey, EventIdProperty);
        }
    }
}
