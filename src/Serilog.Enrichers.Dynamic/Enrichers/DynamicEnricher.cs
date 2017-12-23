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
        private bool _logNullValue;
        public Func<string> Fx { get; set; }
        
        public DynamicEnricher( Func<string> fx,string propertyName = "DynamicProperty", bool logNullValue=true) : base()
        {
            Fx = fx;
            _propertyName = propertyName;
            _logNullValue = logNullValue;
        }
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var logValue = Fx();
            if (!string.IsNullOrEmpty(logValue) || _logNullValue)
            {
                var prop = propertyFactory.CreateProperty(_propertyName,logValue);
                logEvent.AddPropertyIfAbsent(prop);
               
            } 
        }
    }
}
