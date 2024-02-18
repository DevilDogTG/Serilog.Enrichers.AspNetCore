using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Serilog.Enrichers
{
    public class CorrelationIdEnricher : ILogEventEnricher
    {

        //private const string CorrelationIdItemKey = "Serilog_CorrelationId";
        //private const string PropertyName = "CorrelationId";
        //private readonly string _headerKey;
        //private readonly bool _addValueIfHeaderAbsence;
        //private readonly bool _addCorrelationToHeader;
        //private readonly string _correlationIdFormat;
        //private readonly IHttpContextAccessor _contextAccessor;
        //public CorrelationIdEnricher(string headerKey, bool addValueIfHeaderAbsence, bool addCorrelationToHeader, string correlationIdFormat)
        //    : this(headerKey, addValueIfHeaderAbsence, addCorrelationToHeader, correlationIdFormat, new HttpContextAccessor())
        //{
        //}

        //internal CorrelationIdEnricher(string headerKey, bool addValueIfHeaderAbsence, bool addCorrelationToHeader, string correlationIdFormat, IHttpContextAccessor contextAccessor)
        //{
        //    _headerKey = headerKey;
        //    _addValueIfHeaderAbsence = addValueIfHeaderAbsence;
        //    _addCorrelationToHeader = addCorrelationToHeader;
        //    _correlationIdFormat = correlationIdFormat;
        //    _contextAccessor = contextAccessor;
        //}
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            throw new NotImplementedException();
        }
    }
}
