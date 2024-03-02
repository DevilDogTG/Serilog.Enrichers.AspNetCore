using Serilog.Configuration;
using Serilog.Enrichers;
using Serilog.Enrichers.AspNetCore.RequestHeader.Enrichers;

namespace Serilog
{
    public static class LoggerConfigurationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrichmentConfiguration"></param>
        /// <param name="keyName">CorrelationId request header key name</param>
        /// <param name="generatedWhenNotExist">Add CorrelationId to request header when not exists</param>
        /// <param name="generatedFormat">GUID ToString format</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static LoggerConfiguration WithHeaderCorrelationId(
            this LoggerEnrichmentConfiguration enrichmentConfiguration,
            string keyName = "x-correlation-id",
            string propertyName = "CorrelationId",
            bool generatedWhenNotExist = true,
            string generatedFormat = "N")
        {
            if (enrichmentConfiguration == null)
            {
                throw new ArgumentNullException(nameof(enrichmentConfiguration));
            }

            return enrichmentConfiguration.With(new CorrelationIdEnricher(keyName, propertyName, generatedWhenNotExist, generatedFormat));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrichmentConfiguration"></param>
        /// <param name="keyName">Http Header key</param>
        /// <param name="propertyName">Property using on output</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static LoggerConfiguration WithHeaderKey(
            this LoggerEnrichmentConfiguration enrichmentConfiguration,
            string keyName,
            string propertyName = "")
        {
            if (enrichmentConfiguration == null)
            {
                throw new ArgumentNullException(nameof(enrichmentConfiguration));
            }
            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new ArgumentNullException(nameof(keyName));
            }
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                propertyName = keyName;
            }

            return enrichmentConfiguration.With(new RequestHeaderEnricher(keyName, propertyName));
        }
    }
}
