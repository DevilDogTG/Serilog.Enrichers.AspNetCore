using Serilog.Configuration;
using Serilog.Enrichers;

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
            bool generatedWhenNotExist = true,
            string generatedFormat = "N")
        {
            if (enrichmentConfiguration == null)
            {
                throw new ArgumentNullException(nameof(enrichmentConfiguration));
            }

            return enrichmentConfiguration.With(new CorrelationIdEnricher(keyName, generatedWhenNotExist, generatedFormat));
        }
    }
}
