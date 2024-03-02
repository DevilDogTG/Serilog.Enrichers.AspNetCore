using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers
{
    internal class CorrelationIdEnricher : ILogEventEnricher
    {
        private const string ItemKey = "Serilog_AspNetCoreRequestHeaderCorrelationId";
        private readonly string propertyName;
        private readonly string key;
        private readonly bool generatedToHeader;
        private readonly string generatedFormat;
        private readonly IHttpContextAccessor contextAccessor;

        public CorrelationIdEnricher(
            string _key,
            string _propertyName,
            bool _generatedToHeader,
            string _generatedNewIdFormat)
            : this (_key, _propertyName, _generatedToHeader, _generatedNewIdFormat, new HttpContextAccessor())
        { }

        internal CorrelationIdEnricher(
            string _key,
            string _propertyName,
            bool _generatedToHeader,
            string _generatedNewIdFormat,
            IHttpContextAccessor _contextAccessor)
        {
            key = _key;
            propertyName = _propertyName;
            generatedToHeader = _generatedToHeader;
            generatedFormat = _generatedNewIdFormat;
            contextAccessor = _contextAccessor;
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
            string correlationId = "";
            if (string.IsNullOrWhiteSpace(header))
            {
                correlationId = Guid.NewGuid().ToString(generatedFormat);
                if (generatedToHeader)
                { httpContext.Request.Headers[key] = correlationId; }
            }
            else
            { correlationId = header; }

            var correlationIdProperty = new LogEventProperty(propertyName, new ScalarValue(correlationId));
            logEvent.AddOrUpdateProperty(correlationIdProperty);

            httpContext.Items.Add(ItemKey, correlationIdProperty);
        }
    }
}
