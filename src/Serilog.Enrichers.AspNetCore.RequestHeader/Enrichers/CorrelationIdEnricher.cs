using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers
{
    internal class CorrelationIdEnricher : ILogEventEnricher
    {
        private const string ItemKey = "Serilog_AspNetCoreRequestHeaderCorrelationId";
        private const string PropertyName = "CorrelationId";
        private readonly string _key;
        private readonly bool _generatedToHeader;
        private readonly string _generatedFormat;
        private readonly IHttpContextAccessor _contextAccessor;

        public CorrelationIdEnricher(
            string key,
            bool generatedToHeader,
            string generatedNewIdFormat)
            : this (key, generatedToHeader, generatedNewIdFormat, new HttpContextAccessor())
        { }

        internal CorrelationIdEnricher(
            string key,
            bool generatedToHeader,
            string generatedNewIdFormat,
            IHttpContextAccessor contextAccessor)
        {
            _key = key;
            _generatedToHeader = generatedToHeader;
            _generatedFormat = generatedNewIdFormat;
            _contextAccessor = contextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var httpContext = _contextAccessor.HttpContext;
            if (httpContext == null)
            { return; }

            if (httpContext.Items[ItemKey] is LogEventProperty logEventProperty)
            {
                logEvent.AddPropertyIfAbsent(logEventProperty);
                return;
            }

            var header = httpContext.Request.Headers[_key].ToString();
            string correlationId = "";
            if (string.IsNullOrWhiteSpace(header))
            {
                correlationId = Guid.NewGuid().ToString(_generatedFormat);
                if (_generatedToHeader)
                { httpContext.Request.Headers[_key] = correlationId; }
            }
            else
            { correlationId = header; }

            var correlationIdProperty = new LogEventProperty(PropertyName, new ScalarValue(correlationId));
            logEvent.AddOrUpdateProperty(correlationIdProperty);

            httpContext.Items.Add(ItemKey, correlationIdProperty);
        }
    }
}
