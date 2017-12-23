using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Enrichers
{
    public class DynamicEnricher : ILogEventEnricher
    {

        private string _propertyName;
        private bool _enrichWhenNullOrEmptyString;
        private LogEventLevel _minimumLevel;
        private bool _destructureObjects;

        public Func<object> ValueProvider { get; set; }
        
        public DynamicEnricher(string propertyName, 
                Func<object> valueProvider,
                bool destructureObjects=false,
                bool enrichWhenNullOrEmptyString = true,
                LogEventLevel minimumLevel = LogEventLevel.Verbose) : base()
        {
            ValueProvider = valueProvider;
            _propertyName = propertyName;
            _enrichWhenNullOrEmptyString = enrichWhenNullOrEmptyString;
            _minimumLevel = minimumLevel;
            _destructureObjects = destructureObjects;
        }
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var logValue = ValueProvider();

            if ((_enrichWhenNullOrEmptyString || (logValue !=null || (logValue is String && !String.IsNullOrEmpty((string)logValue) ))) && logEvent.Level>=_minimumLevel)
            {
                var prop = propertyFactory.CreateProperty(_propertyName,logValue, _destructureObjects);
                logEvent.AddPropertyIfAbsent(prop);
               
            } 
        }
    }
}
